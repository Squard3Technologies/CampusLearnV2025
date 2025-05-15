import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ModuleService } from '../../services/module/module.service';
import { TopicService } from '../../services/topic/topic.service';
import { Module } from '../../models/module/module.model';
import { Topic } from '../../models/topic/topic.model';
import { AuthService } from '../../services/auth/auth.service';
import { UserRole } from '../../models/user/user.module';

@Component({
  selector: 'app-module-detail',
  templateUrl: './module-detail.component.html',
  styleUrls: ['./module-detail.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class ModuleDetailComponent implements OnInit {
  moduleId: string = '';
  module: Module | undefined;
  topics: Topic[] = [];
  isLoading = true;
  isEnrolled = false;
  isAdmin = false;

  constructor(
    private route: ActivatedRoute,
    private moduleService: ModuleService,
    private topicService: TopicService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.moduleId = id;
        this.loadModuleData();
      }
    });

    // Check if user is admin for conditional UI elements
    this.authService.currentUser$.subscribe(user => {
      this.isAdmin = user?.role === UserRole.Admin || user?.role === UserRole.Tutor;

      if (user) {
        // Check if user is enrolled in this module
        this.moduleService.getModulesByUser(user.id).subscribe(modules => {
          this.isEnrolled = modules.some(m => m.id === this.moduleId);
        });
      }
    });
  }

  loadModuleData(): void {
    // Load module details
    this.moduleService.getModuleById(this.moduleId).subscribe(module => {
      this.module = module;

      // Load topics for this module
      if (module) {
        this.topicService.getTopicsByModule(module.id).subscribe(topics => {
          this.topics = topics;
          this.isLoading = false;
        });
      }
    });
  }

  enrollInModule(): void {
    this.authService.currentUser$.subscribe(user => {
      if (user && this.moduleId) {
        this.moduleService.assignModuleToUser(user.id, this.moduleId).subscribe(() => {
          this.isEnrolled = true;
        });
      }
    }).unsubscribe();
  }
}
