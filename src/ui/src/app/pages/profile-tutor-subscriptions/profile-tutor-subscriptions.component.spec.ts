import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileTutorSubscriptionsComponent } from './profile-tutor-subscriptions.component';

describe('ProfileTutorSubscriptionsComponent', () => {
  let component: ProfileTutorSubscriptionsComponent;
  let fixture: ComponentFixture<ProfileTutorSubscriptionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileTutorSubscriptionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProfileTutorSubscriptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
