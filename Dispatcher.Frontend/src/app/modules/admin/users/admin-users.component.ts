import { Component, inject, OnInit } from '@angular/core';
import { ListUsersRequest, ListUsersResponse } from '../../../api-services/users/users-api.model';
import { UsersApiService } from '../../../api-services/users/users-api.service';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-admin-users',
  standalone: false,
  templateUrl: './admin-users.component.html',
  styleUrl: './admin-users.component.scss',
})
export class AdminUsersComponent implements OnInit{
  private userService = inject(UsersApiService)
  users: ListUsersResponse | null = null;
  loading: boolean = false;
  error: string | null = null;

  searchTerm = '';
  selectedRole = '';

  private searchSubject = new Subject<string>();
  constructor(){
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(searchTerm => {
      this.loadUsers(1, searchTerm, this.selectedRole);
    });
  }

   ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(pageNumber: number = 1, search?: string, role?: string): void {
    this.loading = true;
    this.error = null;

    const request = new ListUsersRequest();
    request.paging.page = pageNumber;
    request.paging.pageSize = 10;
    
    if (search !== undefined) {
      request.search = search;
    }

    // Add role filter if your API supports it
    // request.role = role;

    this.userService.getUsers(request).subscribe({
      next: (response) => {
        this.users = response;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load users. Please try again.';
        this.loading = false;
        console.error('Error loading users:', err);
      }
    });
  }

  onSearchInput(): void {
    this.searchSubject.next(this.searchTerm);
  }

  onSearch(): void {
    this.loadUsers(1, this.searchTerm, this.selectedRole);
  }

  onRoleFilter(): void {
    this.loadUsers(1, this.searchTerm, this.selectedRole);
  }

  onPageChange(pageNumber: number): void {
    this.loadUsers(pageNumber, this.searchTerm || undefined, this.selectedRole);
  }
}
