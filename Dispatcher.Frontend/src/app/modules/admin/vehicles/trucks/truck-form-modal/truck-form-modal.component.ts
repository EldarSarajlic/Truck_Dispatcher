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
    lastMaintenanceDate: [null as string | null],
    nextMaintenanceDate: [null as string | null],
    registrationExpiration: [null as string | null],
    insuranceExpiration: [null as string | null],
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

    const base: CreateTruckRequest = {
      licensePlateNumber: this.form.value.licensePlateNumber,
      vinNumber: this.form.value.vinNumber,
      make: this.form.value.make,
      model: this.form.value.model,
      year: Number(this.form.value.year),
      capacity: Number(this.form.value.capacity),
      lastMaintenanceDate: this.form.value.lastMaintenanceDate || null,
      nextMaintenanceDate: this.form.value.nextMaintenanceDate || null,
      registrationExpiration: this.form.value.registrationExpiration || null,
      insuranceExpiration: this.form.value.insuranceExpiration || null,
      gpsDeviceId: this.form.value.gpsDeviceId || null,
      vehicleStatusId: Number(this.form.value.vehicleStatusId),
      engineCapacity: Number(this.form.value.engineCapacity),
      kw: Number(this.form.value.kw),
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
      lastMaintenanceDate: this.truck.lastMaintenanceDate ?? null,
      nextMaintenanceDate: this.truck.nextMaintenanceDate ?? null,
      registrationExpiration: this.truck.registrationExpiration ?? null,
      insuranceExpiration: this.truck.insuranceExpiration ?? null,
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