import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Quiz, mockQuizzes } from '../../../mock-data';

@Component({
  selector: 'app-topic-quizzes',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './topic-quizzes.component.html',
  styleUrl: './topic-quizzes.component.scss'
})
export class TopicQuizzesComponent implements OnInit {
  topicId!: string;
  moduleId!: string;
  moduleName!: string;
  quizzes: Quiz[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get route parameters
    this.route.params.subscribe(params => {
      this.topicId = params['id'];
    });

    // Get query parameters
    this.route.queryParams.subscribe(params => {
      this.moduleId = params['moduleId'];
      this.moduleName = params['moduleName'];
    });

    // Load quizzes for this topic
    this.loadQuizzes();
  }
  loadQuizzes(): void {
    //this.quizzes = this.getQuizzesByTopicId(this.topicId);
  }

  // Helper function to get quizzes by topic ID
  getQuizzesByTopicId(topicId: number): Quiz[] {
    return mockQuizzes.filter(quiz => quiz.topicId === topicId);
  }

  // Helper function to get quiz by ID
  getQuizById(id: number): Quiz | undefined {
    return mockQuizzes.find(quiz => quiz.id === id);
  }

  navigateToTopicOverview(): void {
    this.router.navigate(['/topic', this.topicId], {
      queryParams: { 
        moduleId: this.moduleId, 
        moduleName: this.moduleName 
      }
    });
  }

  navigateToMaterials(): void {
    this.router.navigate(['/topic', this.topicId, 'learning-material'], {
      queryParams: { 
        moduleId: this.moduleId, 
        moduleName: this.moduleName 
      }
    });
  }

  navigateToDiscussions(): void {
    this.router.navigate(['/topic', this.topicId, 'discussions'], {
      queryParams: { 
        moduleId: this.moduleId, 
        moduleName: this.moduleName 
      }
    });
  }

  startQuiz(quiz: Quiz): void {
    if (quiz.status === 'Active' && (!quiz.maxAttempts || quiz.attempts! < quiz.maxAttempts)) {
      // Navigate to quiz attempt page
      this.router.navigate(['/quiz', quiz.id, 'attempt']);
    }
  }

  reviewQuiz(quiz: Quiz): void {
    // Navigate to quiz review page
    this.router.navigate(['/quiz', quiz.id, 'review']);
  }

  getDifficultyClass(difficulty: string): string {
    const classes: { [key: string]: string } = {
      'Easy': 'text-success',
      'Medium': 'text-warning',
      'Hard': 'text-danger'
    };
    return classes[difficulty] || 'text-secondary';
  }

  getStatusClass(status: string): string {
    const classes: { [key: string]: string } = {
      'Active': 'text-success',
      'Draft': 'text-warning',
      'Archived': 'text-secondary'
    };
    return classes[status] || 'text-secondary';
  }

  canStartQuiz(quiz: Quiz): boolean {
    return quiz.status === 'Active' && (!quiz.maxAttempts || quiz.attempts! < quiz.maxAttempts);
  }

  isQuizOverdue(quiz: Quiz): boolean {
    return quiz.dueDate ? new Date() > quiz.dueDate : false;
  }

  // Helper methods for summary statistics
  getActiveQuizzesCount(): number {
    return this.quizzes.filter(quiz => quiz.status === 'Active').length;
  }

  getPendingQuizzesCount(): number {
    return this.quizzes.filter(quiz => quiz.status === 'Draft').length;
  }

  getCompletedQuizzesCount(): number {
    return this.quizzes.filter(quiz => quiz.attempts && quiz.attempts > 0).length;
  }
}
