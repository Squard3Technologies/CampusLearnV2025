import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { QuestionType } from '../../enums/enums';

interface QuizQuestion {
  id: string;
  name: string;
  questionType: number;
  questionOptions: QuizQuestionOption[];
  userAnswer: string | null;
}

interface QuizQuestionOption {
  id: string;
  isCorrect: boolean;
  name: string;
  questionId: string;
}

interface Quiz {
  id: number;
  title: string;
  description: string;
  duration: string; // in seconds
  totalPoints: number;
  questions: QuizQuestion[];
}

@Component({
  selector: 'app-quiz-attempt',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './quiz-attempt.component.html',
  styleUrl: './quiz-attempt.component.scss'
})
export class QuizAttemptComponent implements OnInit, OnDestroy {

  quizId: string | null = null;
  currentQuiz: Quiz | null = null;
  questions: QuizQuestion[] = [];
  currentQuestionIndex: number = 0;
  timeRemaining: number = 0;
  private timerInterval: any;
  showSubmissionModal: boolean = false;

  constructor(private route: ActivatedRoute, private router: Router, private apiService: ApiService) {}

  ngOnInit() {
    this.loadQuiz();
    this.startTimer();
  }

  ngOnDestroy() {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
  }

  // Load quiz data - replace with actual service call
  loadQuiz() {
    this.route.paramMap.subscribe(params => {
      this.quizId = params.get('id');
    });

    if (this.quizId != null) {
      this.apiService.getQuizDetails(this.quizId).subscribe((x: Quiz | null) => {
        this.currentQuiz = x;
        if (this.currentQuiz?.duration != null) {
          this.timeRemaining = this.parseTimeSpan(this.currentQuiz.duration);
        }
      });
    }
  }

  parseTimeSpan(timeSpan: string): number {
    const parts = timeSpan.split(':').map(Number);
    const hours = parts[0];
    const minutes = parts[1];
    const seconds = parts[2];
    return ((hours * 60 + minutes) * 60 + seconds);
  }

  // Timer functionality
  startTimer() {
    this.timerInterval = setInterval(() => {
      if (this.timeRemaining > 0) {
        this.timeRemaining--;
      } else {
        this.autoSubmitQuiz();
      }
    }, 1000);
  }

  formatTime(seconds: number): string {
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    const secs = seconds % 60;
    
    if (hours > 0) {
      return `${hours}:${minutes.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
    }
    return `${minutes}:${secs.toString().padStart(2, '0')}`;
  }

  get currentQuestion(): QuizQuestion | null {
    return this.currentQuiz?.questions[this.currentQuestionIndex] || null;
  }

  // Progress calculations
  get totalQuestions(): number {
    return this.currentQuiz?.questions.length ?? 0;
  }

  get answeredQuestions(): number {
    return this.currentQuiz?.questions.filter(q => q.userAnswer && q.userAnswer.trim() !== '').length ?? 0;
  }

  get overallProgress(): number {
    return this.totalQuestions > 0 ? (this.answeredQuestions / this.totalQuestions) * 100 : 0;
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
    if (this.currentQuiz && this.currentQuestionIndex < this.currentQuiz.questions.length - 1) {
      this.currentQuestionIndex++;
    }
  }

  goToQuestion(questionIndex: number) {
    if (this.currentQuiz && questionIndex >= 0 && questionIndex < this.currentQuiz.questions.length) {
      this.currentQuestionIndex = questionIndex;
    }
  }

  // Answer management
  updateAnswer(questionId: string, answer: string) {
    // Find and update the question
    const question = this.currentQuiz?.questions.find(q => q.id === questionId);
    if (question) {
      question.userAnswer = answer;
    }
  }
  

  // Section actions
  reviewSection() {
    this.currentQuestionIndex = 0;
  }

  // Quiz submission
  canSubmitQuiz(): boolean {
    return this.answeredQuestions > 0;
  }

  submitQuiz() {
    this.showSubmissionModal = true;
  }

  closeSubmissionModal() {
    this.showSubmissionModal = false;
  }

  confirmSubmission() {
    this.showSubmissionModal = false;
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
    
    // Calculate score and navigate to results
    const score = this.calculateScore();
    console.log('Quiz submitted with score:', score);
    if (this.currentQuiz != null) {
      const payload = {
        questionAnswers: this.currentQuiz.questions
          .filter(q => q.userAnswer) // only include answered questions
          .map(q => ({
            questionId: q.id,
            selectedQuestionOptionId: q.userAnswer
          }))
      };

      this.apiService.submitQuizAttempt(this.quizId, payload).subscribe(x => {
        this.router.navigate([`/quiz/${x}/review`]);
      });
    }
  }

  autoSubmitQuiz() {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
    alert('Time\'s up! Your quiz will be submitted automatically.');
    this.confirmSubmission();
  }

  saveDraft() {
    // Save current progress to localStorage or service
    const quizData = {
      quizId: this.currentQuiz?.id,
      questions: this.currentQuiz?.questions,
      currentQuestionIndex: this.currentQuestionIndex,
      timeRemaining: this.timeRemaining,
      savedAt: new Date().toISOString()
    };
    
    localStorage.setItem('quiz_draft', JSON.stringify(quizData));
    alert('Quiz progress saved as draft!');
  }

  private calculateScore(): number {
    let totalScore = 0;
    let maxScore = this.currentQuiz?.questions.length ?? 0;

    for (const question of this.currentQuiz?.questions ?? []) {

      switch (question.questionType) {
        case QuestionType.MultipleChoice:
          if (question.questionOptions.find(x => x.id == question.userAnswer)?.isCorrect ?? false) {
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
