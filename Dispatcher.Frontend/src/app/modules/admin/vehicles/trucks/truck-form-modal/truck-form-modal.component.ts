import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import {
  TruckDto,
  CreateTruckRequest,
  UpdateTruckRequest,
} from '../../../../../core/models/truck.model';
import { VehicleStatusDto } from '../../../../../core/models/vehicle-status.model';

export type TruckModalMode = 'create' | 'edit' | 'view';

@Component({
  selector: 'app-truck-form-modal',
  templateUrl: './truck-form-modal.component.html',
  styleUrls: ['./truck-form-modal.component.css'],
  standalone: false,
})
export class TruckFormModalComponent implements OnChanges {
  @Input() open = false;
  @Input() mode: TruckModalMode = 'create';
  @Input() truck: TruckDto | null = null;
  @Input() statusOptions: VehicleStatusDto[] = [];

  @Output() closed = new EventEmitter<void>();
  @Output() submitted = new EventEmitter<CreateTruckRequest | UpdateTruckRequest>();

  form!: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      licensePlateNumber: ['', [Validators.required, Validators.maxLength(50)]],
      vinNumber: ['', [Validators.required, Validators.maxLength(50)]],
      make: ['', [Validators.required, Validators.maxLength(100)]],
      model: ['', [Validators.required, Validators.maxLength(100)]],
      year: [
        new Date().getFullYear(),
        [Validators.required, Validators.min(1900), Validators.max(2100)],
      ],
      capacity: [0, [Validators.required, Validators.min(0)]],
      engineCapacity: [0, [Validators.required, Validators.min(0)]],
      kw: [0, [Validators.required, Validators.min(0)]],
      vehicleStatusId: [null as number | null, [Validators.required]],
      lastMaintenanceDate: [null as Date | null],
      nextMaintenanceDate: [null as Date | null],
      registrationExpiration: [null as Date | null],
      insuranceExpiration: [null as Date | null],
      gpsDeviceId: [null as string | null],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['mode'] || changes['truck'] || changes['open']) {
      this.syncForm();
    }
  }

  get title(): string {
    if (this.mode === 'create') return 'Add Truck';
    if (this.mode === 'edit') return 'Edit Truck';
    return 'Truck Details';
  }

  get isView(): boolean {
    return this.mode === 'view';
  }

  onClose(): void {
    this.closed.emit();
  }

  onSubmit(): void {
    if (this.isView) return;

    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    const toYmd = (d: Date | null) => (d ? d.toISOString().slice(0, 10) : null);
    const v = this.form.getRawValue();

    const base: CreateTruckRequest = {
      licensePlateNumber: v.licensePlateNumber,
      vinNumber: v.vinNumber,
      make: v.make,
      model: v.model,
      year: Number(v.year),
      capacity: Number(v.capacity),
      lastMaintenanceDate: toYmd(v.lastMaintenanceDate),
      nextMaintenanceDate: toYmd(v.nextMaintenanceDate),
      registrationExpiration: toYmd(v.registrationExpiration),
      insuranceExpiration: toYmd(v.insuranceExpiration),
      gpsDeviceId: v.gpsDeviceId || null,
      vehicleStatusId: Number(v.vehicleStatusId),
      engineCapacity: Number(v.engineCapacity),
      kw: Number(v.kw),
    };

    if (this.mode === 'create') {
      this.submitted.emit(base);
    } else if (this.mode === 'edit' && this.truck) {
      const payload: UpdateTruckRequest = { id: this.truck.id, ...base };
      this.submitted.emit(payload);
    }
  }

  // ---------------- public helpers for date fields ----------------

  /**
   * Otvara Angular Material datepicker kad klikneš na ikonicu.
   */
  openPicker(picker: any): void {
    if (this.isView) return;
    if (picker && typeof picker.open === 'function') {
      picker.open();
    }
  }

  /**
   * Kad korisnik izabere datum iz datepickera.
   */
  onDatePicked(
    value: Date | null,
    controlName:
      'lastMaintenanceDate' |
      'nextMaintenanceDate' |
      'registrationExpiration' |
      'insuranceExpiration'
  ): void {
    if (this.isView) return;
    this.form.get(controlName)?.setValue(value ?? null);
  }

  /**
   * Kad korisnik ručno upiše datum u input (format MM/DD/YYYY).
   */
  onDateTextChange(
    value: string,
    controlName:
      'lastMaintenanceDate' |
      'nextMaintenanceDate' |
      'registrationExpiration' |
      'insuranceExpiration'
  ): void {
    if (this.isView) return;

    const trimmed = (value || '').trim();
    if (!trimmed) {
      this.form.get(controlName)?.setValue(null);
      return;
    }

    const parts = trimmed.split('/');
    if (parts.length === 3) {
      const [mmStr, ddStr, yyyyStr] = parts;
      const mm = Number(mmStr);
      const dd = Number(ddStr);
      const yyyy = Number(yyyyStr);

      if (!isNaN(mm) && !isNaN(dd) && !isNaN(yyyy)) {
        const d = new Date(yyyy, mm - 1, dd);
        if (!isNaN(d.getTime())) {
          this.form.get(controlName)?.setValue(d);
          return;
        }
      }
    }

    console.warn('Invalid date format for', controlName, value);
  }

  // ---------------- private helpers ----------------

  private syncForm(): void {
    // default vrijednosti
    this.form.reset({
      licensePlateNumber: '',
      vinNumber: '',
      make: '',
      model: '',
      year: new Date().getFullYear(),
      capacity: 0,
      engineCapacity: 0,
      kw: 0,
      vehicleStatusId: null,
      lastMaintenanceDate: null,
      nextMaintenanceDate: null,
      registrationExpiration: null,
      insuranceExpiration: null,
      gpsDeviceId: null,
    });

    // ako edit/view i imamo truck → popuni formu
    if (this.mode !== 'create' && this.truck) {
      console.log('TRUCK IN MODAL', this.truck);

      // Probaj pronaći status po nazivu, ako ID nedostaje
      const matchedStatus = this.statusOptions.find(
        s =>
          (s.statusName ?? '').toLowerCase() ===
          (this.truck?.vehicleStatusName ?? '').toLowerCase()
      );

      this.form.patchValue({
        licensePlateNumber: this.truck.licensePlateNumber ?? '',
        vinNumber: this.truck.vinNumber ?? '',
        make: this.truck.make ?? '',
        model: this.truck.model ?? '',
        year: this.truck.year ?? new Date().getFullYear(),
        capacity: this.truck.capacity ?? 0,
        engineCapacity: this.truck.engineCapacity ?? 0,
        kw: this.truck.kw ?? 0,

        // ako backend vrati vehicleStatusId – koristimo njega,
        // inače uzmi id iz statusOptions po imenu
        vehicleStatusId:
          this.truck.vehicleStatusId ?? matchedStatus?.id ?? null,

        lastMaintenanceDate: this.parseBackendDate(this.truck.lastMaintenanceDate),
        nextMaintenanceDate: this.parseBackendDate(this.truck.nextMaintenanceDate),
        registrationExpiration: this.parseBackendDate(this.truck.registrationExpiration),
        insuranceExpiration: this.parseBackendDate(this.truck.insuranceExpiration),

        gpsDeviceId: this.truck.gpsDeviceId ?? null,
      });

      console.log('PATCHED FORM VALUE', this.form.value);
    }

    // view: disable sve
    if (this.isView) {
      this.form.disable({ emitEvent: false });
    } else {
      this.form.enable({ emitEvent: false });
    }

    // edit: zaključaš samo određena polja
    if (this.mode === 'edit') {
      this.form.get('vinNumber')?.disable({ emitEvent: false });
      this.form.get('make')?.disable({ emitEvent: false });
      this.form.get('model')?.disable({ emitEvent: false });
      this.form.get('year')?.disable({ emitEvent: false });
    }
  }

  private parseBackendDate(value: string | null): Date | null {
    if (!value) return null;

    let s = value.trim();

    // samo yyyy-MM-dd
    if (/^\d{4}-\d{2}-\d{2}$/.test(s)) {
      const dPlain = new Date(s);
      return Number.isNaN(dPlain.getTime()) ? null : dPlain;
    }

    // "2025-10-22 23:56:27.4423254" → "2025-10-22T23:56:27.442"
    s = s.replace(' ', 'T');
    s = s.replace(/\.(\d{3})\d+$/, '.$1');

    const d = new Date(s);
    return Number.isNaN(d.getTime()) ? null : d;
  }
}