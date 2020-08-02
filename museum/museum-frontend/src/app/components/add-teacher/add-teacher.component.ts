import { Component, OnInit, Input, EventEmitter, OnDestroy } from '@angular/core';
import { Teacher } from 'app/models/teacher';
import { TeachersService } from 'app/services/teachers.service';
import { ActivatedRoute, Router, Params } from '@angular/router';
import Swal from 'sweetalert2';
import { Location } from '@angular/common';

@Component({
  selector: 'app-add-teacher',
  templateUrl: './add-teacher.component.html',
  styleUrls: ['./add-teacher.component.scss']
})
export class AddTeacherComponent implements OnInit, OnDestroy {

  @Input()
  teacher: Teacher;

  emitter: EventEmitter<Teacher>;

  activeParameter: any;

  constructor(private teacherService: TeachersService, private activateRoute: ActivatedRoute,
    private location: Location, private router: Router) { }

  ngOnInit() {
    this.activateRoute.params.subscribe((params: Params) => {
      this.activeParameter = params.id;
    });

    if (this.activeParameter) {
      this.getTeacher();
    } else {
      this.teacher = new Teacher();
    }
  }

  ngOnDestroy() {
    if (this.emitter) {
      this.emitter.unsubscribe();
    }

    delete (this.teacher);
  }


  getTeacher() {
    this.teacherService.getTeacherById(this.activeParameter).subscribe((response: Teacher) => {
      this.teacher = { ...response };
    });
  }

  apply() {
    // if there's no teacher id add new teacher
    if (this.teacher && !this.teacher.id) {
      this.teacherService.postTeacher(this.teacher)
        .subscribe(
          res => {
            Swal.fire({
              title: 'Pridėta!',
              type: 'success',
              text: 'Mokytojo informacija pridėta',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#3f7e2e'
            });

            this.router.navigate(['mokytojai']);
          },
          error => {
            Swal.fire({
              title: 'Pridėti nepavyko',
              type: 'error',
              text: 'Mokytojo informacijos pridėti nepavyko',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#c70909'
            });
          });
    } else if (this.teacher && this.teacher.id) {
      // if teacher has id, update existing teacher
      this.teacherService.updateTeacher(this.teacher.id, this.teacher)
        .subscribe(
          res => {
            if (res) {
              Swal.fire({
                title: 'Atnaujinta',
                type: 'success',
                text: 'Mokytojo informacija atnaujinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['mokytojai']);
            }
          },
          error => {
            Swal.fire({
              title: 'Atnaujinti nepavyko',
              type: 'error',
              text: 'Mokytojo informacijos atnaujinti nepavyko',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#c70909'
            });
          });
    }
  }

  delete() {
    Swal.fire({
      title: 'Ištrinti mokytojo duomenis?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.teacherService.deleteTeacher(this.teacher.id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Mokytojo informacija ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['mokytojai']);
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Mokytojo informacijos ištrinti nepavyko',
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

}
