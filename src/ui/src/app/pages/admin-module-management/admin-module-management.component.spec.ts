import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminModuleManagementComponent } from './admin-module-management.component';

describe('AdminModuleManagementComponent', () => {
  let component: AdminModuleManagementComponent;
  let fixture: ComponentFixture<AdminModuleManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminModuleManagementComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminModuleManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
