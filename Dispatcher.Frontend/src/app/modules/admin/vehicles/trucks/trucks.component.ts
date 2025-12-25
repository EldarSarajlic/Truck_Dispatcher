import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, combineLatest, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, startWith, switchMap, takeUntil, tap } from 'rxjs/operators';

import { TruckService } from '../../../../api-services/vehicles/truck.service';
import { VehicleStatusService } from '../../../../api-services/vehicles/vehicle-status.service';

import { TruckDto } from '../../../../core/models/truck.model';
import { VehicleStatusDto } from '../../../../core/models/vehicle-status.model';

type MaintenanceFilter = 'all' | 'dueSoon' | 'overdue';

@Component({
  selector: 'app-trucks',
  templateUrl: './trucks.component.html',
  standalone: false,
  styleUrls: ['./trucks.component.css'],
})
export class TrucksComponent implements OnInit, OnDestroy {
  loading = false;

  // filters
  search = '';
  statusId: number | null = null; // <- VehicleStatusId (T-2.3.5)
  maintenanceFilter: MaintenanceFilter = 'all';

  // dropdown data (from /VehicleStatuses)
  availableStatusOptions: VehicleStatusDto[] = [];

  // data
  rows: TruckDto[] = [];
  filtered: TruckDto[] = [];

  private readonly destroy$ = new Subject<void>();
  private readonly search$ = new Subject<string>();
  private readonly statusId$ = new Subject<number | null>();

  constructor(
    private truckService: TruckService,
    private vehicleStatusService: VehicleStatusService
  ) {}

  ngOnInit(): void {
    // Load status options for dropdown
    this.vehicleStatusService
  .getAll()
  .pipe(takeUntil(this.destroy$))
  .subscribe({
    next: (data: VehicleStatusDto[]) => {
  this.availableStatusOptions = (data ?? [])
    .slice()
    .sort((a: VehicleStatusDto, b: VehicleStatusDto) =>
      (a.statusName ?? '').localeCompare(b.statusName ?? '')
    );
},
    error: () => {
      this.availableStatusOptions = [];
    },
  });

    // Debounced server-side filtering: Search + StatusId
    combineLatest([
      this.search$.pipe(startWith(''), debounceTime(300), distinctUntilChanged()),
      this.statusId$.pipe(startWith(null), distinctUntilChanged()),
    ])
      .pipe(
        tap(() => (this.loading = true)),
        switchMap(([term, statusId]) =>
          this.truckService
            .getAll({
              search: term?.trim() || undefined,
              status: statusId, // <- sends Status=<id>
            })
            .pipe(catchError(() => of([] as TruckDto[])))
        ),
        takeUntil(this.destroy$)
      )
      .subscribe((data: TruckDto[]) => {
        this.rows = data ?? [];
        this.applyFilters(); // maintenance stays local
        this.loading = false;
      });

    // initial load
    this.refresh();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchChange(value: string): void {
    this.search = value;
    this.search$.next(value);
  }

  onStatusIdChange(value: number | null): void {
    this.statusId = value;
    this.statusId$.next(value);
  }

  refresh(): void {
    this.search$.next(this.search);
    this.statusId$.next(this.statusId);
  }

  load(): void {
    this.refresh();
  }

  clearFilters(): void {
    this.search = '';
    this.statusId = null;
    this.maintenanceFilter = 'all';
    this.refresh();
  }

  // Only maintenance filter is local now
  applyFilters(): void {
    this.filtered = this.rows.filter((t) => {
      if (this.maintenanceFilter !== 'all') {
        const d = this.toDate(t.nextMaintenanceDate);
        if (!d) return false;

        const now = new Date();
        const diffDays = this.daysBetween(now, d);

        if (this.maintenanceFilter === 'overdue' && diffDays >= 0) return false;
        if (this.maintenanceFilter === 'dueSoon') {
          if (!(diffDays >= 0 && diffDays <= 14)) return false;
        }
      }

      return true;
    });
  }

  view(t: TruckDto): void {
    console.log('view truck', t);
  }

  edit(t: TruckDto): void {
    console.log('edit truck', t);
  }

  changeStatus(t: TruckDto): void {
    console.log('change status', t);
  }

  // ---------- helpers ----------
  formatDate(value: string | null): string {
    const d = this.toDate(value);
    if (!d) return '-';
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  getDateBadgeClass(value: string | null): string {
    const d = this.toDate(value);
    if (!d) return 'badge-ghost';

    const now = new Date();
    const diffDays = this.daysBetween(now, d);

    if (diffDays < 0) return 'badge-error';
    if (diffDays <= 14) return 'badge-warning';
    return 'badge-success';
  }

  formatCapacity(value: number): string {
    if (value === null || value === undefined) return '-';
    return `${value.toFixed(1)} tons`;
  }

  formatMonthYear(value: string | null): string {
    const d = this.toDate(value);
    if (!d) return '-';
    const month = d.toLocaleString('en-US', { month: 'short' });
    return `${month} ${d.getFullYear()}`;
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

  private toDate(value: string | null): Date | null {
    if (!value) return null;
    const d = new Date(value);
    return isNaN(d.getTime()) ? null : d;
  }

  private daysBetween(a: Date, b: Date): number {
    const ms = 24 * 60 * 60 * 1000;
    const a0 = new Date(a.getFullYear(), a.getMonth(), a.getDate()).getTime();
    const b0 = new Date(b.getFullYear(), b.getMonth(), b.getDate()).getTime();
    return Math.round((b0 - a0) / ms);
  }
}