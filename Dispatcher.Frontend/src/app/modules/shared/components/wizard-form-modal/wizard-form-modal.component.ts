import {
  Component, ElementRef, EventEmitter, HostListener,
  Input, OnChanges, OnDestroy, Output, SimpleChanges, ViewChild, inject,
} from '@angular/core';
import { FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { FormField } from '../form-modal/form-modal.component';

export interface WizardStep {
  title: string;
  icon?:           string;
  fields:          FormField[];
  groupValidators?: ValidatorFn[];
}

@Component({
  selector:    'app-wizard-form-modal',
  standalone:  false,
  templateUrl: './wizard-form-modal.component.html',
  styleUrl:    './wizard-form-modal.component.scss',
})
export class WizardFormModalComponent implements OnChanges, OnDestroy {

  // ── Inputs ────────────────────────────────────────────────────────────────
  @Input({ required: true }) open!:        boolean;
  @Input({ required: true }) steps!:       WizardStep[];
  @Input() title        = 'Form';
  @Input() titleIcon    = 'ph-note-pencil';
  @Input() submitLabel  = 'Submit';
  @Input() isSubmitting = false;
  @Input() submitError: string | null = null;

  // ── Outputs ───────────────────────────────────────────────────────────────
  @Output() closed    = new EventEmitter<void>();
  @Output() submitted = new EventEmitter<Record<string, any>>();

  @ViewChild('bodyRef') bodyRef?: ElementRef<HTMLDivElement>;

  // ── State ─────────────────────────────────────────────────────────────────
  form!:           FormGroup;
  currentStep    = 0;
  openDropdown:  string | null = null;
  rows:            FormField[][] = [];
  datePickerOpen = false;
  spacerActive   = false;

  private lastStepsKey         = '';
  private readonly destroyed$  = new Subject<void>();
  private readonly fb          = inject(FormBuilder);

  // ── Derived getters ───────────────────────────────────────────────────────
  get step():        WizardStep { return this.steps[this.currentStep]; }
  get isFirstStep(): boolean    { return this.currentStep === 0; }
  get isLastStep():  boolean    { return this.currentStep === this.steps.length - 1; }

  // ── Lifecycle ─────────────────────────────────────────────────────────────
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['steps'] && this.steps) {
      const key = this.steps.map(s => s.fields.map(f => f.name).join(',')).join('|');
      if (key !== this.lastStepsKey) {
        this.lastStepsKey = key;
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
  closeDropdown(): void {
    this.openDropdown = null;
    this.checkSpacer();
  }

  toggleDropdown(name: string, e: Event): void {
    e.stopPropagation();
    this.openDropdown = this.openDropdown === name ? null : name;
    this.checkSpacer();
  }

  selectOption(fieldName: string, value: string | number, e: Event): void {
    e.stopPropagation();
    this.form.get(fieldName)?.setValue(value);
    this.form.get(fieldName)?.markAsTouched();
    this.openDropdown = null;
    this.checkSpacer();
    this.step.fields.find(f => f.name === fieldName)?.onValueChange?.(value, this.form);
  }

  onDatePickerToggled(open: boolean): void {
    this.datePickerOpen = open;
    this.checkSpacer();
  }

  private checkSpacer(): void {
    const el = this.bodyRef?.nativeElement;
    const hasOpen = this.datePickerOpen || this.openDropdown !== null;
    if (!hasOpen) { this.spacerActive = false; return; }
    if (!el) { this.spacerActive = true; return; }
    const remainingSpace = el.clientHeight - el.scrollHeight;
    this.spacerActive = remainingSpace < 290;
  }

  optionLabel(field: FormField): string {
    const val = this.form.get(field.name)?.value;
    if (!val && val !== 0) return '';
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
    const input     = e.target as HTMLInputElement;
    const allDigits = input.value.replace(/\D/g, '');
    const safe      = allDigits.length <= prefix.length
      ? prefix
      : prefix + allDigits.slice(prefix.length);
    const formatted = this.applyPhoneFormat(safe, format);
    input.value     = formatted;
    this.form.get(fieldName)?.setValue(formatted, { emitEvent: false });
  }

  // ── Date picker ───────────────────────────────────────────────────────────
  setDateValue(fieldName: string, value: string): void {
    this.form.get(fieldName)?.setValue(value);
    this.form.get(fieldName)?.markAsTouched();
  }

  // ── Validation helpers ────────────────────────────────────────────────────
  fieldInvalid(name: string): boolean {
    const ctrl = this.form?.get(name);
    return !!(ctrl?.touched && ctrl?.invalid);
  }

  // ── Navigation ────────────────────────────────────────────────────────────
  next(): void {
    let stepValid = true;
    for (const field of this.step.fields) {
      const ctrl = this.form.get(field.name);
      ctrl?.markAsTouched();
      if (ctrl?.invalid) stepValid = false;
    }
    if (!stepValid) return;
    this.currentStep++;
    this.buildRows();
  }

  back(): void {
    if (this.currentStep > 0) {
      this.currentStep--;
      this.buildRows();
    }
  }

  submit(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.isSubmitting) return;
    this.submitted.emit(this.form.getRawValue());
  }

  close(): void { this.closed.emit(); }

  // ── Private ───────────────────────────────────────────────────────────────
  private buildForm(): void {
    const controls: Record<string, any> = {};
    const allGroupValidators: ValidatorFn[] = [];

    for (const step of this.steps) {
      for (const field of step.fields) {
        const validators: ValidatorFn[] = [...(field.validators ?? [])];
        if (field.required) validators.unshift(Validators.required);
        controls[field.name] = ['', validators];
      }
      if (step.groupValidators) allGroupValidators.push(...step.groupValidators);
    }

    this.form = this.fb.group(controls, { validators: allGroupValidators });
  }

  private buildRows(): void {
    this.rows = [];
    const fields = [...this.step.fields];
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
    this.currentStep  = 0;
    this.openDropdown = null;
    this.buildRows();
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
}
