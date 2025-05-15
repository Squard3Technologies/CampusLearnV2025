import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { ModuleService } from '../../services/module/module.service';
import { Module } from '../../models/module/module.model';
import { UserRole } from '../../models/user/user.module';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  title = 'CampusLearn';
  userModules: Module[] = [];
  isAdmin = false;
  isTutor = false;

  constructor(
    private authService: AuthService,
    private moduleService: ModuleService
  ) {}

  ngOnInit(): void {
    // Check user role for conditional UI elements
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.isAdmin = user.role === UserRole.Admin;
        this.isTutor = user.role === UserRole.Tutor;

        // Load user-specific modules
        this.moduleService.getModulesByUser(user.id).subscribe(modules => {
          this.userModules = modules;
        });
      }
    });
  }
}
