import {
  Component, ElementRef, EventEmitter,
  HostListener, Input, OnChanges, Output, SimpleChanges,
} from '@angular/core';

export type DpViewMode = 'day' | 'month' | 'year';

interface CalendarDay {
  date:           Date;
  isCurrentMonth: boolean;
  isToday:        boolean;
  isSelected:     boolean;
}

@Component({
  selector:    'app-date-picker',
  standalone:  false,
  templateUrl: './date-picker.component.html',
  styleUrl:    './date-picker.component.scss',
})
export class DatePickerComponent implements OnChanges {

  @Input() value       = '';
  @Input() hasError    = false;
  @Input() placeholder = 'Select date...';

  @Output() valueChange = new EventEmitter<string>();

  open:     boolean    = false;
  viewMode: DpViewMode = 'day';
  viewDate  = new Date();
  days:     CalendarDay[] = [];

  yearRangeStart = 0;

  readonly weekdays = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
  readonly monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December',
  ];
  readonly monthShort = [
    'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec',
  ];

  constructor(private el: ElementRef) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['value'] && this.value) {
      const parsed = new Date(this.value + 'T00:00:00');
      if (!isNaN(parsed.getTime())) {
        this.viewDate = new Date(parsed.getFullYear(), parsed.getMonth(), 1);
      }
    }
    this.buildCalendar();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(e: MouseEvent): void {
    if (!this.el.nativeElement.contains(e.target)) {
      this.open = false;
    }
  }

  toggle(e: Event): void {
    e.stopPropagation();
    this.open = !this.open;
    if (this.open) {
      this.viewMode = 'day';
      this.buildCalendar();
    }
  }

  // ── View mode switching ───────────────────────────────────────────────────
  openMonthView(): void { this.viewMode = 'month'; }

  openYearView(): void {
    this.yearRangeStart = this.viewDate.getFullYear() - 4;
    this.viewMode = 'year';
  }

  selectMonth(index: number): void {
    this.viewDate = new Date(this.viewDate.getFullYear(), index, 1);
    this.viewMode = 'day';
    this.buildCalendar();
  }

  selectYear(year: number): void {
    this.viewDate = new Date(year, this.viewDate.getMonth(), 1);
    this.viewMode = 'month';
  }

  prevYearRange(): void { this.yearRangeStart -= 12; }
  nextYearRange(): void { this.yearRangeStart += 12; }

  // ── Day navigation ────────────────────────────────────────────────────────
  prevMonth(): void {
    this.viewDate = new Date(this.viewDate.getFullYear(), this.viewDate.getMonth() - 1, 1);
    this.buildCalendar();
  }

  nextMonth(): void {
    this.viewDate = new Date(this.viewDate.getFullYear(), this.viewDate.getMonth() + 1, 1);
    this.buildCalendar();
  }

  selectDay(day: CalendarDay, e: Event): void {
    e.stopPropagation();
    const d   = day.date;
    const iso = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
    this.valueChange.emit(iso);
    this.open = false;
  }

  // ── Template helpers ──────────────────────────────────────────────────────
  get displayValue(): string {
    if (!this.value) return '';
    const d = new Date(this.value + 'T00:00:00');
    if (isNaN(d.getTime())) return '';
    return d.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
  }

  get monthLabel(): string  { return this.monthNames[this.viewDate.getMonth()]; }
  get yearLabel():  string  { return String(this.viewDate.getFullYear()); }

  get yearRange(): number[] {
    return Array.from({ length: 12 }, (_, i) => this.yearRangeStart + i);
  }

  get yearRangeLabel(): string {
    return `${this.yearRangeStart} – ${this.yearRangeStart + 11}`;
  }

  isSelectedMonth(index: number): boolean {
    if (!this.value) return false;
    const d = new Date(this.value + 'T00:00:00');
    return d.getFullYear() === this.viewDate.getFullYear() && d.getMonth() === index;
  }

  isSelectedYear(year: number): boolean {
    if (!this.value) return false;
    return new Date(this.value + 'T00:00:00').getFullYear() === year;
  }

  isCurrentYear(year: number):  boolean { return new Date().getFullYear() === year; }

  isCurrentMonth(index: number): boolean {
    const t = new Date();
    return t.getFullYear() === this.viewDate.getFullYear() && t.getMonth() === index;
  }

  // ── Calendar builder ──────────────────────────────────────────────────────
  private buildCalendar(): void {
    const year     = this.viewDate.getFullYear();
    const month    = this.viewDate.getMonth();
    const today    = new Date();
    const selected = this.value ? new Date(this.value + 'T00:00:00') : null;

    const firstDay    = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const daysInPrev  = new Date(year, month, 0).getDate();

    this.days = [];

    for (let i = firstDay - 1; i >= 0; i--)
      this.days.push(this.makeDay(new Date(year, month - 1, daysInPrev - i), false, today, selected));

    for (let d = 1; d <= daysInMonth; d++)
      this.days.push(this.makeDay(new Date(year, month, d), true, today, selected));

    const remaining = 42 - this.days.length;
    for (let d = 1; d <= remaining; d++)
      this.days.push(this.makeDay(new Date(year, month + 1, d), false, today, selected));
  }

  private makeDay(date: Date, isCurrentMonth: boolean, today: Date, selected: Date | null): CalendarDay {
    return {
      date,
      isCurrentMonth,
      isToday:    this.sameDay(date, today),
      isSelected: !!selected && this.sameDay(date, selected),
    };
  }

  private sameDay(a: Date, b: Date): boolean {
    return a.getFullYear() === b.getFullYear()
        && a.getMonth()    === b.getMonth()
        && a.getDate()     === b.getDate();
  }
}
