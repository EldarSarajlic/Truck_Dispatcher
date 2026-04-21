import { Component, OnInit, signal, inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Location, DOCUMENT } from '@angular/common';

type Section = 'language' | 'appearance';
type Theme   = 'dark' | 'light';

@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss',
})
export class SettingsComponent implements OnInit {
  private translate = inject(TranslateService);
  private location  = inject(Location);
  private document  = inject(DOCUMENT);

  readonly activeSection = signal<Section>('language');
  readonly currentLang   = signal<string>('bs');
  readonly currentTheme  = signal<Theme>('dark');

  readonly languages = [
    { code: 'bs', name: 'Bosanski', flag: '🇧🇦' },
    { code: 'en', name: 'English',  flag: '🇬🇧' },
  ];

  ngOnInit(): void {
    const savedLang  = localStorage.getItem('language') || 'bs';
    const savedTheme = (localStorage.getItem('theme') || 'dark') as Theme;
    this.currentLang.set(savedLang);
    this.currentTheme.set(savedTheme);
    this.document.documentElement.setAttribute('data-theme', savedTheme);
  }

  switchLanguage(code: string): void {
    this.currentLang.set(code);
    this.translate.use(code).subscribe();
    localStorage.setItem('language', code);
  }

  switchTheme(theme: Theme): void {
    this.currentTheme.set(theme);
    localStorage.setItem('theme', theme);
    this.document.documentElement.setAttribute('data-theme', theme);
  }

  goBack(): void {
    this.location.back();
  }
}
