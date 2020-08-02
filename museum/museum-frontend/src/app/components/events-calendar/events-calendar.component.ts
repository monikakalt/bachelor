import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { CalendarComponent } from 'ng-fullcalendar';
import { Options } from 'fullcalendar';
import { EventInfo } from 'app/models/event';
import { EventsService } from 'app/services/events.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { UserService } from 'app/services/user.service';
import { User } from 'app/models/user';
import * as moment from 'moment';

@Component({
  selector: 'app-events-calendar',
  templateUrl: './events-calendar.component.html'
})
export class EventsCalendarComponent implements OnInit {

  events$: EventInfo[];
  calendarOptions: Options;
  userId: number;

  @Input()
  event: EventInfo;

  retrievedUser: User;
  eventColor: string;

  allDaySlot = false;

  @ViewChild(CalendarComponent)
  ucCalendar: CalendarComponent;

  constructor(private eventService: EventsService, private router: Router, private userService: UserService) { }
  ngOnInit() {

    if (localStorage.getItem('currentUser')) {
      const local = JSON.parse(localStorage.getItem('currentUser'));
      this.userId = local.id;
      this.getUserById(this.userId);
    }

    this.initializeCalendar();
    this.getEvents();
  }

  getEvents() {
    this.eventService.getEvents().subscribe((res: EventInfo[]) => {
      this.events$ = res;
      this.events$.filter(x => x.fkUser === this.userId).map( (d) => d.color = '#28a745' );
      this.events$.filter(x => x.fkUser !== this.userId).map( (d) => d.color = '#F7A707' );

      this.events$.filter(x => x.fkUser === this.userId).map( (d) => d.editable = true );
      this.events$.filter(x => x.fkUser !== this.userId).map( (d) => d.editable = false );
      this.calendarOptions.events = res;
      if (res) {
        this.ucCalendar.renderEvents(res);
      }
    });
  }

  getUserById(id: number) {
    this.userService.getUserById(id).subscribe((response: User) => {
      this.retrievedUser = { ...response };
      if (this.userId === this.retrievedUser.id) {
        this.eventColor = 'green';
      }
    });
  }

  initializeCalendar() {
    const date = new Date();
    const dateConstraint = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
    this.calendarOptions = {
      height: 555,
      editable: true,
      eventLimit: false,
      timeFormat: 'H:mm',
      displayEventEnd: true,
      allDaySlot: false,
      slotLabelFormat: 'H:mm',
      monthNames: ['Sausis', 'Vasaris', 'Kovas', 'Balandis', 'Gegužė', 'Birželis',
        'Liepa', 'Rugpjūtis', 'Rugsėjis', 'Spalis', 'Lapkritis', 'Gruodis'],
      dayNames: ['Sekmadienis', 'Pirmadienis', 'Antradienis', 'Trečiadienis',
        'Ketvirtadienis', 'Penktadienis', 'Šeštadienis'],
      locale: 'lt',
      buttonText: {
        today: 'Šiandien',
        month: 'Mėnuo',
        week: 'Savaitė',
        day: 'Diena'
      },
      header: {
        left: 'prev,next today, reserve',
        center: 'title',
        right: 'month,agendaWeek,agendaDay'
      },
      hiddenDays: [0, 6],
      minTime: moment.duration('08:00:00'),
      maxTime: moment.duration('18:30:00'),
      scrollTime: moment.duration('08:00:00'),
      slotDuration: moment.duration('00:30:00'),
      slotLabelInterval: moment.duration('00:30:00'),
      aspectRatio: 1.55,
      eventConstraint: {
        start: dateConstraint,
        end: '2025-01-01' // hard coded goodness unfortunately
      },
      events: this.events$
    };
  }

  openEvent(event) {
    this.router.navigate(['rezervacija/' + event.event.id]);
  }

  add() {
    this.router.navigate(['nauja-rezervacija']);
  }

  updateEvent(event) {
    const eventModel = {
      id: event.event.id,
      title: event.event.title,
      start: event.event.start,
      end: event.event.end
    } as EventInfo;
    Swal.fire({
      title: 'Perkelti rezervaciją?',
      confirmButtonText: 'Gerai',
      confirmButtonColor: '#c70909',
      cancelButtonText: 'Atšaukti',
      showCancelButton: true,
    }).then((result) => {
      if (result.value) {
        this.eventService.updateEvent(eventModel.id, eventModel)
          .subscribe(
            res => {
              if (res) {
                Swal.fire({
                  title: 'Perkelta!',
                  type: 'success',
                  text: 'Rezervacija sėkmingai perkelta',
                  confirmButtonText: 'Gerai',
                  confirmButtonColor: '#3f7e2e',
                });

                this.getEvents();
              }
            },
            error => {
              Swal.fire({
                title: 'Perkelti nepavyko',
                type: 'error',
                text: 'Rezervacijos perkelti nepavyko',
                confirmButtonText: 'Gerai',
                confirmButtonColor: '#c70909'
              });
              this.ucCalendar.renderEvents(this.events$);
            });
      } else {
        this.ucCalendar.renderEvents(this.events$);
      }
    });
  }
}
