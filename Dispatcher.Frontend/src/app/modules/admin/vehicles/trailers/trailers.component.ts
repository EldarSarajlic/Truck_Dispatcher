import { Component, OnInit } from '@angular/core';

export interface TrailerDto {
  id: number;
  licensePlateNumber: string;
  make: string;
  model: string;
  year: number;
  type: string;
  length: number;
  capacity: number;
  registrationExpiration: string | null;
  insuranceExpiration: string | null;
  vehicleStatusId: number;
  vehicleStatusName: string;
}

export interface VehicleStatusDto {
  id: number;
  statusName: string;
}

@Component({
  selector: 'app-trailers',
  templateUrl: './trailers.component.html',
  standalone: false,
  styleUrls: ['./trailers.component.css'],
})
export class TrailersComponent implements OnInit {
  loading = false;

  // filters
  search = '';
  statusId: number | null = null;

  // dropdown status options (dummy)
  availableStatusOptions: VehicleStatusDto[] = [
    { id: 1, statusName: 'Available' },
    { id: 2, statusName: 'In Transit' },
    { id: 3, statusName: 'Maintenance' },
    { id: 4, statusName: 'Out of Service' },
  ];

  // data
  rows: TrailerDto[] = [];
  filtered: TrailerDto[] = [];

  // modal
  modalOpen = false;
  modalMode: 'create' | 'edit' | 'view' = 'create';
  selectedTrailer: TrailerDto | null = null;

  ngOnInit(): void {
    this.load();
  }

  // ---------------- DATA ----------------

  load(): void {
    this.loading = true;

    // ⏱ fake loading
    setTimeout(() => {
      this.rows = this.getDummyTrailers();
      this.applyFilters();
      this.loading = false;
    }, 400);
  }

  private getDummyTrailers(): TrailerDto[] {
    return [
      {
        id: 1,
        licensePlateNumber: 'TR-101-AA',
        make: 'Schmitz',
        model: 'S.CS',
        year: 2019,
        type: 'Refrigerated',
        length: 13.6,
        capacity: 24,
        registrationExpiration: '2025-06-30',
        insuranceExpiration: '2025-05-15',
        vehicleStatusId: 1,
        vehicleStatusName: 'Available',
      },
      {
        id: 2,
        licensePlateNumber: 'TR-202-BB',
        make: 'Krone',
        model: 'Cool Liner',
        year: 2021,
        type: 'Refrigerated',
        length: 13.6,
        capacity: 25,
        registrationExpiration: '2024-12-01',
        insuranceExpiration: '2024-11-10',
        vehicleStatusId: 2,
        vehicleStatusName: 'In Transit',
      },
      {
        id: 3,
        licensePlateNumber: 'TR-303-CC',
        make: 'Kögel',
        model: 'SN24',
        year: 2017,
        type: 'Flatbed',
        length: 12.0,
        capacity: 22,
        registrationExpiration: '2024-08-20',
        insuranceExpiration: '2024-08-01',
        vehicleStatusId: 3,
        vehicleStatusName: 'Maintenance',
      },
      {
        id: 4,
        licensePlateNumber: 'TR-404-DD',
        make: 'Schwarzmüller',
        model: 'Curtainsider',
        year: 2016,
        type: 'Curtainsider',
        length: 13.6,
        capacity: 23,
        registrationExpiration: null,
        insuranceExpiration: null,
        vehicleStatusId: 4,
        vehicleStatusName: 'Out of Service',
      },
    ];
  }

  // ---------------- FILTERS ----------------

  onSearchChange(value: string): void {
    this.search = value;
    this.applyFilters();
  }

  onStatusIdChange(value: number | null): void {
    this.statusId = value;
    this.applyFilters();
  }

  clearFilters(): void {
    this.search = '';
    this.statusId = null;
    this.applyFilters();
  }

  applyFilters(): void {
    const term = this.search.toLowerCase();

    this.filtered = this.rows.filter((t) => {
      if (term) {
        const text =
          `${t.licensePlateNumber} ${t.make} ${t.model} ${t.type}`.toLowerCase();
        if (!text.includes(term)) return false;
      }

      if (this.statusId !== null && t.vehicleStatusId !== this.statusId) {
        return false;
      }

      return true;
    });
  }

  // ---------------- MODAL ----------------

  openCreateTrailer(): void {
    this.modalMode = 'create';
    this.selectedTrailer = null;
    this.modalOpen = true;
  }

  openEditTrailer(t: TrailerDto): void {
    this.modalMode = 'edit';
    this.selectedTrailer = t;
    this.modalOpen = true;
  }

  openViewTrailer(t: TrailerDto): void {
    this.modalMode = 'view';
    this.selectedTrailer = t;
    this.modalOpen = true;
  }

  closeModal(): void {
    this.modalOpen = false;
  }

  onTrailerFormSubmitted(payload: any): void {
    console.log('Trailer form submitted:', payload);
    this.closeModal();
  }

  // ---------------- ROW ACTIONS ----------------

  view(t: TrailerDto): void {
    this.openViewTrailer(t);
  }

  edit(t: TrailerDto): void {
    this.openEditTrailer(t);
  }

  changeStatus(t: TrailerDto): void {
    console.log('Change status:', t);
  }

  // ---------------- HELPERS ----------------

  formatDate(value: string | null): string {
    if (!value) return '-';
    const d = new Date(value);
    if (isNaN(d.getTime())) return '-';
    return d.toISOString().split('T')[0];
  }

  formatCapacity(value: number): string {
    if (value === null || value === undefined) return '-';
    return `${value.toFixed(1)} tons`;
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
}
