import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  ListTrailerQueryDto,
  CreateTrailerCommand,
  UpdateTrailerCommand
} from '../../../../../api-services/vehicles/trailers/trailers-api.model';
import { VehicleStatusDto } from '../../../../../core/models/vehicle-status.model';

export type TrailerModalMode = 'create' | 'edit' | 'view';

@Component({
  selector: 'app-trailer-form-modal',
  templateUrl: './trailer-form-modal.component.html',
  styleUrls: ['./trailer-form-modal.component.css'],
  standalone: false,
})
export class TrailerFormModalComponent implements OnChanges {
  @Input() open = false;
  @Input() mode: TrailerModalMode = 'create';
  @Input() trailer: ListTrailerQueryDto | null = null;
  @Input() statusOptions: VehicleStatusDto[] = [];

  @Output() closed = new EventEmitter<void>();
  @Output() submitted = new EventEmitter<CreateTrailerCommand | UpdateTrailerCommand>();

  form!: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      licensePlateNumber: ['', [Validators.required, Validators.maxLength(15)]],
      make: ['', [Validators.required, Validators.maxLength(50)]],
      model: ['', [Validators.required, Validators.maxLength(50)]],
      year: [new Date().getFullYear(), [Validators.required, Validators.min(1900), Validators.max(2100)]],
      type: ['', [Validators.required, Validators.maxLength(50)]],

      length: [0, [Validators.required, Validators.min(0)]],
      capacity: [0, [Validators.required, Validators.min(0)]],

      vehicleStatusId: [null as number | null, [Validators.required]],

      registrationExpiration: [null as string | null],
      insuranceExpiration: [null as string | null],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['mode'] || changes['trailer'] || changes['open']) {
      this.syncForm();
    }
  }

  get title(): string {
    if (this.mode === 'create') return 'Add Trailer';
    if (this.mode === 'edit') return 'Edit Trailer';
    return 'Trailer Details';
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





    const base: CreateTrailerCommand = {
      licensePlateNumber: v.licensePlateNumber,
      make: v.make,
      model: v.model,
      year: Number(v.year),
      type: v.type,
      length: Number(v.length),
      capacity: Number(v.capacity),
      vehicleStatusId: Number(v.vehicleStatusId),
      registrationExpiration: v.registrationExpiration || null,
      insuranceExpiration: v.insuranceExpiration || null,
      
    };

    if (this.mode === 'create') {
      this.submitted.emit(base);
    } else if (this.mode === 'edit' && this.trailer) {
      const payload: UpdateTrailerCommand = { id: this.trailer.id, ...base };
      this.submitted.emit(payload);
    }
  }

  private syncForm(): void {
    // reset default values
    this.form.reset({
      licensePlateNumber: '',
      make: '',
      model: '',
      year: new Date().getFullYear(),
      type: '',
      length: 0,
      capacity: 0,
      vehicleStatusId: null,
      registrationExpiration: null,
      insuranceExpiration: null,
    });

    // patch values when edit/view
    if (this.mode !== 'create' && this.trailer) {
      this.form.patchValue({
        licensePlateNumber: this.trailer.licensePlateNumber ?? '',
        make: this.trailer.make ?? '',
        model: this.trailer.model ?? '',
        year: this.trailer.year ?? new Date().getFullYear(),
        type: this.trailer.type ?? '',
        length: this.trailer.length ?? 0,
        capacity: this.trailer.capacity ?? 0,
        vehicleStatusId: this.trailer.vehicleStatusId ?? null,
        registrationExpiration: this.trailer.registrationExpiration ?? null,
        insuranceExpiration: this.trailer.insuranceExpiration ?? null,
      });
    }

    // View mode: lock everything
    if (this.isView) {
      this.form.disable({ emitEvent: false });
    } else {
      this.form.enable({ emitEvent: false });
    }

    // Edit mode: optionally lock fields
    if (this.mode === 'edit') {
      this.form.get('licensePlateNumber')?.disable({ emitEvent: false });
      this.form.get('year')?.disable({ emitEvent: false });
    }
  }
}
