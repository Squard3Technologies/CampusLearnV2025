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
    this.userSubscription = this.userService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      this.isAdmin = user?.role === 'Administrator';
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

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
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
