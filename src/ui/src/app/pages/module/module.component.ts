import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Topic, mockTopics, mockModuleNames } from '../../mock-data';
import { ApiService } from '../../services/api.service';

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
  constructor(private route: ActivatedRoute, private apiService: ApiService) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.moduleId = params.get('id');
      this.loadModuleData();
    });
  }
  loadModuleData() {
    if (this.moduleId != null) {
      this.apiService.getModules().subscribe((modules:any) => {
        const module = modules.find((x: any) => x.id === this.moduleId);
        if (module) {
          this.moduleName = module.code + " - " + module.name;
        }
      });

      this.apiService.getTopics(this.moduleId).subscribe((x:any) =>
        this.topics = x.body
      );
    }

    // Get topics and set the moduleId for each topic
    this.topics = mockTopics.map(topic => ({
      ...topic,
      moduleId: this.moduleId || ''
    }));
    
    console.log(`Loading module with ID: ${this.moduleId}`);
  }
}
