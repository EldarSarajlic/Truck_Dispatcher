import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { TruckDto, CreateTruckRequest, UpdateTruckRequest } from '../../../../../core/models/truck.model';
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

private parseBackendDate(value: string | null): Date | null {
  if (!value) return null;

  // Accept: "2025-10-22 23:56:27.4423254" or "2025-10-22T23:56:27.4423254"
  let s = value.trim().replace(' ', 'T');

  // Trim fractional seconds to 3 digits (JS Date reliably supports milliseconds)
  s = s.replace(/\.(\d{3})\d+$/, '.$1');

  const d = new Date(s);
  return Number.isNaN(d.getTime()) ? null : d;
}




constructor(private fb: FormBuilder) {
  this.form = this.fb.group({
    licensePlateNumber: ['', [Validators.required, Validators.maxLength(50)]],
    vinNumber: ['', [Validators.required, Validators.maxLength(50)]],
    make: ['', [Validators.required, Validators.maxLength(100)]],
    model: ['', [Validators.required, Validators.maxLength(100)]],
    year: [new Date().getFullYear(), [Validators.required, Validators.min(1900), Validators.max(2100)]],
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

 // ... unutar klase TruckFormModalComponent

private syncForm(): void {
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

 if (this.mode !== 'create' && this.truck) {
  this.form.patchValue({
    licensePlateNumber: this.truck.licensePlateNumber ?? '',
    vinNumber: this.truck.vinNumber ?? '',
    make: this.truck.make ?? '',
    model: this.truck.model ?? '',
    year: this.truck.year ?? new Date().getFullYear(),
    capacity: this.truck.capacity ?? 0,
    engineCapacity: this.truck.engineCapacity ?? 0,
    kw: this.truck.kw ?? 0,
    vehicleStatusId: this.truck.vehicleStatusId ?? null,

    lastMaintenanceDate: this.parseBackendDate(this.truck.lastMaintenanceDate),
    nextMaintenanceDate: this.parseBackendDate(this.truck.nextMaintenanceDate),
    registrationExpiration: this.parseBackendDate(this.truck.registrationExpiration),
    insuranceExpiration: this.parseBackendDate(this.truck.insuranceExpiration),

    gpsDeviceId: this.truck.gpsDeviceId ?? null,
  });
}

  // View mode: lock everything
  if (this.isView) {
    this.form.disable({ emitEvent: false });
  } else {
    this.form.enable({ emitEvent: false });
  }

  // Edit mode: lock specific fields
  if (this.mode === 'edit') {
    this.form.get('vinNumber')?.disable({ emitEvent: false });
    this.form.get('make')?.disable({ emitEvent: false });
    this.form.get('model')?.disable({ emitEvent: false });
    this.form.get('year')?.disable({ emitEvent: false });
  }
}
}