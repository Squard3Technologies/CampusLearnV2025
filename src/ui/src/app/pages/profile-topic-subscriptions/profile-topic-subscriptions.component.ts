import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface TopicSubscription {
  id: number;
  topicName: string;
  description: string;
  tutorName: string;
  startDate: Date;
  duration: string;
  price: number;
  status: 'Active' | 'Expired' | 'Pending' | 'Cancelled';
}

@Component({
  selector: 'app-profile-topic-subscriptions',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './profile-topic-subscriptions.component.html',
  styleUrl: './profile-topic-subscriptions.component.scss'
})
export class ProfileTopicSubscriptionsComponent implements OnInit {

  subscriptions: TopicSubscription[] = [];
  filteredSubscriptions: TopicSubscription[] = [];
  paginatedSubscriptions: TopicSubscription[] = [];
  
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
        topicName: 'Advanced Mathematics',
        description: 'Comprehensive advanced mathematics course covering calculus, linear algebra, and differential equations.',
        tutorName: 'Dr. Sarah Johnson',
        startDate: new Date('2025-01-15'),
        duration: '3 months',
        price: 1500,
        status: 'Active'
      },
      {
        id: 2,
        topicName: 'Computer Science Fundamentals',
        description: 'Introduction to programming concepts, data structures, and algorithms.',
        tutorName: 'Prof. Michael Chen',
        startDate: new Date('2025-02-01'),
        duration: '4 months',
        price: 2000,
        status: 'Active'
      },
      {
        id: 3,
        topicName: 'Physics Mechanics',
        description: 'Classical mechanics, thermodynamics, and electromagnetism fundamentals.',
        tutorName: 'Dr. Emily Rodriguez',
        startDate: new Date('2024-12-01'),
        duration: '2 months',
        price: 1200,
        status: 'Expired'
      },
      {
        id: 4,
        topicName: 'Business Ethics',
        description: 'Corporate responsibility, ethical decision-making, and professional conduct.',
        tutorName: 'Prof. David Williams',
        startDate: new Date('2025-03-01'),
        duration: '6 weeks',
        price: 800,
        status: 'Pending'
      },
      {
        id: 5,
        topicName: 'Digital Marketing',
        description: 'Social media marketing, SEO, content strategy, and analytics.',
        tutorName: 'Ms. Lisa Thompson',
        startDate: new Date('2025-01-20'),
        duration: '8 weeks',
        price: 1100,
        status: 'Active'
      }
    ];

    this.filteredSubscriptions = [...this.subscriptions];
    this.updatePagination();
  }

  // Navigation methods
  navigateToProfile() {
    this.router.navigate(['/profile']);
  }

  navigateToTutorSubscription() {
    this.router.navigate(['/profile/tutors']);
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
        subscription.topicName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        subscription.tutorName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        subscription.description.toLowerCase().includes(this.searchTerm.toLowerCase())
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

  // Action methods
  viewTopicDetails(subscriptionId: number) {
    this.router.navigate(['/topic-details', subscriptionId]);
  }

  unsubscribe(subscriptionId: number) {
    if (confirm('Are you sure you want to unsubscribe from this topic?')) {
      // Update the subscription status
      const subscription = this.subscriptions.find(s => s.id === subscriptionId);
      if (subscription) {
        subscription.status = 'Cancelled';
        this.filteredSubscriptions = [...this.subscriptions];
        this.updatePagination();
      }
      // In a real app, you would call a service to update the backend
      console.log('Unsubscribed from topic:', subscriptionId);
    }
  }

  browseTopic() {
    this.router.navigate(['/topics']);
  }
}
