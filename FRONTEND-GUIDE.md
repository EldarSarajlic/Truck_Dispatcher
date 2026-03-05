# Frontend Guide — Truck Dispatcher Management System

> **Scope:** This document is the single source of truth for how Angular components are
> structured, styled, and animated across the entire application. Every new page, regardless
> of role, follows the patterns described here. Read this before building any component.

---

## Table of Contents

1. [Tech Stack Overview](#1-tech-stack-overview)
2. [The Three-Layer Styling System](#2-the-three-layer-styling-system)
3. [Component Anatomy](#3-component-anatomy)
4. [Signals — The Reactivity Model](#4-signals--the-reactivity-model)
5. [Loading States & Skeleton Pattern](#5-loading-states--skeleton-pattern)
6. [Animation System](#6-animation-system)
7. [Page-by-Page Component Guide](#7-page-by-page-component-guide)
8. [Shared Patterns Reference](#8-shared-patterns-reference)
9. [Setup & Configuration](#9-setup--configuration)
10. [Do / Don't Rules](#10-do--dont-rules)

---

## 1. Tech Stack Overview

| Tool | Role | When to use it |
|---|---|---|
| **Angular Signals** | Reactive state | All component state — replaces `BehaviorSubject` for local data |
| **DaisyUI v5** | Component shapes | `card`, `badge`, `btn`, `modal`, `skeleton`, `table`, `alert` |
| **Tailwind v4** | Layout & spacing utilities | `flex`, `grid`, `gap`, `overflow`, quick one-off sizing |
| **Component SCSS** | Visual polish & animations | Keyframes, `nth-child` stagger, `::before`/`::after`, colour variants |
| **`@angular/animations`** | Route & lifecycle transitions | Page enter/leave, list stagger tied to Angular lifecycle |
| **Phosphor Icons** | Iconography | `ph-duotone` class prefix throughout the app |
| **Leaflet.js** | GPS map views | Driver location tracking — mandatory feature |
| **SignalR** | Real-time chat | Dispatcher ↔ Driver messaging — mandatory feature |

**Not using:** GSAP, Framer Motion, Angular Material (being phased out), heavy charting libs.

---

## 2. The Three-Layer Styling System

Every component in this project uses exactly three styling layers working together.
Understanding which layer handles which concern is the most important rule in this guide.

```
┌─────────────────────────────────────────────────────┐
│  Layer 3 — Component SCSS                           │
│  Keyframe animations, ::before/::after, nth-child   │
│  stagger, colour variant classes, hover systems     │
├─────────────────────────────────────────────────────┤
│  Layer 2 — DaisyUI                                  │
│  Component shape: card, badge, btn, skeleton,       │
│  modal, table, alert — the "what is this thing"     │
├─────────────────────────────────────────────────────┤
│  Layer 1 — Tailwind utilities                       │
│  Layout, spacing, overflow — quick one-liners       │
│  that don't need a class name                       │
└─────────────────────────────────────────────────────┘
```

### Decision rule — which layer to use?

Ask yourself: _"Can Tailwind or DaisyUI express this in one or two words?"_

- **Yes** → use Tailwind/DaisyUI. Don't write custom CSS.
- **No** → go to the SCSS file. This includes: `@keyframes`, pseudo-elements,
  `nth-child` selectors, complex gradients, radial glows, hover systems that
  involve multiple child elements changing simultaneously.

### Practical example

```html
<!-- DaisyUI gives the card its shape -->
<!-- Tailwind handles the overflow utility (one word) -->
<!-- SCSS handles the entrance animation, hover lift, ::before gradient bar -->
<div class="card overflow-hidden metric-card metric-card--blue">
```

```scss
// dashboard.component.scss
.metric-card {
  background: #15151f;
  border: 1px solid #252535;
  animation: card-in 480ms cubic-bezier(0.34, 1.56, 0.64, 1) both;

  // Can't do this in Tailwind — involves a pseudo-element that animates on hover
  &::before {
    content: '';
    position: absolute;
    top: 0; left: 0; right: 0;
    height: 3px;
    background: linear-gradient(90deg, #6366f1, #8b5cf6);
    opacity: 0;
    transition: opacity 280ms ease;
  }
  &:hover::before { opacity: 1; }
}
```

---

## 3. Component Anatomy

Every component in this app follows the same three-file structure. No exceptions.

```
feature-name.component.ts      ← Logic, state, signals, methods
feature-name.component.html    ← Template, @if/@for control flow
feature-name.component.scss    ← Visual polish, animations
```

### TypeScript file structure

Follow this order inside every component class:

```typescript
@Component({
  selector: 'app-feature-name',
  standalone: true,
  imports: [CommonModule],           // add RouterModule, pipes etc. as needed
  templateUrl: './feature-name.component.html',
  styleUrl: './feature-name.component.scss',
})
export class FeatureNameComponent implements OnInit, OnDestroy {

  // ── 1. Signals (state) ────────────────────────────────────────────────
  readonly items     = signal<Item[]>([]);
  readonly isLoading = signal(true);
  readonly hasError  = signal(false);

  // ── 2. Computed signals (derived state) ───────────────────────────────
  readonly filteredItems = computed(() =>
    this.items().filter(i => i.isActive)
  );

  // ── 3. Private cleanup handles ────────────────────────────────────────
  private rafIds:    number[] = [];
  private timers:    ReturnType<typeof setTimeout>[] = [];
  private destroyed$ = new Subject<void>();     // for RxJS .takeUntil()

  // ── 4. Lifecycle hooks ────────────────────────────────────────────────
  ngOnInit():    void { this.loadData(); }
  ngOnDestroy(): void {
    this.rafIds.forEach(cancelAnimationFrame);
    this.timers.forEach(clearTimeout);
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  // ── 5. Private data loading ───────────────────────────────────────────
  private loadData(): void {
    // Replace the setTimeout with a real service call:
    // this.itemService.getAll()
    //   .pipe(takeUntil(this.destroyed$))
    //   .subscribe({ next: data => { ... }, error: () => this.hasError.set(true) });
  }

  // ── 6. Public template methods ────────────────────────────────────────
  statusClass(status: string): string { ... }
  trackById(_: number, item: Item): number { return item.id; }
}
```

**Why this order?** It mirrors how you read a component from top to bottom: first you
understand the data it holds, then how that data is derived, then how it cleans up, then
what triggers the data load, and finally what helper methods the template calls.

### HTML template structure

```html
<div class="page-wrapper">

  <!-- Section 1 -->
  <section class="section-name">
    @if (isLoading()) {
      <!-- Skeleton placeholder — exact same DOM structure as real content -->
      <div class="card skeleton-card">
        <div class="skeleton h-6 w-32"></div>
      </div>
    } @else if (hasError()) {
      <!-- Error state -->
      <div class="alert alert-error">Failed to load data.</div>
    } @else {
      <!-- Real content -->
      @for (item of items(); track item.id) {
        <div class="card item-card">...</div>
      }
      @if (items().length === 0) {
        <div class="empty-state">No items found.</div>
      }
    }
  </section>

</div>
```

**Key rules for templates:**
- Always use `@if / @else if / @else` — never `*ngIf`
- Always use `@for` with `track item.id` — never `*ngFor`
- The skeleton DOM structure should mirror the real content structure so there is no
  layout shift when loading completes
- Never put animation classes or inline styles in the template — all animation lives
  in the SCSS file

---

## 4. Signals — The Reactivity Model

Angular Signals replace `BehaviorSubject` for all local component state. Here is the
complete mental model you need.

### `signal()` — a reactive variable

```typescript
readonly count = signal(0);

// Read it (always call it like a function):
console.log(this.count()); // 0

// Update it:
this.count.set(5);
this.count.update(n => n + 1); // use update when new value depends on old value
```

### `computed()` — derived state

```typescript
readonly activeShipments = signal<Shipment[]>([]);
readonly inTransitCount  = computed(() =>
  this.activeShipments().filter(s => s.status === 'In Transit').length
);
```

`computed` automatically re-runs whenever any signal it reads changes. You never call
`.set()` on it — it manages itself. This is the Angular equivalent of a spreadsheet
formula: you define the relationship, the framework keeps it updated.

### When to use RxJS vs Signals

| Situation | Use |
|---|---|
| Local component state (loading, list of items, form values) | `signal()` |
| Data that derives from other data in the same component | `computed()` |
| HTTP service calls | RxJS Observable + `.pipe(takeUntil(this.destroyed$))` |
| Cross-component communication | Service with a `signal()` or `BehaviorSubject` |
| Real-time data (SignalR) | RxJS Subject feeding into a signal via `.subscribe()` |

---

## 5. Loading States & Skeleton Pattern

Every page that loads data from the backend **must** implement three states:
loading, error, and success. Skipping any of these is a bug in the UX.

```typescript
readonly isLoading = signal(true);
readonly hasError  = signal(false);
readonly items     = signal<Item[]>([]);

private loadData(): void {
  this.itemService.getAll()
    .pipe(takeUntil(this.destroyed$))
    .subscribe({
      next:  data  => { this.items.set(data); this.isLoading.set(false); },
      error: ()    => { this.hasError.set(true); this.isLoading.set(false); },
    });
}
```

### Skeleton sizing rule

Skeletons must match the approximate dimensions of the real content they replace.
If a real row is `h-12`, the skeleton row should also be `h-12`. This prevents
the page from visually jumping when data loads.

```html
@if (isLoading()) {
  @for (i of [1,2,3,4]; track i) {
    <tr>
      <td colspan="5" class="px-4 py-3">
        <div class="flex items-center gap-3">
          <div class="skeleton h-8 w-8 rounded-lg"></div>
          <div class="skeleton h-3 flex-1 rounded"></div>
          <div class="skeleton h-5 w-16 rounded-full"></div>
        </div>
      </td>
    </tr>
  }
}
```

---

## 6. Animation System

Animations in this project are split between three mechanisms depending on what you
are animating.

### 6a. CSS `@keyframes` + `nth-child` stagger (preferred for entrance animations)

Use for: page entrance, card grid entrance, table row entrance. Zero JavaScript.
Zero Angular dependencies.

```scss
// Metric card grid — each card enters 80ms after the previous
.metric-card {
  animation: card-in 480ms cubic-bezier(0.34, 1.56, 0.64, 1) both;
}
.metric-card:nth-child(1) { animation-delay:   0ms; }
.metric-card:nth-child(2) { animation-delay:  80ms; }
.metric-card:nth-child(3) { animation-delay: 160ms; }
.metric-card:nth-child(4) { animation-delay: 240ms; }

@keyframes card-in {
  from { opacity: 0; transform: translateY(24px) scale(0.97); }
  to   { opacity: 1; transform: translateY(0)    scale(1);    }
}

// Table rows slide in from the left
.list-row {
  animation: row-in 350ms ease-out both;
}
.list-row:nth-child(1) { animation-delay:   0ms; }
.list-row:nth-child(2) { animation-delay:  55ms; }
.list-row:nth-child(3) { animation-delay: 110ms; }
.list-row:nth-child(4) { animation-delay: 165ms; }
.list-row:nth-child(5) { animation-delay: 220ms; }

@keyframes row-in {
  from { opacity: 0; transform: translateX(-12px); }
  to   { opacity: 1; transform: translateX(0);     }
}
```

### 6b. `requestAnimationFrame` count-up (for dashboard number stats)

Use for: any dashboard metric that should count from 0 to its target value on load.
Gives a polished "live data" feel. Implemented in the TypeScript file with no
external dependencies.

```typescript
private animateCount(
  target:   number,
  key:      keyof DashboardStats,
  duration  = 900,
  delay     = 350,
): void {
  const timer = setTimeout(() => {
    const start = performance.now();
    const tick  = (now: number) => {
      const t     = Math.min((now - start) / duration, 1);
      const eased = 1 - Math.pow(1 - t, 3);            // cubic ease-out
      this.displayStats.update(s => ({ ...s, [key]: Math.round(eased * target) }));
      if (t < 1) this.rafIds.push(requestAnimationFrame(tick));
    };
    this.rafIds.push(requestAnimationFrame(tick));
  }, delay);
  this.timers.push(timer);
}
```

The `delay` (default 350ms) lets card entrance animations finish before the numbers
start counting. This prevents two animations competing for attention at the same time.

Always cancel pending rAF IDs and timers in `ngOnDestroy` — if the user navigates
away mid-animation, the loop must stop.

### 6c. `@angular/animations` (for lifecycle-tied transitions)

Use for: route transitions, elements entering/leaving the DOM based on state changes,
list items that can be individually added or removed at runtime.

```typescript
// src/app/shared/animations/index.ts

import { trigger, transition, style, animate, query, stagger, group } from '@angular/animations';

// Single element fade + slide on DOM enter/leave
export const fadeInOut = trigger('fadeInOut', [
  transition(':enter', [
    style({ opacity: 0, transform: 'translateY(12px)' }),
    animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
  ]),
  transition(':leave', [
    animate('200ms ease-in', style({ opacity: 0, transform: 'translateY(-8px)' })),
  ]),
]);

// Stagger children as they enter a list
export const staggerList = trigger('staggerList', [
  transition('* => *', [
    query(':enter', [
      style({ opacity: 0, transform: 'translateY(16px)' }),
      stagger('60ms', [
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ]),
    ], { optional: true }),
  ]),
]);

// Slide panel in from right (detail drawers, side panels)
export const slideInRight = trigger('slideInRight', [
  transition(':enter', [
    style({ transform: 'translateX(100%)', opacity: 0 }),
    animate('350ms cubic-bezier(0.25, 0.46, 0.45, 0.94)',
      style({ transform: 'translateX(0)', opacity: 1 })),
  ]),
  transition(':leave', [
    animate('250ms ease-in',
      style({ transform: 'translateX(100%)', opacity: 0 })),
  ]),
]);

// Status badge pop on value change
export const statusChange = trigger('statusChange', [
  transition('* => *', [
    style({ transform: 'scale(0.9)', opacity: 0.5 }),
    animate('200ms ease-out', style({ transform: 'scale(1)', opacity: 1 })),
  ]),
]);

// Page route transition
export const routeAnimation = trigger('routeAnimation', [
  transition('* <=> *', [
    query(':enter, :leave', [
      style({ position: 'absolute', width: '100%' }),
    ], { optional: true }),
    group([
      query(':leave', [
        animate('200ms ease-in', style({ opacity: 0, transform: 'translateY(-8px)' })),
      ], { optional: true }),
      query(':enter', [
        style({ opacity: 0, transform: 'translateY(12px)' }),
        animate('300ms 100ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
      ], { optional: true }),
    ]),
  ]),
]);
```

### Animation decision table

| What you're animating | Use |
|---|---|
| Page entering the viewport | CSS `@keyframes` on `.page-wrapper` |
| Card grid appearing | CSS `@keyframes` + `nth-child` stagger |
| Table rows appearing | CSS `@keyframes` + `nth-child` stagger |
| Dashboard number counting up | `requestAnimationFrame` in TypeScript |
| Status badge colour change | `@angular/animations` `statusChange` |
| Element entering/leaving DOM at runtime | `@angular/animations` `fadeInOut` |
| Detail panel sliding in from side | `@angular/animations` `slideInRight` |
| Route/page transition | `@angular/animations` `routeAnimation` |
| Hover lift on a card | SCSS `transition: transform` + `&:hover` |
| Button hover/active state | Tailwind `transition-*` utilities |
| Loading spinner | Tailwind `animate-spin` |
| Loading skeleton shimmer | DaisyUI `.skeleton` class |

### Pulsing status dot pattern

Status badges in this app use a `::before` pseudo-element as a small coloured dot
that pulses with a `box-shadow` ring animation. This makes live statuses (In Transit,
Available) feel active without being distracting.

```scss
.status-badge::before {
  content:       '';
  width:         5px; height: 5px;
  border-radius: 50%;
  background:    currentColor;
}

.badge--available::before { animation: ring-green 2.2s ease-out infinite; }
.badge--transit::before   { animation: ring-blue  2.2s ease-out infinite; }
// Maintenance has no pulse — the vehicle isn't active

@keyframes ring-green {
  0%   { box-shadow: 0 0 0 0px rgba( 52,211,153,0.65); }
  70%  { box-shadow: 0 0 0 5px rgba( 52,211,153,0);    }
  100% { box-shadow: 0 0 0 0px rgba( 52,211,153,0);    }
}
```

### Reduced motion — accessibility requirement

Every component SCSS file must include this block at the bottom. This respects users
who have enabled "reduce motion" in their OS settings (common for people with
vestibular disorders or epilepsy).

```scss
@media (prefers-reduced-motion: reduce) {
  .page-wrapper,
  .metric-card,
  .list-row,
  .status-badge::before {
    animation:  none !important;
    transition: none !important;
  }
}
```

---

## 7. Page-by-Page Component Guide

Each section below describes the components for one role/module, what state they
manage, which patterns to apply, and any role-specific notes.

---

### Admin Module

The admin sees everything. Admin pages are data-heavy and management-focused.
The priority is clarity, scannable tables, and reliable CRUD operations.

#### Admin Dashboard (`/admin/dashboard`)

**Component:** `DashboardComponent`

State signals:
```typescript
readonly vehicles    = signal<Vehicle[]>([]);
readonly isLoading   = signal(true);
readonly displayStats = signal<DashboardStats>({ ... });
readonly metricCards  = computed(() => [...]);   // derived from displayStats
```

Animation checklist:
- ✅ Page entrance: CSS `@keyframes page-in` on `.page-wrapper`
- ✅ Metric card stagger: `nth-child` delays on `.metric-card`
- ✅ Count-up: `requestAnimationFrame` on each stat after card entrance finishes
- ✅ Table row stagger: `nth-child` delays on `.fleet-row`
- ✅ Pulsing status dots: `ring-green` / `ring-blue` `@keyframes` on `::before`
- ✅ Hover lift: SCSS `transition` + `&:hover { transform: translateY(-4px) }`
- ✅ Hover accent bar: `::before` pseudo-element revealed at `opacity: 1` on hover

Colour variants for metric cards: `amber`, `green`, `blue`, `violet`. Applied as
`metric-card--{variant}` and `icon--{variant}` in the template from the `metricCards`
computed signal.

---

#### Admin Users (`/admin/users`)

**Component:** `AdminUsersComponent`

Page type: **table + CRUD**. The standard layout for all list pages.

State signals:
```typescript
readonly users     = signal<User[]>([]);
readonly isLoading = signal(true);
readonly hasError  = signal(false);
readonly search    = signal('');
readonly filteredUsers = computed(() =>
  this.users().filter(u =>
    u.fullName.toLowerCase().includes(this.search().toLowerCase())
  )
);
```

Animation checklist:
- ✅ Page entrance on `.page-wrapper`
- ✅ Table row stagger (`nth-child`, 5 rows)
- ✅ Status badge pulse for active/inactive users
- ✅ Skeleton rows while loading (match table row height)
- ✅ Fade-in on search results change (`@angular/animations` `fadeInOut` optional)

CRUD modals: Use DaisyUI `<dialog class="modal">` for create/edit forms. Confirm
deletes with a DaisyUI `alert` or an `alert-dialog` from Spartan UI.

---

#### Admin Vehicles (`/admin/vehicles`)

**Component:** `AdminVehiclesComponent` (parent with tabs: Trucks, Trailers)

Page type: **tabbed table**. Each tab is a child route.

Notes:
- Tab navigation via `routerLinkActive` — already set up in the routing module
- Trucks and Trailers are separate child components, each following the standard
  table pattern
- Vehicle status badges use the three-variant pattern: `Available`, `In Transit`,
  `Maintenance` — same classes as the dashboard fleet table

---

#### Admin Map (`/admin/map`)

**Component:** `MapComponent`

Page type: **full-screen map panel**. This is the GPS tracking view (mandatory feature).

Notes:
- Leaflet.js is loaded here. Do not import Leaflet globally — lazy load it
- The map `<div>` must have an explicit pixel height (e.g., `height: calc(100vh - 120px)`)
  or Leaflet will render at 0px
- Vehicle markers should use a custom icon matching the app's colour palette
- Real-time position updates come via SignalR — update marker position without
  re-rendering the whole map

```typescript
private map!: L.Map;

ngAfterViewInit(): void {
  // Leaflet must initialise after the DOM exists — use AfterViewInit, not OnInit
  this.map = L.map('map-container').setView([43.84, 18.35], 8);
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(this.map);
}
```

Animation checklist:
- ✅ Page entrance on the wrapper
- ✅ Marker pop animation (Leaflet CSS class `leaflet-zoom-animated` handles this)
- ✅ No table stagger needed here

---

### Dispatcher Module

The dispatcher manages the core workflow: creating shipments, assigning drivers,
tracking deliveries. Pages are action-focused with frequent status changes.

#### Dispatcher Dashboard (`/dispatcher/dashboard`)

Same structure as the Admin Dashboard but with dispatcher-specific stats:
active dispatches, pending assignments, drivers available, deliveries today.

Apply the same count-up + stagger pattern.

---

#### Dispatcher Shipments (`/dispatcher/shipments`)

**Component:** `DispatcherShipmentsComponent`

Page type: **filterable table with status pipeline**.

State signals:
```typescript
readonly shipments      = signal<Shipment[]>([]);
readonly isLoading      = signal(true);
readonly activeFilter   = signal<ShipmentStatus | 'All'>('All');
readonly filteredShipments = computed(() =>
  this.activeFilter() === 'All'
    ? this.shipments()
    : this.shipments().filter(s => s.status === this.activeFilter())
);
```

Filter tabs are rendered from a `computed` signal, not hardcoded in the template:
```typescript
readonly filterTabs = computed(() => [
  { label: 'All',         value: 'All',         count: this.shipments().length },
  { label: 'Pending',     value: 'Pending',      count: this.shipments().filter(...).length },
  { label: 'In Transit',  value: 'In Transit',   count: ... },
  { label: 'Delivered',   value: 'Delivered',    count: ... },
]);
```

Animation checklist:
- ✅ Page entrance
- ✅ Table row stagger
- ✅ Status badge pulse (`In Transit` status pulses)
- ✅ `@angular/animations` `staggerList` when filter changes (list re-renders)
- ✅ `statusChange` animation when a shipment's status updates via SignalR

---

#### Dispatcher Chat (`/dispatcher/chat`)

**Component:** `DispatcherChatComponent` (mandatory SignalR feature)

Page type: **real-time messaging panel**.

Notes:
- SignalR hub connection is initialised in `ngOnInit` and stopped in `ngOnDestroy`
- Messages are stored in a signal and appended (never re-fetched on each message)
- Auto-scroll to bottom when new messages arrive using `ViewChild` on the message container

```typescript
readonly messages = signal<ChatMessage[]>([]);
private hubConnection!: signalR.HubConnection;

ngOnInit(): void {
  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/hubs/chat')
    .build();

  this.hubConnection.on('ReceiveMessage', (msg: ChatMessage) => {
    this.messages.update(msgs => [...msgs, msg]);
    this.scrollToBottom();
  });

  this.hubConnection.start();
}

ngOnDestroy(): void {
  this.hubConnection.stop();
}
```

Animation checklist:
- ✅ Page entrance
- ✅ Individual new message: CSS `@keyframes msg-in` (slide up from bottom, 200ms)
- ✅ No stagger on historical messages — they appear instantly on load

---

### Driver Module

Drivers see only their own assigned shipments and communicate with the dispatcher.
Pages are simple, high-contrast, and mobile-friendly (drivers use phones).

#### Driver Dashboard (`/driver/dashboard`)

Keep it minimal: one active shipment card, current status, a map snippet, and the
chat button. No tables. No complex filtering.

State signals:
```typescript
readonly activeShipment = signal<Shipment | null>(null);
readonly isLoading      = signal(true);
```

Animation checklist:
- ✅ Page entrance
- ✅ Single card entrance (no stagger needed — only one card)
- ✅ Status badge pulse

---

#### Driver Shipment Detail (`/driver/shipment/:id`)

Page type: **detail view with map**. Shows full shipment info + Leaflet map of the route.

Notes:
- Use `ActivatedRoute` to read the `:id` param and load the shipment
- Leaflet map shows origin → destination route polyline
- The driver can update their status (e.g. "Picked Up", "Delivered") from this page

---

### Client Module

Clients place orders and track their deliveries. The experience should feel clean
and self-service, closer to a customer portal than an admin tool.

#### Client Dashboard (`/client/dashboard`)

State signals:
```typescript
readonly orders       = signal<Order[]>([]);
readonly activeOrders = computed(() => this.orders().filter(o => o.status !== 'Delivered'));
readonly isLoading    = signal(true);
```

Animation checklist:
- ✅ Page entrance
- ✅ Order card stagger (cards, not table rows)
- ✅ Status badge pulse for orders in transit

---

#### Client New Order (`/client/orders/new`)

Page type: **multi-step form**.

Use a `signal` for the current step:
```typescript
readonly currentStep = signal<1 | 2 | 3>(1);
```

Each step is a separate section in the template controlled by `@if (currentStep() === n)`.
The `@angular/animations` `fadeInOut` trigger handles the transition between steps.

Validation: Inline validation on blur. Show error messages as `<span class="text-error text-xs">`
directly below each input. Never use `alert()` or `console.error()` for validation feedback.

---

## 8. Shared Patterns Reference

### Colour variant system

Whenever a component has multiple colour states (status badges, metric cards, icon
backgrounds), define the variants as SCSS classes and apply them dynamically from the
TypeScript, not with inline styles.

```typescript
// In the component
statusClass(status: string): string {
  return ({
    'Available':   'badge--available',
    'In Transit':  'badge--transit',
    'Maintenance': 'badge--maintenance',
    'Pending':     'badge--pending',
  } as Record<string, string>)[status] ?? '';
}
```

```html
<span class="badge status-badge {{ statusClass(vehicle.status) }}">
  {{ vehicle.status }}
</span>
```

```scss
// All variants in one place in the SCSS file
.badge--available   { background: rgba( 52,211,153,0.12); color: #34d399; border: 1px solid rgba(52,211,153,0.25); }
.badge--transit     { background: rgba( 56,189,248,0.12); color: #38bdf8; border: 1px solid rgba(56,189,248,0.25); }
.badge--maintenance { background: rgba(251,191, 36,0.12); color: #fbbf24; border: 1px solid rgba(251,191,36,0.25); }
.badge--pending     { background: rgba(192,132,252,0.12); color: #c084fc; border: 1px solid rgba(192,132,252,0.25); }
```

### Row number formatting

Table row indices use `padStart` to render as `01`, `02`, `03`:

```html
<td class="col-idx">{{ (i + 1).toString().padStart(2, '0') }}</td>
```

### Empty state

Every table and list must handle the empty case:

```html
@if (items().length === 0) {
  <tr>
    <td colspan="5" class="empty-row">
      <i class="ph-duotone ph-package"></i>
      No items found.
    </td>
  </tr>
}
```

```scss
.empty-row {
  text-align: center;
  padding: 60px 20px !important;
  color: #44445a;
  font-size: 0.85rem;

  i { font-size: 40px; display: block; margin-bottom: 10px; opacity: 0.25; }
}
```

### Lookup table pattern for class mapping

Prefer an object lookup over `if/else` chains for mapping values to CSS classes,
route paths, or display strings:

```typescript
// Good — easy to extend, no side effects, single expression
readonly variantMap: Record<string, string> = {
  'Pending':    'amber',
  'Approved':   'green',
  'InTransit':  'blue',
  'Delivered':  'violet',
};

variantFor(status: string): string {
  return this.variantMap[status] ?? 'neutral';
}
```

---

## 9. Setup & Configuration

### Tailwind v4 + DaisyUI v5 import syntax

```css
/* styles.css — NOT tailwind.config.js plugin syntax */
@import "tailwindcss";
@import "daisyui";
```

> ⚠️ Tailwind v4 does NOT use `tailwind.config.js` by default. If you add custom
> animations via config, you must explicitly opt in to config-file mode.

### Angular animations provider

```typescript
// app.config.ts
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
  ],
};
```

### JetBrains Mono font (used for numeric values and licence plates)

```css
/* styles.css */
@import url('https://fonts.googleapis.com/css2?family=JetBrains+Mono:wght@400;500;600;700&display=swap');
```

Apply with: `font-family: 'JetBrains Mono', monospace;` in SCSS, never as a Tailwind class.

### Phosphor Icons

Loaded as a CDN stylesheet or npm package. Classes follow the pattern:
`ph-duotone ph-{icon-name}` for duotone icons, `ph ph-{icon-name}` for outline.

---

## 10. Do / Don't Rules

### Do

- Use `signal()` for all local component state
- Use `computed()` to derive values — never duplicate state
- Implement loading, error, and empty states on every data-loading page
- Match skeleton dimensions to real content dimensions
- Put all `@keyframes` and pseudo-element styles in the SCSS file, not the template
- Use `nth-child` for animation stagger — it requires zero JavaScript
- Cancel all `requestAnimationFrame` IDs and `setTimeout` handles in `ngOnDestroy`
- Use the lookup table pattern for class mapping
- Add `@media (prefers-reduced-motion: reduce)` to every SCSS file that defines animations
- Use `@if` / `@for` (Angular 17+ control flow), not `*ngIf` / `*ngFor`
- Always `track item.id` in `@for` loops

### Don't

- Don't put animation-related classes or inline styles in HTML templates
- Don't write custom CSS for things Tailwind can express in one word
- Don't use `BehaviorSubject` for local component state — use `signal()`
- Don't skip `ngOnDestroy` cleanup — memory leaks cause bugs when navigating between pages
- Don't use `alert()`, `confirm()`, or `console.error()` for user-facing feedback
- Don't animate data table cells or form inputs — keep them instant
- Don't use animation durations over 400ms — this is a business tool, not a portfolio
- Don't use GSAP, Framer Motion, or other heavy animation libraries
- Don't initialise a Leaflet map in `ngOnInit` — it must be in `ngAfterViewInit`
- Don't import Leaflet globally — load it only in the components that need it
