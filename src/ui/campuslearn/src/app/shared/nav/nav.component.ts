import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { UserService } from '../../services/user/user.service';
import { Subscription } from 'rxjs';
import { User, UserRole } from '../../models/user/user.module';

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
  isTutor = false;
  private userSubscription?: Subscription;

  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Subscribe to user changes
    this.userSubscription = this.userService.currentUser$.subscribe(user => {
      console.log('Nav component received user update:', user); // Add logging
      this.currentUser = user;

      if (user) {
        this.isAdmin = user.role === UserRole.Admin;
        this.isTutor = user.role === UserRole.Tutor || user.role === UserRole.Lecturer;
      } else {
        this.isAdmin = false;
        this.isTutor = false;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }

  logout(): void {
    this.userService.clearCurrentUser();
    this.router.navigate(['/login']);
  }
}
