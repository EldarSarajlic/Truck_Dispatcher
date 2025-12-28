import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, of } from 'rxjs';
import { takeUntil, catchError, tap, switchMap, startWith } from 'rxjs/operators';

import { TrailersService } from '../../../../api-services/vehicles/trailers/trailers-api.service';
import { VehicleStatusService } from '../../../../api-services/vehicles/vehicle-status.service';

import {
  ListTrailerQueryDto,
  CreateTrailerCommand,
  UpdateTrailerCommand,
  ChangeTrailerStatusCommand
} from '../../../../api-services/vehicles/trailers/trailers-api.model';

import { VehicleStatusDto } from '../../../../core/models/vehicle-status.model';

@Component({
  selector: 'app-trailers',
  templateUrl: './trailers.component.html',
  standalone: false,
  styleUrls: ['./trailers.component.css']
})
export class TrailersComponent implements OnInit, OnDestroy {
  loading = false;

  // filters
  statusId: number | null = null;
  availableStatusOptions: VehicleStatusDto[] = [];

  // data
  rows: ListTrailerQueryDto[] = [];
  filtered: ListTrailerQueryDto[] = [];

  // modal
  modalOpen = false;
  modalMode: 'create' | 'edit' | 'view' = 'create';
  selectedTrailer: ListTrailerQueryDto | null = null;

  // inline status edit
  editingStatusTrailerId: number | null = null;

  private readonly destroy$ = new Subject<void>();
  private readonly statusId$ = new Subject<number | null>();

  constructor(
    private trailersService: TrailersService,
    private vehicleStatusService: VehicleStatusService
  ) {}

  ngOnInit(): void {
    // load vehicle status options
    this.vehicleStatusService.getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: data => this.availableStatusOptions = (data ?? []).sort((a,b) => (a.statusName ?? '').localeCompare(b.statusName ?? '')),
        error: () => this.availableStatusOptions = []
      });

    // load trailers
    this.statusId$
      .pipe(
        startWith(null),
        tap(() => this.loading = true),
        switchMap(() =>
          this.trailersService.list()
            .pipe(
              catchError(err => {
                console.error('Error loading trailers', err);
                return of({ items: [] } as any);
              })
            )
        ),
        takeUntil(this.destroy$)
      )
      .subscribe(res => {
        this.rows = res.items ?? [];
        this.applyFilters();
        this.loading = false;
      });

    this.refresh();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  refresh(): void {
    this.statusId$.next(this.statusId);
  }

  applyFilters(): void {
    if (this.statusId != null) {
      this.filtered = this.rows.filter(x => x.vehicleStatusId === this.statusId);
    } else {
      this.filtered = [...this.rows];
    }
  }

  onStatusIdChange(value: number | null): void {
    this.statusId = value;
    this.refresh();
  }

  clearFilters(): void {
    this.statusId = null;
    this.refresh();
  }

  // modal handlers
  openCreateTrailer(): void {
    this.modalMode = 'create';
    this.selectedTrailer = null;
    this.modalOpen = true;
  }

  openEditTrailer(trailer: ListTrailerQueryDto): void {
    this.modalMode = 'edit';
    this.selectedTrailer = trailer;
    this.modalOpen = true;
  }

  openViewTrailer(trailer: ListTrailerQueryDto): void {
    this.modalMode = 'view';
    this.selectedTrailer = trailer;
    this.modalOpen = true;
  }

  closeModal(): void {
    this.modalOpen = false;
    this.selectedTrailer = null;
  }

  onTrailerFormSubmitted(payload: CreateTrailerCommand | UpdateTrailerCommand): void {
    if ('id' in payload) {
      this.trailersService.update(payload.id, payload).subscribe({
        next: () => { this.closeModal(); this.refresh(); },
        error: err => console.error(err)
      });
    } else {
      this.trailersService.create(payload).subscribe({
        next: () => { this.closeModal(); this.refresh(); },
        error: err => console.error(err)
      });
    }
  }

  // row actions
  view(trailer: ListTrailerQueryDto): void { this.openViewTrailer(trailer); }
  edit(trailer: ListTrailerQueryDto): void { this.openEditTrailer(trailer); }
  delete(trailer: ListTrailerQueryDto): void {
    if (!confirm(`Da li sigurno želiš obrisati prikolicu ${trailer.licensePlateNumber}?`)) return;
    this.trailersService.delete(trailer.id).subscribe({
      next: () => this.refresh(),
      error: err => console.error(err)
    });
  }

  // inline status
  changeStatus(trailer: ListTrailerQueryDto): void {
    this.editingStatusTrailerId = trailer.id;
  }

  changeStatusForTrailer(trailer: ListTrailerQueryDto, newStatusId: number): void {
    if (trailer.vehicleStatusId === newStatusId) {
      this.editingStatusTrailerId = null;
      return;
    }
    this.trailersService.changeStatus(trailer.id, { vehicleStatusId: newStatusId }).subscribe({
      next: () => { this.editingStatusTrailerId = null; this.refresh(); },
      error: err => console.error(err)
    });
  }

  getStatusBadgeClass(status: string | null): string {
    const s = (status ?? '').toLowerCase();
    if (s.includes('available')) return 'badge-success';
    if (s.includes('transit')) return 'badge-info';
    if (s.includes('maintenance')) return 'badge-warning';
    if (s.includes('out')) return 'badge-error';
    return 'badge-ghost';
  }

  getStatusDotClass(status: string | null): string {
    const s = (status ?? '').toLowerCase();
    if (s.includes('available')) return 'bg-success';
    if (s.includes('transit')) return 'bg-info';
    if (s.includes('maintenance')) return 'bg-warning';
    if (s.includes('out')) return 'bg-error';
    return 'bg-base-content/40';
  }

   formatCapacity(value: number): string {
    if (value === null || value === undefined) return '-';
    return `${value.toFixed(1)} tons`;
  }

  formatDate(value: string | null): string {
    const d = this.toDate(value);
    if (!d) return '-';
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
    private toDate(value: string | null): Date | null {
    if (!value) return null;
    const d = new Date(value);
    return isNaN(d.getTime()) ? null : d;
  }
}
