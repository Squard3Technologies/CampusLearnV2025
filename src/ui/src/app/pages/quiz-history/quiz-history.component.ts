import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';

interface QuizHistory {
  id: string;
  quizId: string;
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
  imports: [CommonModule, RouterModule],
  templateUrl: './quiz-history.component.html',
  styleUrl: './quiz-history.component.scss'
})
export class QuizHistoryComponent {

  constructor(private router: Router,
    private apiService: ApiService) {}

  completedQuizzes: QuizHistory[] = [];

  ngOnInit(): void {
    this.apiService.getQuizHistory().subscribe((quizzes: QuizHistory[] | null) => {
      this.completedQuizzes = quizzes;
    });
  }

  navigateToAssignedQuizzes() {
    this.router.navigate(['/quizzes']);
  }
}
