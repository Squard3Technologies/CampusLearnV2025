import { Component, OnInit, OnDestroy, AfterViewInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  isAdmin = false;
  isNavbarCollapsed = false;
  isDropdownOpen = false;
  private userSubscription?: Subscription;

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Subscribe to user changes
    this.userSubscription = this.userService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      this.isAdmin = user?.role === 'admin';
    });
  }

  ngOnDestroy(): void {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }

  // Toggle the mobile navbar
  toggleNavbar(): void {
    this.isNavbarCollapsed = !this.isNavbarCollapsed;
    // Close dropdown when toggling navbar
    this.isDropdownOpen = false;
  }

  // Toggle the user dropdown menu
  toggleDropdown(event: Event): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  // Close dropdown when clicking outside
  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    // Check if the click is outside the dropdown
    if (!target.closest('.dropdown-menu') && !target.classList.contains('dropdown-toggle')) {
      this.isDropdownOpen = false;
    }
  }

  logout(): void {
    this.userService.clearCurrentUser();
    this.isDropdownOpen = false;
    this.router.navigate(['/login']);
  }
}
