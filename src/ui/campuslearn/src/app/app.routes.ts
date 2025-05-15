import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { ModulesComponent } from './pages/modules/modules.component';
import { ModuleDetailComponent } from './pages/module-detail/module-detail.component';
import { TopicDetailComponent } from './pages/topic-detail/topic-detail.component';
import { DiscussionDetailComponent } from './pages/discussion-detail/discussion-detail.component';
import { QuizDetailComponent } from './pages/quiz-detail/quiz-detail.component';
import { EnquiryFormComponent } from './pages/enquiry-form/enquiry-form.component';
import { TutorDashboardComponent } from './pages/tutor-dashboard/tutor-dashboard.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'modules', component: ModulesComponent },
  { path: 'modules/:id', component: ModuleDetailComponent },
  { path: 'topics/:id', component: TopicDetailComponent },
  { path: 'discussions/:id', component: DiscussionDetailComponent },
  { path: 'quizzes/:id', component: QuizDetailComponent },
  { path: 'ask', component: EnquiryFormComponent },
  { path: 'tutor-dashboard', component: TutorDashboardComponent }
];
