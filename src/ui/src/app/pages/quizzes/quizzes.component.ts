import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.scss'
})
export class QuizzesComponent {
  constructor(private router: Router,
    private apiService: ApiService) {}

  activeQuizzes: any[] = [];

  ngOnInit(): void {
    this.apiService.getActiveQuizzes().subscribe(quizzes => {
      this.activeQuizzes = quizzes;
    });
  }

  navigateToQuizHistory() {
    this.router.navigate(['/quiz/history']);
  }
}
