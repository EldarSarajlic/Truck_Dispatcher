import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-paginator-bar',
  standalone: false,
  templateUrl: './paginator-bar.component.html',
  styleUrl: './paginator-bar.component.scss',
})
export class PaginatorBarComponent {
  @Input({ required: true }) currentPage!: number;
  @Input({ required: true }) totalPages!: number;
  @Input({ required: true }) totalItems!: number;
  @Input() pageSize = 10;
  @Output() pageChange = new EventEmitter<number>();

  getPageNumbers(): (number | null)[] {
    const total   = this.totalPages || 1;
    const current = this.currentPage || 1;
    if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1);
    if (current <= 4)         return [1, 2, 3, 4, 5, null, total];
    if (current >= total - 3) return [1, null, total - 4, total - 3, total - 2, total - 1, total];
    return [1, null, current - 1, current, current + 1, null, total];
  }

  getShowingStart(): number {
    if (this.totalItems === 0) return 0;
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  getShowingEnd(): number {
    return Math.min(this.currentPage * this.pageSize, this.totalItems);
  }
}
