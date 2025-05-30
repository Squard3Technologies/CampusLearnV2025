import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface QuizHistory {
  id: string;
  quizName: string;
  moduleCode: string;
  topicName: string;
  duration: string;
  lastAttemptDateTime: Date;
  lastAttemptScore: string;
}

@Component({
  selector: 'app-quiz-history',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './quiz-history.component.html',
  styleUrl: './quiz-history.component.scss'
})
export class QuizHistoryComponent implements OnInit {
  quizHistory: QuizHistory[] = [];
  filteredQuizHistory: QuizHistory[] = [];
  
  // Filter and search properties
  searchTerm: string = '';
  selectedModule: string = '';
  selectedDateRange: string = '';
  sortBy: string = 'lastAttemptDateTime';
  sortDirection: 'asc' | 'desc' = 'desc';
  
  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 10;
  totalPages: number = 0;
  
  // Loading state
  isLoading: boolean = false;

  ngOnInit() {
    this.loadQuizHistory();
  }

  loadQuizHistory() {
    this.isLoading = true;
    
    // Mock data - replace with actual service call
    setTimeout(() => {
      this.quizHistory = [
        {
          id: '1',
          quizName: 'Angular Fundamentals Quiz',
          moduleCode: 'CS401',
          topicName: 'Introduction to Angular',
          duration: '30',
          lastAttemptDateTime: new Date('2024-05-25T14:30:00'),
          lastAttemptScore: '18 / 20'
        },
        {
          id: '2',
          quizName: 'Component Lifecycle Assessment',
          moduleCode: 'CS401',
          topicName: 'Angular Components',
          duration: '45',
          lastAttemptDateTime: new Date('2024-05-20T10:15:00'),
          lastAttemptScore: '25 / 30'
        },
        {
          id: '3',
          quizName: 'Services and Dependency Injection',
          moduleCode: 'CS401',
          topicName: 'Angular Services',
          duration: '60',
          lastAttemptDateTime: new Date('2024-05-18T16:45:00'),
          lastAttemptScore: '22 / 25'
        },
        {
          id: '4',
          quizName: 'Database Design Principles',
          moduleCode: 'CS302',
          topicName: 'Relational Database Design',
          duration: '50',
          lastAttemptDateTime: new Date('2024-05-15T09:30:00'),
          lastAttemptScore: '28 / 35'
        },
        {
          id: '5',
          quizName: 'SQL Query Optimization',
          moduleCode: 'CS302',
          topicName: 'Advanced SQL',
          duration: '40',
          lastAttemptDateTime: new Date('2024-05-10T13:20:00'),
          lastAttemptScore: '15 / 20'
        },
        {
          id: '6',
          quizName: 'Data Structures and Algorithms',
          moduleCode: 'CS201',
          topicName: 'Array and Linked Lists',
          duration: '75',
          lastAttemptDateTime: new Date('2024-05-05T11:00:00'),
          lastAttemptScore: '32 / 40'
        }
      ];
      
      this.filteredQuizHistory = [...this.quizHistory];
      this.updatePagination();
      this.isLoading = false;
    }, 1000);
  }

  // Filter and search methods
  applyFilters() {
    this.filteredQuizHistory = this.quizHistory.filter(quiz => {
      const matchesSearch = !this.searchTerm || 
        quiz.quizName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        quiz.topicName.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      const matchesModule = !this.selectedModule || quiz.moduleCode === this.selectedModule;
      
      const matchesDateRange = this.filterByDateRange(quiz.lastAttemptDateTime);
      
      return matchesSearch && matchesModule && matchesDateRange;
    });
    
    this.sortQuizHistory();
    this.currentPage = 1;
    this.updatePagination();
  }

  filterByDateRange(date: Date): boolean {
    if (!this.selectedDateRange) return true;
    
    const now = new Date();
    const daysDiff = Math.floor((now.getTime() - date.getTime()) / (1000 * 60 * 60 * 24));
    
    switch (this.selectedDateRange) {
      case 'last7days':
        return daysDiff <= 7;
      case 'last30days':
        return daysDiff <= 30;
      case 'last90days':
        return daysDiff <= 90;
      default:
        return true;
    }
  }

  sortQuizHistory() {
    this.filteredQuizHistory.sort((a, b) => {
      let aValue: any, bValue: any;
      
      switch (this.sortBy) {
        case 'quizName':
          aValue = a.quizName.toLowerCase();
          bValue = b.quizName.toLowerCase();
          break;
        case 'moduleCode':
          aValue = a.moduleCode;
          bValue = b.moduleCode;
          break;
        case 'lastAttemptDateTime':
          aValue = a.lastAttemptDateTime.getTime();
          bValue = b.lastAttemptDateTime.getTime();
          break;
        case 'score':
          aValue = this.getScorePercentage(a.lastAttemptScore);
          bValue = this.getScorePercentage(b.lastAttemptScore);
          break;
        default:
          return 0;
      }
      
      if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
      if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });
  }

  getScorePercentage(score: string): number {
    const [earned, total] = score.split(' / ').map(Number);
    return (earned / total) * 100;
  }

  setSortBy(field: string) {
    if (this.sortBy === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortDirection = 'desc';
    }
    this.applyFilters();
  }

  // Pagination methods
  updatePagination() {
    this.totalPages = Math.ceil(this.filteredQuizHistory.length / this.itemsPerPage);
  }

  get paginatedQuizHistory(): QuizHistory[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.filteredQuizHistory.slice(startIndex, endIndex);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  // Utility methods
  get uniqueModules(): string[] {
    return [...new Set(this.quizHistory.map(quiz => quiz.moduleCode))].sort();
  }

  getScoreClass(score: string): string {
    const percentage = this.getScorePercentage(score);
    if (percentage >= 90) return 'score-excellent';
    if (percentage >= 80) return 'score-good';
    if (percentage >= 70) return 'score-average';
    return 'score-poor';
  }

  getDaysAgo(date: Date): string {
    const now = new Date();
    const diffTime = Math.abs(now.getTime() - date.getTime());
    const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));
    
    if (diffDays === 0) return 'Today';
    if (diffDays === 1) return '1 day ago';
    return `${diffDays} days ago`;
  }

  reviewQuizAttempt(quizId: string) {
    // Navigate to quiz attempt review page
    console.log('Reviewing quiz attempt:', quizId);
    // this.router.navigate(['/quiz-attempt-review', quizId]);
  }

  retakeQuiz(quizId: string) {
    // Navigate to quiz attempt page
    console.log('Retaking quiz:', quizId);
    // this.router.navigate(['/quiz-attempt', quizId]);
  }

  clearFilters() {
    this.searchTerm = '';
    this.selectedModule = '';
    this.selectedDateRange = '';
    this.sortBy = 'lastAttemptDateTime';
    this.sortDirection = 'desc';
    this.currentPage = 1;
    this.applyFilters();
  }
}
