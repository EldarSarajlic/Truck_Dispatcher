import { Component, OnInit, OnDestroy, HostListener, computed, signal, inject } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { ToasterService }      from '../../../core/services/toaster.service';
import { MetricCard }          from '../../shared/components/metric-card/metric-card.component';
import { InventoryApiService } from '../../../api-services/inventory/inventory-api.service';
import { InventoryItemDto, ListInventoryRequest } from '../../../api-services/inventory/inventory-api.model';

export type CartItem = { item: InventoryItemDto; quantity: number };

@Component({
  selector: 'app-admin-inventory',
  standalone: false,
  templateUrl: './admin-inventory.component.html',
  styleUrl: './admin-inventory.component.scss',
})
export class AdminInventoryComponent implements OnInit, OnDestroy {

  // ── 1. Signals ────────────────────────────────────────────────────────────
  readonly items        = signal<InventoryItemDto[]>([]);
  readonly isLoading    = signal(true);
  readonly hasError     = signal(false);
  readonly totalItems   = signal(0);
  readonly cartItems    = signal<CartItem[]>([]);
  readonly cartOpen     = signal(false);
  readonly selectedItem = signal<InventoryItemDto | null>(null);

  readonly addItemModalOpen = signal(false);
  readonly detailsModalOpen = signal(false);

  // ── 2. Computed ───────────────────────────────────────────────────────────
  readonly filteredItems = computed(() => this.items());

  readonly cartCount = computed(() =>
    this.cartItems().reduce((s, c) => s + c.quantity, 0)
  );

  readonly cartTotal = computed(() =>
    this.cartItems().reduce((s, c) => s + c.item.unitPrice * c.quantity, 0)
  );

  readonly metricCards = computed<MetricCard[]>(() => {
    const all = this.items();
    return [
      { label: 'Total Items',  value: this.totalItems(),                       variant: 'blue',   icon: 'ph-duotone ph-package',     pill: 'In catalog'  },
      { label: 'Active',       value: all.filter(i => i.isActive).length,      variant: 'green',  icon: 'ph-duotone ph-check-circle', pill: 'Available'   },
      { label: 'Inactive',     value: all.filter(i => !i.isActive).length,     variant: 'amber',  icon: 'ph-duotone ph-warning',      pill: 'Unavailable' },
      { label: 'Categories',   value: new Set(all.map(i => i.category)).size,  variant: 'violet', icon: 'ph-duotone ph-tag',          pill: 'Distinct'    },
    ];
  });

  // ── 3. Form state ─────────────────────────────────────────────────────────
  addForm: { sku: string; name: string; description: string; category: string; unitOfMeasure: string; unitPrice: number | null } = {
    sku: '', name: '', description: '', category: '', unitOfMeasure: 'pcs', unitPrice: null,
  };
  addFormErrors: Record<string, string> = {};

  // ── 4. Constants ──────────────────────────────────────────────────────────
  readonly categories   = ['Electronics', 'Automotive', 'Furniture', 'Food', 'Other'];
  readonly skeletonRows = [1, 2, 3, 4, 5];

  // ── 5. Search / filter ────────────────────────────────────────────────────
  searchTerm = '';
  readonly selectedCategory = signal('');
  categoryDropdownOpen = false;

  // ── 6. Private ────────────────────────────────────────────────────────────
  private readonly destroyed$    = new Subject<void>();
  private readonly searchSubject = new Subject<string>();
  private readonly toast         = inject(ToasterService);
  private readonly api           = inject(InventoryApiService);

  constructor() {
    this.searchSubject.pipe(
      debounceTime(700),
      distinctUntilChanged(),
      takeUntil(this.destroyed$),
    ).subscribe(term => this.loadItems(term));
  }

