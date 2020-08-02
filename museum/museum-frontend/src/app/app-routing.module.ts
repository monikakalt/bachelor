import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { EventsCalendarComponent } from './components/events-calendar/events-calendar.component';
import { ChroniclesComponent } from './components/chronicles/chronicles.component';
import { StudentsComponent } from './components/students/students.component';
import { AddStudentComponent } from './components/add-student/add-student.component';
import { TeachersComponent } from './components/teachers/teachers.component';
import { ClassesComponent } from './components/classes/classes.component';
import { AddEventComponent } from './components/add-event/add-event.component';
import { AddChronicleComponent } from './components/add-chronicle/add-chronicle.component';
import { AuthGuard } from './services/guards/auth.guard';
import { GraduatesComponent } from './graduates/graduates.component';
import { AddGraduatesComponent } from './components/add-graduates/add-graduates.component';
import { AddClassComponent } from './components/add-class/add-class.component';
import { AddTeacherComponent } from './components/add-teacher/add-teacher.component';

const routes: Routes = [
  { path: '' , component: HomeComponent},
  { path: 'prisijungti' , component: LoginComponent},
  { path: 'kalendorius' , component: EventsCalendarComponent, canActivate: [AuthGuard]},
  { path: 'metrasciai' , component: ChroniclesComponent, canActivate: [AuthGuard]},
  { path: 'mokiniai' , component: StudentsComponent, canActivate: [AuthGuard]},
  { path: 'laidos' , component: GraduatesComponent, canActivate: [AuthGuard]},
  { path: 'mokytojai' , component: TeachersComponent, canActivate: [AuthGuard]},
  { path: 'klases' , component: ClassesComponent, canActivate: [AuthGuard]},
  { path: 'klase/:id' , component: AddClassComponent, canActivate: [AuthGuard]},
  { path: 'nauja-klase' , component: AddClassComponent, canActivate: [AuthGuard]},
  { path: 'mokinys/:id' , component: AddStudentComponent, canActivate: [AuthGuard]},
  { path: 'naujas-mokinys' , component: AddStudentComponent, canActivate: [AuthGuard]},
  { path: 'rezervacija/:id' , component: AddEventComponent, canActivate: [AuthGuard]},
  { path: 'nauja-rezervacija' , component: AddEventComponent, canActivate: [AuthGuard]},
  { path: 'metrastis/:id' , component: AddChronicleComponent, canActivate: [AuthGuard]},
  { path: 'naujas-metrastis' , component: AddChronicleComponent, canActivate: [AuthGuard]},
  { path: 'laida/:id' , component: AddGraduatesComponent, canActivate: [AuthGuard]},
  { path: 'nauja-laida' , component: AddGraduatesComponent, canActivate: [AuthGuard]},
  { path: 'mokytojas/:id' , component: AddTeacherComponent, canActivate: [AuthGuard]},
  { path: 'naujas-mokytojas' , component: AddTeacherComponent, canActivate: [AuthGuard]},
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
