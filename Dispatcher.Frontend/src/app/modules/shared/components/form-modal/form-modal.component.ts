import {
  Component, EventEmitter, HostListener,
  Input, OnChanges, OnDestroy, Output, SimpleChanges, inject,
} from '@angular/core';
import {
  AbstractControl, FormBuilder, FormGroup,
  ValidatorFn, Validators,
} from '@angular/forms';
import { Subject } from 'rxjs';

export interface FormFieldOption {
  label: string;
  value: string | number;
}

export interface FormField {
  name:         string;
  label:        string;
  type:         'text' | 'email' | 'tel' | 'password' | 'date' | 'select';
  placeholder?: string;
  required?:    boolean;
  validators?:  ValidatorFn[];
  options?:     FormFieldOption[];     // for type === 'select' (can be updated dynamically)
  halfWidth?:   boolean;               // pair with adjacent halfWidth field → 2-col row
  phoneFormat?: string;                // e.g. '+XXX XX XXX XXX' — auto-formats tel input on type
  phonePrefix?: string;               // digit-only country code e.g. '387' — kept fixed/undeletable
  // Dynamic select support
  isLoadingOptions?: boolean;          // show spinner in dropdown while fetching options
  onValueChange?:    (value: string | number, form: FormGroup) => void;
}

@Component({
  selector:    'app-form-modal',
  standalone:  false,
  templateUrl: './form-modal.component.html',
  styleUrl:    './form-modal.component.scss',
})
export class FormModalComponent implements OnChanges, OnDestroy {

  // ── Inputs ────────────────────────────────────────────────────────────────
  @Input({ required: true }) open!:           boolean;
  @Input({ required: true }) fields!:         FormField[];
  @Input() title           = 'Form';
  @Input() titleIcon       = 'ph-note-pencil';
  @Input() submitLabel     = 'Submit';
  @Input() isSubmitting    = false;
  @Input() submitError:    string | null = null;
  @Input() groupValidators: ValidatorFn[] = [];

  // ── Outputs ───────────────────────────────────────────────────────────────
  @Output() closed    = new EventEmitter<void>();
  @Output() submitted = new EventEmitter<Record<string, any>>();

  // ── Internal state ────────────────────────────────────────────────────────
  form!:             FormGroup;
  openDropdown:      string | null = null;  // name of the currently open select field
  rows:              FormField[][] = [];

  private lastFieldKey              = '';
  private readonly destroyed$       = new Subject<void>();
  private readonly fb               = inject(FormBuilder);

  // ── Lifecycle ─────────────────────────────────────────────────────────────
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fields'] && this.fields) {
      // Only rebuild the form (resetting values) when the field structure changes.
      // When only options/isLoadingOptions update, skip buildForm so values are preserved.
      const key = this.fields.map(f => f.name).join(',');
      if (key !== this.lastFieldKey) {
        this.lastFieldKey = key;
        this.buildForm();
      }
      this.buildRows();
    }
    if (changes['open']?.currentValue === true) {
      this.reset();
    }
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  // ── Dropdown ──────────────────────────────────────────────────────────────
  @HostListener('document:click')
  closeDropdown(): void { this.openDropdown = null; }

  toggleDropdown(name: string, e: Event): void {
    e.stopPropagation();
    this.openDropdown = this.openDropdown === name ? null : name;
  }

  selectOption(fieldName: string, value: string | number, e: Event): void {
    e.stopPropagation();
    this.form.get(fieldName)?.setValue(value);
    this.form.get(fieldName)?.markAsTouched();
    this.openDropdown = null;

    const field = this.fields.find(f => f.name === fieldName);
    field?.onValueChange?.(value, this.form);
  }

  optionLabel(field: FormField): string {
    const val = this.form.get(field.name)?.value;
    if (!val) return '';
    return field.options?.find(o => o.value === val)?.label ?? String(val);
  }

  // ── Tel formatter ─────────────────────────────────────────────────────────
  onTelKeydown(format: string, prefix: string, e: KeyboardEvent): void {
    if (e.key !== 'Backspace' && e.key !== 'Delete') return;
    const input     = e.target as HTMLInputElement;
    const prefixEnd = this.formattedPrefixEnd(format, prefix);
    const cursor    = input.selectionStart ?? 0;
    const selEnd    = input.selectionEnd   ?? 0;
    const samePos   = cursor === selEnd;
    if ((e.key === 'Backspace' && samePos && cursor <= prefixEnd) ||
        (e.key === 'Delete'    && samePos && cursor <  prefixEnd)) {
      e.preventDefault();
    }
  }

  onTelInput(fieldName: string, format: string, prefix: string, e: Event): void {
    const input    = e.target as HTMLInputElement;
    const allDigits = input.value.replace(/\D/g, '');
    // Ensure prefix digits are always present
    const safe      = allDigits.length <= prefix.length
      ? prefix
      : prefix + allDigits.slice(prefix.length);
    const formatted = this.applyPhoneFormat(safe, format);
    input.value     = formatted;
    this.form.get(fieldName)?.setValue(formatted, { emitEvent: false });
  }

  private formattedPrefixEnd(format: string, prefixDigits: string): number {
    let di = 0, i = 0;
    for (; i < format.length && di < prefixDigits.length; i++) {
      if (format[i] === 'X') di++;
    }
    return i;
  }

  private applyPhoneFormat(digits: string, format: string): string {
    let di = 0, result = '';
    for (const ch of format) {
      if (di >= digits.length) break;
      result += ch === 'X' ? digits[di++] : ch;
    }
    return result;
  }

  // ── Date picker ───────────────────────────────────────────────────────────
  setDateValue(fieldName: string, value: string): void {
    this.form.get(fieldName)?.setValue(value);
    this.form.get(fieldName)?.markAsTouched();
  }

  // ── Actions ───────────────────────────────────────────────────────────────
  close(): void { this.closed.emit(); }

  submit(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.isSubmitting) return;
    this.submitted.emit(this.form.getRawValue());
  }

  // ── Template helpers ──────────────────────────────────────────────────────
  fieldInvalid(name: string): boolean {
    const ctrl = this.form?.get(name);
    return !!(ctrl?.touched && ctrl?.invalid);
  }

  // ── Private ───────────────────────────────────────────────────────────────
  private buildForm(): void {
    const controls: Record<string, any> = {};
    for (const field of this.fields) {
      const validators: ValidatorFn[] = [...(field.validators ?? [])];
      if (field.required) validators.unshift(Validators.required);
      controls[field.name] = ['', validators];
    }
    this.form = this.fb.group(controls, { validators: this.groupValidators });
  }

  private buildRows(): void {
    this.rows = [];
    const fields = [...this.fields];
    let i = 0;
    while (i < fields.length) {
      if (fields[i].halfWidth && fields[i + 1]?.halfWidth) {
        this.rows.push([fields[i], fields[i + 1]]);
        i += 2;
      } else {
        this.rows.push([fields[i]]);
        i++;
      }
    }
  }

  private reset(): void {
    this.form?.reset();
    this.openDropdown = null;
  }
}
