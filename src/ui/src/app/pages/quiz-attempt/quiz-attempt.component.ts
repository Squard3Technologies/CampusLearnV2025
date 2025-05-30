import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface QuizQuestion {
  id: number;
  question: string;
  type: 'trueFalse' | 'shortText' | 'longText';
  points: number;
  userAnswer?: string;
  correctAnswer?: string;
}

interface QuizSection {
  id: number;
  title: string;
  questions: QuizQuestion[];
  isCompleted: boolean;
}

interface Quiz {
  id: number;
  title: string;
  description: string;
  timeLimit: number; // in seconds
  totalPoints: number;
}

@Component({
  selector: 'app-quiz-attempt',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './quiz-attempt.component.html',
  styleUrl: './quiz-attempt.component.scss'
})
export class QuizAttemptComponent implements OnInit, OnDestroy {

  currentQuiz: Quiz | null = null;
  quizSections: QuizSection[] = [];
  currentSectionIndex: number = 0;
  currentQuestionIndex: number = 0;
  timeRemaining: number = 0;
  private timerInterval: any;
  showSubmissionModal: boolean = false;

  constructor(private router: Router) {}

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
    this.currentQuiz = {
      id: 1,
      title: 'Computer Science Fundamentals Quiz',
      description: 'Test your knowledge of basic computer science concepts',
      timeLimit: 3600, // 1 hour
      totalPoints: 100
    };

    this.quizSections = [
      {
        id: 1,
        title: 'Programming Basics',
        isCompleted: false,
        questions: [
          {
            id: 1,
            question: 'Python is a compiled programming language.',
            type: 'trueFalse',
            points: 5,
            correctAnswer: 'false'
          },
          {
            id: 2,
            question: 'What is the purpose of a variable in programming?',
            type: 'shortText',
            points: 10
          },
          {
            id: 3,
            question: 'Explain the difference between a compiler and an interpreter. Provide examples of languages that use each approach.',
            type: 'longText',
            points: 15
          }
        ]
      },
      {
        id: 2,
        title: 'Data Structures',
        isCompleted: false,
        questions: [
          {
            id: 4,
            question: 'A stack follows the Last In, First Out (LIFO) principle.',
            type: 'trueFalse',
            points: 5,
            correctAnswer: 'true'
          },
          {
            id: 5,
            question: 'What is the time complexity of searching in a binary search tree?',
            type: 'shortText',
            points: 10
          },
          {
            id: 6,
            question: 'Compare and contrast arrays and linked lists. Discuss their advantages and disadvantages in terms of memory usage and performance.',
            type: 'longText',
            points: 20
          }
        ]
      },
      {
        id: 3,
        title: 'Algorithms',
        isCompleted: false,
        questions: [
          {
            id: 7,
            question: 'Binary search can only be performed on sorted arrays.',
            type: 'trueFalse',
            points: 5,
            correctAnswer: 'true'
          },
          {
            id: 8,
            question: 'What is the Big O notation for bubble sort?',
            type: 'shortText',
            points: 10
          },
          {
            id: 9,
            question: 'Describe the quicksort algorithm. Explain its best-case and worst-case time complexities and when each might occur.',
            type: 'longText',
            points: 20
          }
        ]
      }
    ];

    this.timeRemaining = this.currentQuiz.timeLimit;
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

  // Getters for current items
  get currentSection(): QuizSection | null {
    return this.quizSections[this.currentSectionIndex] || null;
  }

  get currentQuestion(): QuizQuestion | null {
    return this.currentSection?.questions[this.currentQuestionIndex] || null;
  }

  // Progress calculations
  get totalQuestions(): number {
    return this.quizSections.reduce((total, section) => total + section.questions.length, 0);
  }

  get answeredQuestions(): number {
    return this.quizSections.reduce((total, section) => 
      total + section.questions.filter(q => q.userAnswer && q.userAnswer.trim() !== '').length, 0);
  }

  get overallProgress(): number {
    return this.totalQuestions > 0 ? (this.answeredQuestions / this.totalQuestions) * 100 : 0;
  }

  // Question type helper
  getQuestionTypeLabel(type: string): string {
    switch (type) {
      case 'trueFalse': return 'True/False';
      case 'shortText': return 'Short Answer';
      case 'longText': return 'Essay';
      default: return 'Question';
    }
  }

  // Navigation methods
  navigateToSection(sectionIndex: number) {
    if (sectionIndex >= 0 && sectionIndex < this.quizSections.length) {
      this.currentSectionIndex = sectionIndex;
      this.currentQuestionIndex = 0;
    }
  }

  nextSection() {
    if (this.currentSectionIndex < this.quizSections.length - 1) {
      this.currentSectionIndex++;
      this.currentQuestionIndex = 0;
    }
  }

  previousQuestion() {
    if (this.currentQuestionIndex > 0) {
      this.currentQuestionIndex--;
    } else if (this.currentSectionIndex > 0) {
      this.currentSectionIndex--;
      this.currentQuestionIndex = (this.quizSections[this.currentSectionIndex]?.questions.length || 1) - 1;
    }
  }

  nextQuestion() {
    if (this.currentSection && this.currentQuestionIndex < this.currentSection.questions.length - 1) {
      this.currentQuestionIndex++;
    } else if (this.currentSectionIndex < this.quizSections.length - 1) {
      this.currentSectionIndex++;
      this.currentQuestionIndex = 0;
    }
  }

  goToQuestion(questionIndex: number) {
    if (this.currentSection && questionIndex >= 0 && questionIndex < this.currentSection.questions.length) {
      this.currentQuestionIndex = questionIndex;
    }
  }

  // Answer management
  updateAnswer(questionId: number, answer: string) {
    // Find and update the question
    for (const section of this.quizSections) {
      const question = section.questions.find(q => q.id === questionId);
      if (question) {
        question.userAnswer = answer;
        this.checkSectionCompletion();
        break;
      }
    }
  }

  checkSectionCompletion() {
    for (const section of this.quizSections) {
      const allAnswered = section.questions.every(q => q.userAnswer && q.userAnswer.trim() !== '');
      section.isCompleted = allAnswered;
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
    
    // In a real application, you would send the results to a service
    this.router.navigate(['/quiz-results'], { 
      queryParams: { 
        quizId: this.currentQuiz?.id, 
        score: score,
        totalQuestions: this.totalQuestions,
        answeredQuestions: this.answeredQuestions
      } 
    });
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
      sections: this.quizSections,
      currentSectionIndex: this.currentSectionIndex,
      currentQuestionIndex: this.currentQuestionIndex,
      timeRemaining: this.timeRemaining,
      savedAt: new Date().toISOString()
    };
    
    localStorage.setItem('quiz_draft', JSON.stringify(quizData));
    alert('Quiz progress saved as draft!');
  }

  private calculateScore(): number {
    let totalScore = 0;
    let maxScore = 0;

    for (const section of this.quizSections) {
      for (const question of section.questions) {
        maxScore += question.points;
        
        if (question.userAnswer && question.correctAnswer) {
          // Simple scoring for true/false questions
          if (question.type === 'trueFalse' && 
              question.userAnswer.toLowerCase() === question.correctAnswer.toLowerCase()) {
            totalScore += question.points;
          }
          // For text questions, you'd typically need manual grading or more sophisticated scoring
          else if (question.type === 'shortText' || question.type === 'longText') {
            // For demo purposes, give partial credit if answered
            if (question.userAnswer.trim() !== '') {
              totalScore += Math.floor(question.points * 0.7); // 70% for attempt
            }
          }
        }
      }
    }

    return Math.round((totalScore / maxScore) * 100);
  }
}
