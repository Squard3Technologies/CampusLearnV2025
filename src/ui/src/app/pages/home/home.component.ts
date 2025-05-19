import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  currentUser: User | null = null;
  recentModules: any[] = [];

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;

      // Mock some modules for demonstration
      if (user) {
        this.recentModules = [
          { id: '1', title: 'Introduction to Programming', progress: 60 },
          { id: '2', title: 'Web Development Fundamentals', progress: 30 },
          { id: '3', title: 'Database Systems', progress: 75 }
        ];
      }
    });
  }
}
