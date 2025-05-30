import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Topic, mockTopics, mockModuleNames } from '../../mock-data';

@Component({
  selector: 'app-module',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './module.component.html',
  styleUrl: './module.component.scss'
})
export class ModuleComponent implements OnInit {
  moduleId: string | null = null;
  moduleName: string | null = null;
  topics: Topic[] = [];
  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.moduleId = params.get('id');
      this.loadModuleData();
    });
  }
  loadModuleData() {
    // Get module name from mock data
    this.moduleName = mockModuleNames[this.moduleId || ''] || mockModuleNames['default'];
    
    // Get topics and set the moduleId for each topic
    this.topics = mockTopics.map(topic => ({
      ...topic,
      moduleId: this.moduleId || ''
    }));
    
    console.log(`Loading module with ID: ${this.moduleId}`);
  }
}
