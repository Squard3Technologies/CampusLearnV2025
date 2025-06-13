import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { mockModules } from '../../mock-data';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-modules',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './modules.component.html',
  styleUrl: './modules.component.scss'
})
export class ModulesComponent {
  userModules: any[] = [];

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService.getModules().subscribe((x: any | null) => {
      this.userModules = x;
    });
  }
}
