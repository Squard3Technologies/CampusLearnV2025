import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicQuizzesComponent } from './topic-quizzes.component';

describe('TopicQuizzesComponent', () => {
  let component: TopicQuizzesComponent;
  let fixture: ComponentFixture<TopicQuizzesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopicQuizzesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TopicQuizzesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
