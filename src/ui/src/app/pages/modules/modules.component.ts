import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { User, UserService } from '../../services/user.service';
import { mockModules } from '../../mock-data';

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
        this.userModules = mockModules;
      }
    });
  }
}
