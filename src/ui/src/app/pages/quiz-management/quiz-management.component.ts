import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { UserService, User } from '../../services/user.service';
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { QuestionType } from '../../enums/enums';

interface QuizQuestion {
  id: string;
  name: string;
  questionType: number;
  questionOptions: QuizQuestionOption[];
}

interface QuizQuestionOption {
  id: string;
  isCorrect: boolean;
  name: string;
  questionId: string;
}

interface Quiz {
  id: string;
  name: string;
  description: string;
  duration: string;
  durationMinites: number;
  questions: QuizQuestion[];
}

@Component({
  selector: 'app-quiz-management',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './quiz-management.component.html',
  styleUrl: './quiz-management.component.scss'
})
export class QuizManagementComponent {
  quizId: string | null = null;
  topicId: string | null = null;
  quiz: Quiz | null = null;
  moduleId: string | null = null;
  moduleName: string | null = null;
  durationMinutes: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService
  ) {}

  ngOnInit(){
    this.route.queryParams.subscribe(params => {
      this.quizId = params['quizId'];
      this.topicId = params['topicId'];
      this.moduleId = params['moduleId'];
      this.moduleName = params['moduleName'];
    });

    if (this.quizId != null) {
      this.apiService.getQuizDetails(this.quizId).subscribe((x: any) => {
        this.quiz = x;
        this.quiz.durationMinites = this.parseTimeSpan(x.duration);
      });
    }
  }

  getQuestionTypeLabel(type: number): string {
    switch (type) {
      case QuestionType.MultipleChoice: return 'Multiple Choice';
      default: return 'Question';
    }
  }

  editQuestion(question: QuizQuestion) {

  }

  deleteQuestion(question: QuizQuestion) {
    
  }

  parseTimeSpan(timeSpan: string): number {
    const parts = timeSpan.split(':').map(Number);
    const hours = parts[0];
    const minutes = parts[1];
    const seconds = parts[2];
    return Math.round(((hours * 60 + minutes) * 60) / 60);
  }

  formatTimeSpan(totalMinites: number): string {
    const totalSeconds = totalMinites * 60;
    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${pad(hours)}:${pad(minutes)}:${pad(seconds)}`;
  }

  onSubmit() {
    if (this.quizId == null) {
      this.apiService.createQuiz({
        name: this.quiz.name,
        description: this.quiz.description,
        topicId: this.topicId,
        duration: this.formatTimeSpan(this.quiz.durationMinites)
      }).subscribe((x: any) => {
        this.quizId = x
      })
    }
    else {
      this.apiService.updateQuiz(this.quizId, {
        name: this.quiz.name,
        description: this.quiz.description,
        topicId: this.topicId,
        duration: this.formatTimeSpan(this.quiz.durationMinites)
      }).subscribe();
    }
    
  }
}
