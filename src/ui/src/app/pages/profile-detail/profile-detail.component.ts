import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-profile-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './profile-detail.component.html',
  styleUrl: './profile-detail.component.scss'
})
export class ProfileDetailComponent {

  constructor(private router: Router) {}

  // Navigation methods for sidebar tabs
  navigateToTopicSections() {
    this.router.navigate(['/topic-sections']);
  }

  navigateToTutorSubscription() {
    this.router.navigate(['/tutor-subscription']);
  }

  navigateToTopicSubscription() {
    this.router.navigate(['/topic-subscription']);
  }

  navigateToChangePassword() {
    this.router.navigate(['/change-password']);
  }

  logout() {
    // Clear user session data here
    localStorage.removeItem('user');
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  // Form action methods
  updateProfile() {
    // Implement profile update logic here
    console.log('Updating profile...');
  }
}
