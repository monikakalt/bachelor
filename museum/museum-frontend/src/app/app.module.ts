import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationService } from './services/authentication.service';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { JwtInterceptor } from './services/helpers/jwt.interceptor';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HomeComponent } from './components/home/home.component';
import { ChroniclesComponent } from './components/chronicles/chronicles.component';
import { StudentsComponent } from './components/students/students.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { FullCalendarModule } from 'ng-fullcalendar';
import { EventsCalendarComponent } from './components/events-calendar/events-calendar.component';
import { AddChronicleComponent } from './components/add-chronicle/add-chronicle.component';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRippleModule } from '@angular/material/core';
import { MatInputModule, MatButtonModule, MatNativeDateModule } from '@angular/material';
import { FileUploadModule } from 'ng2-file-upload';
import { StudentService } from './services/students.service';
import { AlertService } from './services/alert.service';
import { UserService } from './services/user.service';
import { AddEventComponent } from './components/add-event/add-event.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
import { AgGridModule } from 'ag-grid-angular';
import { AddStudentComponent } from './components/add-student/add-student.component';
import { ClassesComponent } from './components/classes/classes.component';
import { TeachersComponent } from './components/teachers/teachers.component';
import { TeachersService } from './services/teachers.service';
import { ClassInfoService } from './services/classes.service';
import { OWL_DATE_TIME_LOCALE } from 'ng-pick-datetime';
import { EventsService } from './services/events.service';
import {DatePipe, registerLocaleData} from '@angular/common';
import { ChronicleService } from './services/chronicle.service';
import { NguCarouselModule } from '@ngu/carousel';
import { AuthGuard } from './services/guards/auth.guard';
import { GraduatesComponent } from './graduates/graduates.component';
import { AddGraduatesComponent } from './components/add-graduates/add-graduates.component';
import { GraduatesService } from './services/graduates.service';
import { AddClassComponent } from './components/add-class/add-class.component';
import localeLt from '@angular/common/locales/lt';
registerLocaleData(localeLt);
import 'hammerjs';
import 'hammer-timejs';
import { PhotoCarouselDirective } from './components/chronicles/photo-directive/photo-carousel.directive';
import { ColumnSelectionModalComponent } from './components/column-selection-modal/column-selection-modal.component';
import { ModalService } from './services/modal.service';
import { AddTeacherComponent } from './components/add-teacher/add-teacher.component';
import { NgxImageGalleryModule } from 'ngx-image-gallery';

@NgModule({
  declarations: [
    AppComponent,
    ColumnSelectionModalComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    ChroniclesComponent,
    StudentsComponent,
    LoginComponent,
    RegisterComponent,
    EventsCalendarComponent,
    AddChronicleComponent,
    LoginComponent,
    AddEventComponent,
    AddStudentComponent,
    ClassesComponent,
    TeachersComponent,
    GraduatesComponent,
    AddGraduatesComponent,
    AddClassComponent,
    PhotoCarouselDirective,
    AddTeacherComponent
  ],
  imports: [
    NgbModule,
    MDBBootstrapModule.forRoot(),
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FullCalendarModule,
    MatDatepickerModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatRippleModule,
    MatNativeDateModule,
    FileUploadModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    AgGridModule.withComponents([]),
    AppRoutingModule,
    NguCarouselModule,
    NgxImageGalleryModule
  ],
  entryComponents: [AddStudentComponent],
  exports: [
    MatDatepickerModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatRippleModule,
    MatButtonModule,
    MatNativeDateModule
  ],
  providers: [
    DatePipe,
    AuthenticationService,
    StudentService,
    AlertService,
    EventsService,
    UserService,
    TeachersService,
    AuthGuard,
    ClassInfoService,
    ChronicleService,
    ModalService,
    GraduatesService,
    {provide: OWL_DATE_TIME_LOCALE, useValue: 'lt'},
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
