import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { User, UserService } from '../../services/user.service';

@Component({
  selector: 'app-modules',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './modules.component.html',
  styleUrl: './modules.component.scss'
})
export class ModulesComponent {
  currentUser: User | null = null;
    userModules: any[] = [];

    constructor(private userService: UserService) {}

    ngOnInit(): void {
      this.userService.currentUser$.subscribe((user: User | null) => {
        this.currentUser = user;

        if (user) {
          this.userModules = [
            { id: '1', title: 'Introduction to Programming', progress: 60 },
            { id: '2', title: 'Web Development Fundamentals', progress: 30 },
            { id: '3', title: 'Database Systems', progress: 75 }
          ];
        }
      });
    }
}
