import { Component, OnInit, Input, Output, EventEmitter, OnDestroy, OnChanges } from '@angular/core';
import { EventsService } from 'app/services/events.service';
import Swal from 'sweetalert2';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import { EventInfo } from 'app/models/event';
import { UserService } from 'app/services/user.service';
import { User } from 'app/models/user';

@Component({
  selector: 'app-add-event',
  templateUrl: './add-event.component.html',
  styleUrls: ['./add-event.component.scss']
})
export class AddEventComponent implements OnInit, OnDestroy {
  @Input()
  event: EventInfo;
  usrId: any;
  activeParameter: any;

  @Output()
  change: EventEmitter<string> = new EventEmitter<string>();

  emitter: EventEmitter<EventInfo>;

  isValid = false;
  user: any;

  canEdit = false;

  retrievedUser: User;

  constructor(private location: Location, private activateRoute: ActivatedRoute,
              private eventService: EventsService,
              private userService: UserService,
              private router: Router) {
  }

  ngOnInit() {
    this.user = JSON.parse(localStorage.getItem('currentUser'));
    this.usrId = this.user.id;

    this.activateRoute.params.subscribe((params: Params) => {
      this.activeParameter = params.id;
    });

    if (this.activeParameter) {
      this.getEventInfo();
    } else {
      this.event = new EventInfo();
      this.getUserById(this.usrId);
    }
  }

  ngOnDestroy() {
    if (this.emitter) {
      this.emitter.unsubscribe();
    }

    delete (this.event);
  }

  getEventInfo() {
    this.eventService.getEventById(this.activeParameter).subscribe((response: EventInfo) => {
      this.event = { ...response };
      this.getUserById(this.event.fkUser);
    });
  }

  getUserById(id: number) {
    this.userService.getUserById(id).subscribe((response: User) => {
      this.retrievedUser = { ...response };
      if ((this.usrId === this.retrievedUser.id && this.event.start && Date.parse(this.event.start.toString()) > Date.now())
       || !this.event.start) {
        this.canEdit = true;
      }
    });
  }

  apply() {
    // if there's no event id add new event
    if (this.event && !this.event.id) {
      this.event.fkUser = this.usrId;
      this.eventService.postEvent(this.event)
      .subscribe(
        res => {
          Swal.fire({
            title: 'Pridėta!',
            type: 'success',
            text: 'Rezervacija sėkmingai pridėta',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#3f7e2e'
          });

          this.router.navigate(['kalendorius']);
        },
        error => {
          Swal.fire({
            title: 'Pridėti nepavyko',
            type: 'error',
            text: 'Rezervacijos pridėti nepavyko',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#c70909'
          });
        });
    } else if (this.event && this.event.id) {
      // if event has id, update existing student
      this.event.fkUser = this.usrId;
      this.eventService.updateEvent(this.event.id, this.event)
      .subscribe(
        (res) => {
          if (res) {
            Swal.fire({
              title: 'Atnaujinta',
              type: 'success',
              text: 'Rezervacija sėkmingai atnaujinta',
              confirmButtonText: 'Gerai',
              confirmButtonColor: '#3f7e2e'
            });

            this.router.navigate(['kalendorius']);
          }
        },
        error => {
          Swal.fire({
            title: 'Atnaujinti nepavyko',
            type: 'error',
            text: 'Rezervacijos atnaujinti nepavyko',
            confirmButtonText: 'Gerai',
            confirmButtonColor: '#c70909'
          });
        });
    }
  }

  delete() {
    Swal.fire({
      title: 'Ištrinti rezervaciją?',
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#c70909',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Patvirtinti',
      cancelButtonText: 'Atšaukti'
    }).then((result) => {
      if (result.value) {
        this.eventService.deleteEvent(this.event.id)
          .subscribe(
            res => {
              Swal.fire({
                title: 'Ištrinta!',
                type: 'success',
                text: 'Rezervacija sėkmingai ištrinta',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#3f7e2e'
              });

              this.router.navigate(['kalendorius']);
            },
            error => {
              Swal.fire({
                title: 'Ištrinti nepavyko',
                type: 'error',
                text: 'Rezervacijos ištrinti nepavyko',
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

  validate() {
    this.isValid = !!this.event.title && !!this.event.email && !!this.event.start && !!this.event.end;
    if (this.event.start && this.event.end) {
      if (this.event.end < this.event.start) {
        this.isValid = false;
      }
    }
    return this.isValid;
  }

}
