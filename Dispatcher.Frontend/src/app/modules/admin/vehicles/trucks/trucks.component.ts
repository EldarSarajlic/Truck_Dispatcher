import { Component, OnDestroy, OnInit } from '@angular/core';
import { TruckService } from '../../../../api-services/trucks/truck.service';
import { TruckDto } from '../../../../core/models/truck.model';
import { Subject, of } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, startWith, switchMap, takeUntil, tap } from 'rxjs/operators';

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
  statusFilter: string = 'all';
  maintenanceFilter: MaintenanceFilter = 'all';

  // data
  rows: TruckDto[] = [];
  filtered: TruckDto[] = [];

  availableStatuses: string[] = [];

  private readonly destroy$ = new Subject<void>();
  private readonly search$ = new Subject<string>();

  constructor(private truckService: TruckService) {}

  ngOnInit(): void {
    // Debounced server-side search
    this.search$
      .pipe(
        startWith(''),
        debounceTime(300),
        distinctUntilChanged(),
        tap(() => (this.loading = true)),
        switchMap((term) =>
          this.truckService.getAll({ search: term?.trim() || undefined }).pipe(catchError(() => of([] as TruckDto[])))
        ),
        takeUntil(this.destroy$)
      )
      .subscribe((data: TruckDto[]) => {
        this.rows = data ?? [];

        // status dropdown iz podataka
        const set = new Set<string>();
        for (const t of this.rows) {
          if (t.vehicleStatusName) set.add(t.vehicleStatusName);
        }
        this.availableStatuses = Array.from(set).sort((a, b) => a.localeCompare(b));

        // maintenance/status(name) filter ostaje lokalno
        this.applyFilters();
        this.loading = false;
      });

    // initial load
    this.refresh();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Called from HTML on typing (ngModelChange)
  onSearchChange(value: string): void {
    this.search = value;
    this.search$.next(value);
  }

  // Manual refresh (button)
  refresh(): void {
    this.search$.next(this.search);
  }

  // Keep old name so you don't have to change all buttons
  load(): void {
    this.refresh();
  }

  clearFilters(): void {
    this.search = '';
    this.statusFilter = 'all';
    this.maintenanceFilter = 'all';

    // reset server-side search
    this.refresh();

    // reset local filters
    this.applyFilters();
  }

  applyFilters(): void {
    const q = this.search.trim().toLowerCase();

    this.filtered = this.rows.filter((t) => {
      // local status filter by name (until T-2.3.5)
      if (this.statusFilter !== 'all' && (t.vehicleStatusName ?? '') !== this.statusFilter) {
        return false;
      }

      // maintenance filter (local)
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

      // NOTE: server-side search already filtered rows,
      // but we keep this local search too so UI behaves the same.
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