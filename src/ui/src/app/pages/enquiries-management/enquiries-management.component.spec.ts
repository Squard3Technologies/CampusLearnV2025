import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnquiriesManagementComponent } from './enquiries-management.component';

describe('EnquiriesManagementComponent', () => {
  let component: EnquiriesManagementComponent;
  let fixture: ComponentFixture<EnquiriesManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnquiriesManagementComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EnquiriesManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
