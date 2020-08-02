import Swal from 'sweetalert2';
import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { ClassInfo } from 'app/models/classInfo';
import { Location } from '@angular/common';

import { ClassInfoService } from 'app/services/classes.service';
import { TeachersService } from 'app/services/teachers.service';
import { Teacher } from 'app/models/teacher';
@Component({
  selector: 'app-add-class',
  templateUrl: './add-class.component.html',
  styleUrls: ['./add-class.component.scss']
})
export class AddClassComponent implements OnInit, OnDestroy {

  @Input()
  classInfo: ClassInfo;

  teachers: Teacher[];
  @Output()
  change: EventEmitter<string> = new EventEmitter<string>();

  emitter: EventEmitter<ClassInfo>;

  activeParameter: any;
  constructor(private location: Location, private activateRoute: ActivatedRoute, private classesService: ClassInfoService,
              private router: Router, private teacherService: TeachersService) {
                this.getTeachers();
}

  ngOnInit() {
    this.activateRoute.params.subscribe((params: Params) => {
      this.activeParameter = params.id;
    });

    if (this.activeParameter) {
      this.getClass();
    } else {
      this.classInfo = new ClassInfo();
    }
  }

  ngOnDestroy() {
    if (this.emitter) {
      this.emitter.unsubscribe();
    }

    delete (this.classInfo);
  }

  getClass() {
    this.classesService.getClassById(this.activeParameter).subscribe((response: ClassInfo) => {
      this.classInfo = { ...response };
    });
  }

  getTeachers() {
    this.teacherService.getTeachers().subscribe((response: Teacher[]) => {
      this.teachers = response;
    });
  }

  apply() {
    // if there's no graduate id add new event
    if (this.classInfo && !this.classInfo.id) {
      this.classesService.postClass(this.classInfo)
      .subscribe(
        res => {
          Swal.fire({
            title: 'Pridėta!',
            type: 'success',
            text: 'Klasė pridėta',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#3f7e2e'
          });

          this.router.navigate(['klases']);
        },
        error => {
          Swal.fire({
            title: 'Pridėti nepavyko',
            type: 'error',
            text: 'Klasės pridėti nepavyko',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#c70909'
          });
        });
    } else if (this.classInfo && this.classInfo.id) {
      this.classesService.updateClass(this.classInfo.id, this.classInfo)
      .subscribe(
        (res) => {
          if (res) {
            Swal.fire({
              title: 'Atnaujinta',
              type: 'success',
              text: 'Klasės informacija atnaujinta',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#3f7e2e'
            });

            this.router.navigate(['klases']);
          }
        },
        error => {
          Swal.fire({
            title: 'Atnaujinti nepavyko',
            type: 'error',
            text: 'Klasės informacijos atnaujinti nepavyko',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#c70909'
          });
        });
    }
  }

  delete() {
    Swal.fire({
      title: 'Ištrinti klasę?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.classesService.deleteClass(this.classInfo.id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Klasės informacija ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['klases']);
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Klasės informacijos ištrinti nepavyko',
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
