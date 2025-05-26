import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileTopicSubscriptionsComponent } from './profile-topic-subscriptions.component';

describe('ProfileTopicSubscriptionsComponent', () => {
  let component: ProfileTopicSubscriptionsComponent;
  let fixture: ComponentFixture<ProfileTopicSubscriptionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileTopicSubscriptionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProfileTopicSubscriptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
