import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { QuizService } from '../../services/quiz/quiz.service';
import { AuthService } from '../../services/auth/auth.service';
import {
  Quiz,
  Question,
  QuestionOption,
  QuizAttempt,
  QuizAttemptStatus
} from '../../models';

@Component({
  selector: 'app-quiz-detail',
  templateUrl: './quiz-detail.component.html',
  styleUrls: ['./quiz-detail.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule]
})
export class QuizDetailComponent implements OnInit {
  quizId: string = '';
  quiz: Quiz | undefined;
  questions: Question[] = [];
  questionsWithOptions: { question: Question; options: QuestionOption[] }[] = [];
  currentUserId: string = '';
  isLoading = true;
  quizAttempt: QuizAttempt | undefined;
  quizStarted = false;
  quizCompleted = false;
  quizScore: number | undefined;
  quizForm: FormGroup;
  startTime: Date | undefined;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private quizService: QuizService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.quizForm = this.fb.group({});
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.quizId = id;
        this.loadQuizData();
      }
    });

    // Get current user ID
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUserId = user.id;
      }
    });
  }

  loadQuizData(): void {
    // Load quiz details
    this.quizService.getQuizById(this.quizId).subscribe(quiz => {
      this.quiz = quiz;

      if (quiz) {
        // Load questions for this quiz
        this.quizService.getQuestionsByQuiz(quiz.id).subscribe(questions => {
          this.questions = questions;

          // Create a form control for each question
          const formGroupConfig: any = {};

          // Load options for each question
          const questionLoads = questions.map(question => {
            return this.quizService.getOptionsByQuestion(question.id);
          });

          // Wait for all options to be loaded
          if (questionLoads.length > 0) {
            Promise.all(questionLoads.map(load => load.toPromise())).then(optionsArrays => {
              this.questionsWithOptions = questions.map((question, index) => {
                formGroupConfig[question.id] = [''];
                return {
                  question: question,
                  options: optionsArrays[index] || []
                };
              });

              this.quizForm = this.fb.group(formGroupConfig);
              this.isLoading = false;
            });
          } else {
            this.isLoading = false;
          }
        });
      }
    });
  }

  startQuiz(): void {
    if (!this.currentUserId || !this.quizId) return;

    // Create a new quiz attempt
    this.quizService.assignQuizToUser(this.quizId, this.currentUserId, this.currentUserId)
      .subscribe(attempt => {
        this.quizAttempt = attempt;

        // Start the quiz attempt
        this.quizService.startQuizAttempt(attempt.id).subscribe(updatedAttempt => {
          this.quizAttempt = updatedAttempt;
          this.quizStarted = true;
          this.startTime = new Date();
        });
      });
  }

  submitQuiz(): void {
    if (!this.quizAttempt || !this.startTime) return;

    // Calculate duration
    const endTime = new Date();
    const durationMs = endTime.getTime() - this.startTime.getTime();
    const durationMinutes = Math.floor(durationMs / 60000);
    const durationSeconds = Math.floor((durationMs % 60000) / 1000);
    const formattedDuration = `00:${durationMinutes.toString().padStart(2, '0')}:${durationSeconds.toString().padStart(2, '0')}`;

    // Submit each answer
    const answerPromises = this.questionsWithOptions.map(qwo => {
      const questionId = qwo.question.id;
      const selectedOptionId = this.quizForm.get(questionId)?.value;

      if (selectedOptionId) {
        return this.quizService.submitAnswer({
          quizAttemptId: this.quizAttempt?.id,
          questionId,
          questionOptionId: selectedOptionId
        }).toPromise();
      }
      return Promise.resolve(null);
    });

    // Wait for all answers to be submitted
    Promise.all(answerPromises).then(() => {
      // Complete the quiz attempt
      if (this.quizAttempt) {
        this.quizService.completeQuizAttempt(this.quizAttempt.id, formattedDuration)
          .subscribe(() => {
            // Calculate the score
            if (this.quizAttempt) {
              this.quizService.calculateQuizScore(this.quizAttempt.id).subscribe(score => {
                this.quizScore = score;
                this.quizCompleted = true;
              });
            }
          });
      }
    });
  }
}
