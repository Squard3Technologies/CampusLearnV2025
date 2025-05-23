import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-module',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './module.component.html',
  styleUrl: './module.component.scss'
})
export class ModuleComponent implements OnInit {
  moduleId: string | null = null;
  moduleName :string | null = null;
  topics: any[] = [];
  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.moduleId = params.get('id');
      this.loadModuleData();
    });
  }

  loadModuleData() {
    // Define the Topic interface

    this.moduleName= "test Module";
    interface Topic {
      id: string;
      createdByUserId: string;
      title: string;
      description: string;
      dateCreated: Date;
      moduleId: string;
    }

    // Sample topics array for the current module
    const sampleTopics: Topic[] = [
      {
        id: '123e4567-e89b-12d3-a456-426614174000',
        createdByUserId: '98765432-e89b-12d3-a456-426614174000',
        title: 'Introduction to Angular',
        description: 'Learn the basics of Angular framework and component architecture',
        dateCreated: new Date('2023-01-15'),
        moduleId: this.moduleId || ''
      },
      {
        id: '223e4567-e89b-12d3-a456-426614174001',
        createdByUserId: '88765432-e89b-12d3-a456-426614174001',
        title: 'Component Communication',
        description: 'Understanding how components interact in Angular applications',
        dateCreated: new Date('2023-01-20'),
        moduleId: this.moduleId || ''
      }
    ];

    // TODO: Replace with actual API call to fetch topics for this module
    this.topics = sampleTopics;
    console.log(`Loading module with ID: ${this.moduleId}`);
  }
}
