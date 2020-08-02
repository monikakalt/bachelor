
import { Component, OnInit, OnChanges, OnDestroy, Input, ViewChild, EventEmitter, Output } from '@angular/core';
import { Student } from 'app/models/student';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { StudentService } from 'app/services/students.service';
import { Location } from '@angular/common';
import { TeachersService } from 'app/services/teachers.service';
import { ClassInfoService } from 'app/services/classes.service';
import { ClassInfo } from 'app/models/classInfo';
import { Teacher } from 'app/models/teacher';
import Swal from 'sweetalert2';
import { GraduatesService } from 'app/services/graduates.service';
import { Graduates } from 'app/models/graduates';

@Component({
  selector: 'app-add-student',
  templateUrl: './add-student.component.html',
  styleUrls: ['./add-student.component.scss']
})
export class AddStudentComponent implements OnInit, OnChanges, OnDestroy {

  @Input()
  student: Student;

  classes: ClassInfo[];
  teachers: Teacher[];
  graduates: Graduates[];

  emitter: EventEmitter<Student>;

  @Output()
  change: EventEmitter<string> = new EventEmitter<string>();

  activeParameter: any;

  constructor(private studentService: StudentService, private activateRoute: ActivatedRoute, private location: Location,
// tslint:disable-next-line: max-line-length
              private gradService: GraduatesService, private teacherService: TeachersService, private router: Router, private classesService: ClassInfoService) {
    this.getClasses();
    this.getTeachers();
    this.getGrads();
  }

  ngOnInit() {
    this.activateRoute.params.subscribe((params: Params) => {
      this.activeParameter = params.id;
    });

    if (this.activeParameter) {
      this.getStudentInfo();
    } else {
      this.student = new Student();
    }
  }

  ngOnChanges() {
  }

  getStudentInfo() {
    this.studentService.getStudentById(this.activeParameter).subscribe((response: Student) => {
      this.student = { ...response };
    });
  }

  getGrads() {
    this.gradService.getGraduates().subscribe((response: Graduates[]) => {
      this.graduates = response;
    });
  }

  getClasses() {
    this.classesService.getClasses().subscribe((response: ClassInfo[]) => {
      this.classes = response;
    });
  }

  getTeachers() {
    this.teacherService.getTeachers().subscribe((response: Teacher[]) => {
      this.teachers = response;
    });
  }

  ngOnDestroy() {
    if (this.emitter) {
      this.emitter.unsubscribe();
    }

    delete (this.student);
  }

  apply() {
    // if there's no student id add new student
    if (this.student && !this.student.id) {
      this.studentService.postStudent(this.student)
        .subscribe(
          res => {
            Swal.fire({
              title: 'Pridėta!',
              type: 'success',
              text: 'Mokinio informacija pridėta',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#3f7e2e'
            });

            this.router.navigate(['mokiniai']);
          },
          error => {
            Swal.fire({
              title: 'Pridėti nepavyko',
              type: 'error',
              text: 'Mokinio informacijos pridėti nepavyko',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#c70909'
            });
          });
    } else if (this.student && this.student.id) {
      // if student has id, update existing student
      this.studentService.updateStudent(this.student.id, this.student)
        .subscribe(
          res => {
            if (res) {
              Swal.fire({
                title: 'Atnaujinta',
                type: 'success',
                text: 'Mokinio informacija atnaujinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['mokiniai']);
            }
          },
          error => {
            Swal.fire({
              title: 'Atnaujinti nepavyko',
              type: 'error',
              text: 'Mokinio informacijos atnaujinti nepavyko',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#c70909'
            });
          });
    }
  }

  delete() {
    Swal.fire({
      title: 'Ištrinti mokinio duomenis?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.studentService.deleteStudent(this.student.id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Mokinio informacija ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['mokiniai']);
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Mokinio informacijos ištrinti nepavyko',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#c70909'
              });
            });
      }
    });
  }

  cancel() {
    this.location.back();
  }


  validate() { }


}
