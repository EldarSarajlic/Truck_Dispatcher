import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

// Ako već imaš modele u core/models, koristi njih.
// Ako nemaš, možeš ostaviti ove lokalne tipove ili ih prebaci u core/models.
export interface TrailerDto {
  id: number;
  licensePlateNumber: string;
  make: string;
  model: string;
  year: number;
  type: string;
  registrationExpiration: string | null;
  insuranceExpiration: string | null;
  length: number;
  capacity: number;
  vehicleStatusId: number;
  vehicleStatusName?: string;
}

export interface CreateTrailerRequest {
  licensePlateNumber: string;
  make: string;
  model: string;
  year: number;
  type: string;
  registrationExpiration: string | null;
  insuranceExpiration: string | null;
  length: number;
  capacity: number;
  vehicleStatusId: number;
}

export interface UpdateTrailerRequest extends CreateTrailerRequest {
  id: number;
}

export interface VehicleStatusDto {
  id: number;
  statusName: string;
}

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
  @Input() trailer: TrailerDto | null = null;
  @Input() statusOptions: VehicleStatusDto[] = [];

  @Output() closed = new EventEmitter<void>();
  @Output() submitted = new EventEmitter<CreateTrailerRequest | UpdateTrailerRequest>();

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

    const base: CreateTrailerRequest = {
      licensePlateNumber: this.form.value.licensePlateNumber,
      make: this.form.value.make,
      model: this.form.value.model,
      year: Number(this.form.value.year),
      type: this.form.value.type,

      length: Number(this.form.value.length),
      capacity: Number(this.form.value.capacity),

      registrationExpiration: this.form.value.registrationExpiration || null,
      insuranceExpiration: this.form.value.insuranceExpiration || null,

      vehicleStatusId: Number(this.form.value.vehicleStatusId),
    };

    if (this.mode === 'create') {
      this.submitted.emit(base);
    } else if (this.mode === 'edit' && this.trailer) {
      const payload: UpdateTrailerRequest = { id: this.trailer.id, ...base };
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

    // Edit mode: po želji zaključaš neke fieldove (kao truck)
    if (this.mode === 'edit') {
      // npr. tablice često ne mijenjaš:
      this.form.get('licensePlateNumber')?.disable({ emitEvent: false });
      // i year možda:
      this.form.get('year')?.disable({ emitEvent: false });
    }
  }
}
