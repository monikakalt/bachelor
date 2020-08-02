import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { DatePipe, Location } from '@angular/common';
import { Graduates } from 'app/models/graduates';
import { GraduatesService } from 'app/services/graduates.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-graduates',
  templateUrl: './add-graduates.component.html',
  styleUrls: ['./add-graduates.component.scss']
})
export class AddGraduatesComponent implements OnInit, OnDestroy {
  @Input()
  graduate: Graduates;

  activeParameter: any;

  @Output()
  change: EventEmitter<string> = new EventEmitter<string>();

  emitter: EventEmitter<Graduates>;
// tslint:disable-next-line: max-line-length
  constructor(private location: Location, private activateRoute: ActivatedRoute, private gradService: GraduatesService, private datePipe: DatePipe,
              private router: Router) {
  }

  ngOnInit() {
    this.activateRoute.params.subscribe((params: Params) => {
      this.activeParameter = params.id;
    });

    if (this.activeParameter) {
      this.getGraduate();
    } else {
      this.graduate = new Graduates();
    }
  }

  ngOnDestroy() {
    if (this.emitter) {
      this.emitter.unsubscribe();
    }

    delete (this.graduate);
  }

  getGraduate() {
    this.gradService.getGraduateById(this.activeParameter).subscribe((response: Graduates) => {
      this.graduate = { ...response };
    });
  }

  apply() {
    // if there's no graduate id add new event
    if (this.graduate && !this.graduate.id) {
      this.graduate.year = +this.graduate.year;
      this.gradService.postGraduates(this.graduate)
      .subscribe(
        res => {
          Swal.fire({
            title: 'Pridėta!',
            type: 'success',
            text: 'Laidos informacija pridėta',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#3f7e2e'
          });

          this.router.navigate(['laidos']);
        },
        error => {
          Swal.fire({
            title: 'Pridėti nepavyko',
            type: 'error',
            text: 'Laidos informacijos pridėti nepavyko',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#c70909'
          });
        });
    } else if (this.graduate && this.graduate.id) {
      this.graduate.year = +this.graduate.year;
      this.gradService.updateGraduates(this.graduate.id, this.graduate)
      .subscribe(
        (res) => {
          if (res) {
            Swal.fire({
              title: 'Atnaujinta',
              type: 'success',
              text: 'Laidos informacija atnaujinta',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#3f7e2e'
            });

            this.router.navigate(['laidos']);
          }
        },
        error => {
          Swal.fire({
            title: 'Atnaujinti nepavyko',
            type: 'error',
            text: 'Laidos informacijos atnaujinti nepavyko',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#c70909'
          });
        });
    }
  }

  delete() {
    Swal.fire({
      title: 'Ištrinti laidą?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.gradService.deleteGraduates(this.graduate.id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Laidos informacija ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['laidos']);
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Laidos informacijos ištrinti nepavyko',
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
