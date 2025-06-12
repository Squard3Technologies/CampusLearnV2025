import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.scss'
})
export class QuizzesComponent {
  constructor(private router: Router,
    private userService: UserService,
    private apiService: ApiService) {}

  activeQuizzes: any[] = [];
  currentUser: User | null = null;

  ngOnInit(): void {
    /*this.userService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;

      if (user) {
        this.activeQuizzes = this.apiService.getActiveQuizzes();
      }
    });*/

    this.apiService.getActiveQuizzes().subscribe(quizzes => {
      this.activeQuizzes = quizzes;
    });
  }

  navigateToQuizHistory() {
    this.router.navigate(['/quiz/history']);
  }
}
