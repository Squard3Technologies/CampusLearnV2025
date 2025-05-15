import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { ModuleService } from '../../services/module/module.service';
import { TopicService } from '../../services/topic/topic.service';
import { EnquiryService } from '../../services/enquiry/enquiry.service';
import { Module } from '../../models/module/module.model';
import { Topic } from '../../models/topic/topic.model';
import { Enquiry, EnquiryStatus } from '../../models/enquiry/enquiry.model';
import { UserRole } from '../../models/user/user.module';

@Component({
  selector: 'app-tutor-dashboard',
  templateUrl: './tutor-dashboard.component.html',
  styleUrls: ['./tutor-dashboard.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class TutorDashboardComponent implements OnInit {
  currentUserId: string = '';
  isAuthorized = false;
  isLoading = true;

  modules: Module[] = [];
  topics: Topic[] = [];
  openEnquiries: Enquiry[] = [];

  activeTab: 'modules' | 'topics' | 'enquiries' = 'modules';

  constructor(
    private authService: AuthService,
    private moduleService: ModuleService,
    private topicService: TopicService,
    private enquiryService: EnquiryService
  ) {}

  ngOnInit(): void {
    // Check if the user has tutor or admin role
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUserId = user.id;
        this.isAuthorized = user.role === UserRole.Admin || user.role === UserRole.Tutor;

        if (this.isAuthorized) {
          this.loadData();
        } else {
          this.isLoading = false;
        }
      }
    });
  }

  loadData(): void {
    // Load modules
    this.moduleService.getAllModules().subscribe(modules => {
      this.modules = modules;

      // Load topics created by this tutor
      this.topicService.getTopicsByUser(this.currentUserId).subscribe(topics => {
        this.topics = topics;

        // Load open enquiries
        this.enquiryService.getOpenEnquiries().subscribe(enquiries => {
          this.openEnquiries = enquiries;
          this.isLoading = false;
        });
      });
    });
  }

  setActiveTab(tab: 'modules' | 'topics' | 'enquiries'): void {
    this.activeTab = tab;
  }

  resolveEnquiry(enquiryId: string): void {
    this.enquiryService.resolveEnquiry(enquiryId, this.currentUserId)
      .subscribe((updatedEnquiry) => {
        // Remove the resolved enquiry from the list
        this.openEnquiries = this.openEnquiries.filter(e => e.id !== enquiryId);
      });
  }
}
