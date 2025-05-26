import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicDiscussionsComponent } from './topic-discussions.component';

describe('TopicDiscussionsComponent', () => {
  let component: TopicDiscussionsComponent;
  let fixture: ComponentFixture<TopicDiscussionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopicDiscussionsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TopicDiscussionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
