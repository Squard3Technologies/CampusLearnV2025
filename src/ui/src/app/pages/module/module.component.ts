import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-module',
  standalone: true,
  imports: [],
  templateUrl: './module.component.html',
  styleUrl: './module.component.scss'
})
export class ModuleComponent implements OnInit {
  moduleId: string | null = null;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.moduleId = params.get('id');
      this.loadModuleData();
    });
  }

  loadModuleData() {
    console.log(`Loading module with ID: ${this.moduleId}`);
  }
}