  // ── 7. Lifecycle ──────────────────────────────────────────────────────────
  ngOnInit():    void { this.loadItems(); }
  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }

  // ── 8. Data loading ───────────────────────────────────────────────────────
  loadItems(search = this.searchTerm, category = this.selectedCategory()): void {
    this.isLoading.set(true);
    this.hasError.set(false);

    const req      = new ListInventoryRequest();
    req.paging     = { page: 1, pageSize: 100 };
    req.search     = search   || null;
    req.category   = category || null;

    this.api.getItems(req)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: res => {
          this.items.set(res.items as InventoryItemDto[]);
          this.totalItems.set(res.totalItems);
          this.isLoading.set(false);
        },
        error: () => {
          this.hasError.set(true);
          this.isLoading.set(false);
        },
      });
  }

  reload(): void { this.loadItems(); }

  // ── 9. Search & category ──────────────────────────────────────────────────
  onSearchInput(): void { this.searchSubject.next(this.searchTerm); }
  onSearch(): void      { this.loadItems(this.searchTerm); }

  @HostListener('document:click')
  closeDropdowns(): void { this.categoryDropdownOpen = false; }

  toggleCategoryDropdown(e: Event): void {
    e.stopPropagation();
    this.categoryDropdownOpen = !this.categoryDropdownOpen;
  }

  selectCategory(cat: string, e: Event): void {
    e.stopPropagation();
    this.selectedCategory.set(cat);
    this.categoryDropdownOpen = false;
    this.loadItems(this.searchTerm, cat);
  }

  clearCategory(e: Event): void {
    e.stopPropagation();
    this.selectedCategory.set('');
    this.categoryDropdownOpen = false;
    this.loadItems(this.searchTerm, '');
  }

  // ── 10. Class helpers ─────────────────────────────────────────────────────
  categoryClass(category: string): string {
    return ({
      'Electronics': 'cat--electronics',
      'Automotive':  'cat--automotive',
      'Furniture':   'cat--furniture',
      'Food':        'cat--food',
    } as Record<string, string>)[category] ?? 'cat--other';
  }

  // ── 11. Cart ──────────────────────────────────────────────────────────────
  toggleCart(): void { this.cartOpen.update(v => !v); }

  addToCart(item: InventoryItemDto): void {
    this.cartItems.update(cart => {
      const existing = cart.find(c => c.item.id === item.id);
      if (existing)
        return cart.map(c => c.item.id === item.id ? { ...c, quantity: c.quantity + 1 } : c);
      return [...cart, { item, quantity: 1 }];
    });
    this.toast.success(`${item.name} added to cart.`);
  }

  removeFromCart(itemId: number): void {
    this.cartItems.update(cart => cart.filter(c => c.item.id !== itemId));
  }

  updateCartQty(itemId: number, qty: number): void {
    if (qty < 1) { this.removeFromCart(itemId); return; }
    this.cartItems.update(cart =>
      cart.map(c => c.item.id === itemId ? { ...c, quantity: qty } : c)
    );
  }

  clearCart(): void {
    this.cartItems.set([]);
    this.toast.info('Cart cleared.');
  }

  // ── 12. Add item modal ────────────────────────────────────────────────────
  openAddItemModal(): void {
    this.addForm = { sku: '', name: '', description: '', category: '', unitOfMeasure: 'pcs', unitPrice: null };
    this.addFormErrors = {};
    this.addItemModalOpen.set(true);
  }

  closeAddItemModal(): void { this.addItemModalOpen.set(false); }

  submitAddItem(): void {
    this.addFormErrors = {};
    if (!this.addForm.sku.trim())    this.addFormErrors['sku']      = 'SKU is required';
    if (!this.addForm.name.trim())   this.addFormErrors['name']     = 'Name is required';
    if (!this.addForm.category)      this.addFormErrors['category'] = 'Category is required';
    if (!this.addForm.unitOfMeasure) this.addFormErrors['uom']      = 'Unit of measure is required';
    if (!this.addForm.unitPrice || this.addForm.unitPrice <= 0)
                                     this.addFormErrors['price']    = 'Valid price required (> 0)';

    if (Object.keys(this.addFormErrors).length) return;

    // TODO: wire to POST /Inventory once the create endpoint is implemented
    this.toast.info('Add item API endpoint not yet implemented.');
    this.addItemModalOpen.set(false);
  }

  // ── 13. Details modal ─────────────────────────────────────────────────────
  openDetails(item: InventoryItemDto): void {
    this.selectedItem.set(item);
    this.detailsModalOpen.set(true);
  }

  closeDetails(): void { this.detailsModalOpen.set(false); }

  addToCartFromDetails(item: InventoryItemDto): void {
    this.addToCart(item);
    this.closeDetails();
  }
}
