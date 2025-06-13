import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { QuestionType } from '../../enums/enums';

interface QuizAttemptQuestion {
  id: string;
  name: string;
  questionType: number;
  questionOptions: QuizQuestionOption[];
}

interface QuizQuestionOption {
  id: string;
  isCorrect: boolean;
  isChosen: boolean;
  name: string;
  questionId: string;
}

interface QuizAttempt {
  id: string;
  name: string;
  questions: QuizAttemptQuestion[];
}

@Component({
  selector: 'app-quiz-attempt-review',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './quiz-attempt-review.component.html',
  styleUrl: './quiz-attempt-review.component.scss'
})
export class QuizAttemptReviewComponent implements OnInit, OnDestroy {

  quizAttemptId: string | null = null;
  currentQuizAttempt: QuizAttempt | null = null;
  //questions: QuizAttemptQuestion[] = [];
  currentQuestionIndex: number = 0;

  constructor(private route: ActivatedRoute, private router: Router, private apiService: ApiService) {}

  ngOnInit() {
    this.loadQuizAttempt();
  }

  ngOnDestroy() {
  }

  // Load quiz data - replace with actual service call
  loadQuizAttempt() {
    this.route.paramMap.subscribe(params => {
      this.quizAttemptId = params.get('id');
    });

    if (this.quizAttemptId != null) {
      this.apiService.getQuizAttemptHistory(this.quizAttemptId).subscribe((x: QuizAttempt | null) => {
        this.currentQuizAttempt = x;
      });
    }
  }

  get currentQuestion(): QuizAttemptQuestion | null {
    return this.currentQuizAttempt?.questions[this.currentQuestionIndex] || null;
  }

  // Progress calculations
  get totalQuestions(): number {
    return this.currentQuizAttempt?.questions.length ?? 0;
  }

  // Question type helper
  getQuestionTypeLabel(type: number): string {
    switch (type) {
      case QuestionType.MultipleChoice: return 'Multiple Choice';
      default: return 'Question';
    }
  }

  previousQuestion() {
    if (this.currentQuestionIndex > 0) {
      this.currentQuestionIndex--;
    }
  }

  nextQuestion() {
    if (this.currentQuizAttempt && this.currentQuestionIndex < this.currentQuizAttempt.questions.length - 1) {
      this.currentQuestionIndex++;
    }
  }

  goToQuestion(questionIndex: number) {
    if (this.currentQuizAttempt && questionIndex >= 0 && questionIndex < this.currentQuizAttempt.questions.length) {
      this.currentQuestionIndex = questionIndex;
    }
  }

  closeReview() {
    this.router.navigate([`/quiz/history`]);
  }

  getQuestionOptionClass(option: QuizQuestionOption) {
    if (!option.isChosen && !option.isCorrect)
      return "";
    return "option-label-" + (option.isCorrect ? "correct" : "incorrect");
  }

  private calculateScore(): number {
    let totalScore = 0;
    let maxScore = this.currentQuizAttempt?.questions.length ?? 0;

    for (const question of this.currentQuizAttempt?.questions ?? []) {

      switch (question.questionType) {
        case QuestionType.MultipleChoice:
          if (question.questionOptions.find(x => x.isCorrect) ?? false) {
            totalScore += 1;
          }
          break;
        default:
          break;
      }
    }

    return Math.round((totalScore / maxScore) * 100);
  }
}
