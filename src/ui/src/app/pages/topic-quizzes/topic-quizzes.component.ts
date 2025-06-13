import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { UserService, User } from '../../services/user.service';
import { Subscription } from 'rxjs';

export interface Quiz {
  id: string;
  name: string;
  description: string;
  moduleCode: string;
  topicName: string;
  duration: string;
}

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
  currentUser: User | null = null;
  allowQuizManagement = false;
  private userSubscription?: Subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService,
    private userService: UserService
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

    this.userSubscription = this.userService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;
      this.allowQuizManagement = (user?.role === 'Tutor'
        || user?.role === 'Lecturer'
        || user?.role === 'Administrator') ?? false;
    });

    this.loadQuizzes();
  }

  ngOnDelete() {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }

  loadQuizzes(): void {
    this.apiService.getQuizzes(this.topicId).subscribe((x:any) =>
      this.quizzes = x
    );
  }

  parseTimeSpanToMinutes(timeSpan: string): number {
    const parts = timeSpan.split(':').map(Number);
    const hours = parts[0];
    const minutes = parts[1];
    const seconds = parts[2];
    return Math.round(((hours * 60 + minutes) * 60 + seconds) / 60);
  }

  getQuizById(id: string): Quiz | undefined {
    return this.quizzes.find(quiz => quiz.id === id);
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
    this.apiService.createQuizAttempt(quiz.id).subscribe(attemptId => {
    this.router.navigate(
      ['/quiz', quiz.id, 'attempt'],
      {
        queryParams: {
          quizAttemptId: attemptId
        }
      }
    );
  });
  }

  editQuiz(quiz: Quiz): void {
    this.router.navigate(
      ['/quiz/management'],
      {
        queryParams: {
          moduleId: this.moduleId,
          moduleName: this.moduleName,
          topicId: this.topicId,
          quizId: quiz.id
        }
      }
    );
  }

  createQuiz(): void {
    this.router.navigate(
      ['/quiz/management'],
      {
        queryParams: {
          moduleId: this.moduleId,
          moduleName: this.moduleName,
          topicId: this.topicId
        }
      }
    );
  }
}
