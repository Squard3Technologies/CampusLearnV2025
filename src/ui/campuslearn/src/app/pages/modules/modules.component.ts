import { Component, OnInit } from '@angular/core';
import { ModuleService } from '../../services/module/module.service';
import { Module } from '../../models/module/module.model';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { UserRole } from '../../models/user/user.module';

@Component({
  selector: 'app-modules',
  templateUrl: './modules.component.html',
  styleUrls: ['./modules.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class ModulesComponent implements OnInit {
  modules: Module[] = [];
  userModules: Module[] = [];
  isLoading = true;
  isAdmin = false;

  constructor(
    private moduleService: ModuleService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Check if user is admin for conditional UI elements
    this.authService.currentUser$.subscribe(user => {
      this.isAdmin = user?.role === UserRole.Admin || user?.role === UserRole.Tutor;
    });

    // Get all modules
    this.moduleService.getAllModules().subscribe(modules => {
      this.modules = modules;
      this.isLoading = false;
    });

    // Get user-specific modules
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.moduleService.getModulesByUser(user.id).subscribe(userModules => {
          this.userModules = userModules;
        });
      }
    });
  }
}
