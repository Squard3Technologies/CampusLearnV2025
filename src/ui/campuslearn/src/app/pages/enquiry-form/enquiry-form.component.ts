import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { EnquiryService } from '../../services/enquiry/enquiry.service';
import { AuthService } from '../../services/auth/auth.service';
import { ModuleService } from '../../services/module/module.service';
import { Module } from '../../models/module/module.model';

@Component({
  selector: 'app-enquiry-form',
  templateUrl: './enquiry-form.component.html',
  styleUrls: ['./enquiry-form.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule]
})
export class EnquiryFormComponent implements OnInit {
  enquiryForm: FormGroup;
  currentUserId: string = '';
  modules: Module[] = [];
  isLoading = true;
  formSubmitted = false;
  submitError = false;

  constructor(
    private fb: FormBuilder,
    private enquiryService: EnquiryService,
    private authService: AuthService,
    private moduleService: ModuleService
  ) {
    this.enquiryForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      content: ['', [Validators.required, Validators.minLength(20)]],
      moduleId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // Get current user ID
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUserId = user.id;

        // Get modules the user is enrolled in
        this.moduleService.getModulesByUser(user.id).subscribe(modules => {
          this.modules = modules;
          this.isLoading = false;
        });
      }
    });
  }

  submitEnquiry(): void {
    if (this.enquiryForm.invalid || !this.currentUserId) {
      return;
    }

    const enquiryData = {
      title: this.enquiryForm.value.title,
      content: this.enquiryForm.value.content,
      createdByUserId: this.currentUserId
    };

    this.enquiryService.createEnquiry(enquiryData).subscribe(
      (response) => {
        this.formSubmitted = true;
        this.enquiryForm.reset();
      },
      (error) => {
        this.submitError = true;
      }
    );
  }
}
