import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { Quiz, Question, QuestionOption, QuizAttempt, QuizAttemptStatus, QuizAttemptQuestionAnswer } from '../../models';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  private mockQuizzes: Quiz[] = [
    {
      id: '1',
      title: 'Introduction to Programming Quiz',
      createdByUserId: '1',
      topicId: '1',
      dateCreated: new Date('2025-03-01'),
      duration: '00:30:00' // 30 minutes
    },
    {
      id: '2',
      title: 'Variables and Data Types Quiz',
      createdByUserId: '1',
      topicId: '2',
      dateCreated: new Date('2025-03-10'),
      duration: '00:45:00' // 45 minutes
    }
  ];

  private mockQuestions: Question[] = [
    {
      id: '1',
      quizId: '1',
      questionType: 1, // Boolean
      dateCreated: new Date('2025-03-01')
    },
    {
      id: '2',
      quizId: '1',
      questionType: 2, // Multiple choice
      dateCreated: new Date('2025-03-01')
    },
    {
      id: '3',
      quizId: '2',
      questionType: 2, // Multiple choice
      dateCreated: new Date('2025-03-10')
    }
  ];

  private mockQuestionOptions: QuestionOption[] = [
    { id: '1', questionId: '1', value: 'True', isCorrect: true, dateCreated: new Date('2025-03-01') },
    { id: '2', questionId: '1', value: 'False', isCorrect: false, dateCreated: new Date('2025-03-01') },

    { id: '3', questionId: '2', value: 'Option A', isCorrect: false, dateCreated: new Date('2025-03-01') },
    { id: '4', questionId: '2', value: 'Option B', isCorrect: true, dateCreated: new Date('2025-03-01') },
    { id: '5', questionId: '2', value: 'Option C', isCorrect: false, dateCreated: new Date('2025-03-01') },

    { id: '6', questionId: '3', value: 'Option X', isCorrect: false, dateCreated: new Date('2025-03-10') },
    { id: '7', questionId: '3', value: 'Option Y', isCorrect: true, dateCreated: new Date('2025-03-10') },
    { id: '8', questionId: '3', value: 'Option Z', isCorrect: false, dateCreated: new Date('2025-03-10') }
  ];

  private mockQuizAttempts: QuizAttempt[] = [
    {
      id: '1',
      userId: '3',
      quizId: '1',
      dateCreated: new Date('2025-03-05'),
      assignedByUserId: '1',
      status: QuizAttemptStatus.Completed,
      dateAttempted: new Date('2025-03-06'),
      attemptDuration: '00:25:30'
    },
    {
      id: '2',
      userId: '4',
      quizId: '1',
      dateCreated: new Date('2025-03-05'),
      assignedByUserId: '1',
      status: QuizAttemptStatus.Assigned
    }
  ];

  private mockQuizAttemptAnswers: QuizAttemptQuestionAnswer[] = [
    {
      id: '1',
      quizAttemptId: '1',
      questionId: '1',
      questionOptionId: '1',
      isCorrect: true
    },
    {
      id: '2',
      quizAttemptId: '1',
      questionId: '2',
      questionOptionId: '5',
      isCorrect: false
    }
  ];

  constructor() { }

  // Quiz methods
  getAllQuizzes(): Observable<Quiz[]> {
    return of(this.mockQuizzes).pipe(delay(500));
  }

  getQuizById(id: string): Observable<Quiz | undefined> {
    const quiz = this.mockQuizzes.find(q => q.id === id);
    return of(quiz).pipe(delay(300));
  }

  getQuizzesByTopic(topicId: string): Observable<Quiz[]> {
    const quizzes = this.mockQuizzes.filter(quiz => quiz.topicId === topicId);
    return of(quizzes).pipe(delay(500));
  }

  createQuiz(quiz: Partial<Quiz>): Observable<Quiz> {
    const newQuiz: Quiz = {
      id: Date.now().toString(),
      title: quiz.title || '',
      createdByUserId: quiz.createdByUserId || '',
      topicId: quiz.topicId || '',
      dateCreated: new Date(),
      duration: quiz.duration || '00:30:00'
    };

    this.mockQuizzes.push(newQuiz);
    return of(newQuiz).pipe(delay(500));
  }

  updateQuiz(id: string, quiz: Partial<Quiz>): Observable<Quiz | undefined> {
    const index = this.mockQuizzes.findIndex(q => q.id === id);
    if (index !== -1) {
      this.mockQuizzes[index] = {
        ...this.mockQuizzes[index],
        ...quiz
      };
      return of(this.mockQuizzes[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  deleteQuiz(id: string): Observable<boolean> {
    const initialLength = this.mockQuizzes.length;
    this.mockQuizzes = this.mockQuizzes.filter(q => q.id !== id);

    // Also delete associated questions, options, attempts
    this.mockQuestions = this.mockQuestions.filter(q => q.quizId !== id);
    this.mockQuizAttempts = this.mockQuizAttempts.filter(qa => qa.quizId !== id);

    return of(initialLength > this.mockQuizzes.length).pipe(delay(500));
  }

  // Question methods
  getQuestionsByQuiz(quizId: string): Observable<Question[]> {
    const questions = this.mockQuestions.filter(question => question.quizId === quizId);
    return of(questions).pipe(delay(500));
  }

  createQuestion(question: Partial<Question>): Observable<Question> {
    const newQuestion: Question = {
      id: Date.now().toString(),
      quizId: question.quizId || '',
      questionType: question.questionType || 1,
      dateCreated: new Date()
    };

    this.mockQuestions.push(newQuestion);
    return of(newQuestion).pipe(delay(500));
  }

  // Question Option methods
  getOptionsByQuestion(questionId: string): Observable<QuestionOption[]> {
    const options = this.mockQuestionOptions.filter(option => option.questionId === questionId);
    return of(options).pipe(delay(500));
  }

  createQuestionOption(option: Partial<QuestionOption>): Observable<QuestionOption> {
    const newOption: QuestionOption = {
      id: Date.now().toString(),
      questionId: option.questionId || '',
      value: option.value || '',
      isCorrect: option.isCorrect || false,
      dateCreated: new Date()
    };

    this.mockQuestionOptions.push(newOption);
    return of(newOption).pipe(delay(500));
  }

  // Quiz Attempt methods
  getQuizAttemptsByUser(userId: string): Observable<QuizAttempt[]> {
    const attempts = this.mockQuizAttempts.filter(attempt => attempt.userId === userId);
    return of(attempts).pipe(delay(500));
  }

  getQuizAttemptsByQuiz(quizId: string): Observable<QuizAttempt[]> {
    const attempts = this.mockQuizAttempts.filter(attempt => attempt.quizId === quizId);
    return of(attempts).pipe(delay(500));
  }

  assignQuizToUser(quizId: string, userId: string, assignedByUserId: string): Observable<QuizAttempt> {
    const newAttempt: QuizAttempt = {
      id: Date.now().toString(),
      userId,
      quizId,
      dateCreated: new Date(),
      assignedByUserId,
      status: QuizAttemptStatus.Assigned
    };

    this.mockQuizAttempts.push(newAttempt);
    return of(newAttempt).pipe(delay(500));
  }

  startQuizAttempt(attemptId: string): Observable<QuizAttempt | undefined> {
    const index = this.mockQuizAttempts.findIndex(a => a.id === attemptId);
    if (index !== -1) {
      this.mockQuizAttempts[index] = {
        ...this.mockQuizAttempts[index],
        status: QuizAttemptStatus.InProgress,
        dateAttempted: new Date()
      };
      return of(this.mockQuizAttempts[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  completeQuizAttempt(attemptId: string, duration: string): Observable<QuizAttempt | undefined> {
    const index = this.mockQuizAttempts.findIndex(a => a.id === attemptId);
    if (index !== -1) {
      this.mockQuizAttempts[index] = {
        ...this.mockQuizAttempts[index],
        status: QuizAttemptStatus.Completed,
        attemptDuration: duration
      };
      return of(this.mockQuizAttempts[index]).pipe(delay(500));
    }
    return of(undefined).pipe(delay(300));
  }

  // Quiz Attempt Answer methods
  submitAnswer(answer: Partial<QuizAttemptQuestionAnswer>): Observable<QuizAttemptQuestionAnswer> {
    // Check if the answer is correct
    let isCorrect = false;
    if (answer.questionOptionId) {
      const option = this.mockQuestionOptions.find(o => o.id === answer.questionOptionId);
      isCorrect = option?.isCorrect || false;
    }

    const newAnswer: QuizAttemptQuestionAnswer = {
      id: Date.now().toString(),
      quizAttemptId: answer.quizAttemptId || '',
      questionId: answer.questionId || '',
      questionOptionId: answer.questionOptionId || '',
      isCorrect
    };

    this.mockQuizAttemptAnswers.push(newAnswer);
    return of(newAnswer).pipe(delay(500));
  }

  getAnswersByAttempt(attemptId: string): Observable<QuizAttemptQuestionAnswer[]> {
    const answers = this.mockQuizAttemptAnswers.filter(answer => answer.quizAttemptId === attemptId);
    return of(answers).pipe(delay(500));
  }

  calculateQuizScore(attemptId: string): Observable<number> {
    const answers = this.mockQuizAttemptAnswers.filter(answer => answer.quizAttemptId === attemptId);
    if (answers.length === 0) return of(0).pipe(delay(300));

    const correctAnswers = answers.filter(answer => answer.isCorrect).length;
    const score = (correctAnswers / answers.length) * 100;

    return of(score).pipe(delay(500));
  }
}
