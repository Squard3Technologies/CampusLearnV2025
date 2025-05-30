import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface TutorSubscription {
  id: number;
  tutorId: number;
  tutorName: string;
  tutorAvatar: string;
  subject: string;
  qualification: string;
  experience: string;
  hourlyRate: number;
  totalSessions: number;
  nextSession: Date | null;
  rating: number;
  status: 'Active' | 'Expired' | 'Pending' | 'Cancelled';
}

@Component({
  selector: 'app-profile-tutor-subscriptions',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './profile-tutor-subscriptions.component.html',
  styleUrl: './profile-tutor-subscriptions.component.scss'
})
export class ProfileTutorSubscriptionsComponent implements OnInit {

  subscriptions: TutorSubscription[] = [];
  filteredSubscriptions: TutorSubscription[] = [];
  paginatedSubscriptions: TutorSubscription[] = [];
  
  searchTerm: string = '';
  showSearch: boolean = false;
  currentPage: number = 1;
  itemsPerPage: number = 6;
  totalPages: number = 1;

  constructor(private router: Router) {}

  ngOnInit() {
    this.loadSubscriptions();
  }

  // Mock data - replace with actual service call
  loadSubscriptions() {
    this.subscriptions = [
      {
        id: 1,
        tutorId: 101,
        tutorName: 'Dr. Sarah Johnson',
        tutorAvatar: 'assets/images/tutors/sarah.jpg',
        subject: 'Advanced Mathematics',
        qualification: 'PhD in Mathematics',
        experience: '8 years teaching experience',
        hourlyRate: 350,
        totalSessions: 24,
        nextSession: new Date('2025-01-18T14:00:00'),
        rating: 4.8,
        status: 'Active'
      },
      {
        id: 2,
        tutorId: 102,
        tutorName: 'Prof. Michael Chen',
        tutorAvatar: 'assets/images/tutors/michael.jpg',
        subject: 'Computer Science',
        qualification: 'MSc Computer Science',
        experience: '10 years industry + 5 years teaching',
        hourlyRate: 400,
        totalSessions: 18,
        nextSession: new Date('2025-01-20T16:30:00'),
        rating: 4.9,
        status: 'Active'
      },
      {
        id: 3,
        tutorId: 103,
        tutorName: 'Dr. Emily Rodriguez',
        tutorAvatar: 'assets/images/tutors/emily.jpg',
        subject: 'Physics',
        qualification: 'PhD in Physics',
        experience: '12 years research + teaching',
        hourlyRate: 380,
        totalSessions: 30,
        nextSession: null,
        rating: 4.7,
        status: 'Expired'
      },
      {
        id: 4,
        tutorId: 104,
        tutorName: 'Prof. David Williams',
        tutorAvatar: 'assets/images/tutors/david.jpg',
        subject: 'Business Studies',
        qualification: 'MBA, CPA',
        experience: '15 years corporate + 6 years teaching',
        hourlyRate: 320,
        totalSessions: 0,
        nextSession: new Date('2025-02-01T10:00:00'),
        rating: 4.6,
        status: 'Pending'
      },
      {
        id: 5,
        tutorId: 105,
        tutorName: 'Ms. Lisa Thompson',
        tutorAvatar: 'assets/images/tutors/lisa.jpg',
        subject: 'Digital Marketing',
        qualification: 'MSc Marketing, Google Certified',
        experience: '7 years agency + 3 years teaching',
        hourlyRate: 290,
        totalSessions: 12,
        nextSession: new Date('2025-01-22T13:00:00'),
        rating: 4.5,
        status: 'Active'
      },
      {
        id: 6,
        tutorId: 106,
        tutorName: 'Dr. James Wilson',
        tutorAvatar: 'assets/images/tutors/james.jpg',
        subject: 'Chemistry',
        qualification: 'PhD in Organic Chemistry',
        experience: '9 years teaching + research',
        hourlyRate: 360,
        totalSessions: 15,
        nextSession: null,
        rating: 4.4,
        status: 'Cancelled'
      }
    ];

    this.filteredSubscriptions = [...this.subscriptions];
    this.updatePagination();
  }

  // Navigation methods
  navigateToProfile() {
    this.router.navigate(['/profile']);
  }


  navigateToTopicSubscription() {
    this.router.navigate(['/profile/topics']);
  }

  navigateToChangePassword() {
    this.router.navigate(['/change-password']);
  }

  logout() {
    localStorage.removeItem('user');
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }

  // Search functionality
  toggleSearch() {
    this.showSearch = !this.showSearch;
    if (!this.showSearch) {
      this.searchTerm = '';
      this.onSearchChange();
    }
  }

  onSearchChange() {
    if (this.searchTerm.trim() === '') {
      this.filteredSubscriptions = [...this.subscriptions];
    } else {
      this.filteredSubscriptions = this.subscriptions.filter(subscription =>
        subscription.tutorName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        subscription.subject.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        subscription.qualification.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }
    this.currentPage = 1;
    this.updatePagination();
  }

  // Pagination methods
  updatePagination() {
    this.totalPages = Math.ceil(this.filteredSubscriptions.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedSubscriptions = this.filteredSubscriptions.slice(startIndex, endIndex);
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  goToPage(page: number) {
    this.currentPage = page;
    this.updatePagination();
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  // Utility methods
  getStars(rating: number): string {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 !== 0;
    let stars = '★'.repeat(fullStars);
    if (hasHalfStar) stars += '☆';
    const emptyStars = 5 - Math.ceil(rating);
    stars += '☆'.repeat(emptyStars);
    return stars;
  }

  // Action methods
  viewTutorProfile(tutorId: number) {
    this.router.navigate(['/tutor-profile', tutorId]);
  }

  scheduleSession(subscriptionId: number) {
    this.router.navigate(['/schedule-session', subscriptionId]);
  }

  unsubscribe(subscriptionId: number) {
    if (confirm('Are you sure you want to unsubscribe from this tutor?')) {
      const subscription = this.subscriptions.find(s => s.id === subscriptionId);
      if (subscription) {
        subscription.status = 'Cancelled';
        subscription.nextSession = null;
        this.filteredSubscriptions = [...this.subscriptions];
        this.updatePagination();
      }
      console.log('Unsubscribed from tutor:', subscriptionId);
    }
  }

  resubscribe(subscriptionId: number) {
    if (confirm('Would you like to resubscribe to this tutor?')) {
      const subscription = this.subscriptions.find(s => s.id === subscriptionId);
      if (subscription) {
        subscription.status = 'Pending';
        this.filteredSubscriptions = [...this.subscriptions];
        this.updatePagination();
      }
      console.log('Resubscribed to tutor:', subscriptionId);
    }
  }

  findTutors() {
    this.router.navigate(['/find-tutors']);
  }
}
