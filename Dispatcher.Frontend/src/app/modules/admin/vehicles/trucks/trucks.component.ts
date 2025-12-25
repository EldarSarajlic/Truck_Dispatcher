import { Component, OnInit } from '@angular/core';
import { TruckService } from '../../../../api-services/trucks/truck.service';
import { TruckDto } from '../../../../core/models/truck.model';

type MaintenanceFilter = 'all' | 'dueSoon' | 'overdue';

@Component({
  selector: 'app-trucks',
  templateUrl: './trucks.component.html',
  standalone: false,
  styleUrls: ['./trucks.component.css'],
})
export class TrucksComponent implements OnInit {
  loading = false;

  // filters
  search = '';
  statusFilter: string = 'all';
  maintenanceFilter: MaintenanceFilter = 'all';

  // data
  rows: TruckDto[] = [];
  filtered: TruckDto[] = [];

  availableStatuses: string[] = [];

  constructor(private truckService: TruckService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
  this.loading = true;

  this.truckService.getAll().subscribe({
    next: (data: TruckDto[]) => {
      this.rows = data ?? [];

      const set = new Set<string>();
      for (const t of this.rows) {
        if (t.vehicleStatusName) set.add(t.vehicleStatusName);
      }
      this.availableStatuses = Array.from(set).sort((a, b) => a.localeCompare(b));

      this.applyFilters();
      this.loading = false;
    },
    error: () => {
      this.rows = [];
      this.filtered = [];
      this.availableStatuses = [];
      this.loading = false;
    },
  });
}

  clearFilters(): void {
    this.search = '';
    this.statusFilter = 'all';
    this.maintenanceFilter = 'all';
    this.applyFilters();
  }

  applyFilters(): void {
    const q = this.search.trim().toLowerCase();

    this.filtered = this.rows.filter((t) => {
      if (this.statusFilter !== 'all' && (t.vehicleStatusName ?? '') !== this.statusFilter) {
        return false;
      }

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

      if (!q) return true;

      const hay = [
        t.licensePlateNumber ?? '',
        t.vinNumber ?? '',
        t.make ?? '',
        t.model ?? '',
        String(t.year ?? ''),
        t.vehicleStatusName ?? '',
        t.gpsDeviceId ?? '',
      ]
        .join(' ')
        .toLowerCase();

      return hay.includes(q);
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