import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPendingRegistrationsComponent } from './admin-pending-registrations.component';

describe('AdminPendingRegistrationsComponent', () => {
  let component: AdminPendingRegistrationsComponent;
  let fixture: ComponentFixture<AdminPendingRegistrationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminPendingRegistrationsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminPendingRegistrationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
