import { Component, HostBinding, Input } from '@angular/core';

export interface MetricCard {
  label:    string;
  value:    number | string;   // raw number or pre-formatted string (e.g. '1.234.567')
  variant:  'amber' | 'green' | 'blue' | 'violet';
  icon:     string;   // Phosphor icon class
  pill:     string;   // small sub-label i18n key
  prefix?:  string;   // e.g. '€' for revenue
}

@Component({
  selector:    'app-metric-card',
  standalone:  false,
  templateUrl: './metric-card.component.html',
  styleUrl:    './metric-card.component.scss',
})
export class MetricCardComponent {
  @Input() label!:   string;
  @Input() value!:   number | string;
  @Input() variant!: 'amber' | 'green' | 'blue' | 'violet';
  @Input() icon!:    string;
  @Input() pill!:    string;
  @Input() prefix?: string;
  @Input() index:   number = 0;
  @Input() loading: boolean = false;

  // Replaces the nth-child stagger in SCSS — each card enters 60ms after the previous.
  @HostBinding('style.animation-delay')
  get animationDelay(): string {
    return (40 + this.index * 60) + 'ms';
  }
}
