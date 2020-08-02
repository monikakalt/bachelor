import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EventInfo } from 'app/models/event';

@Injectable()
export class EventsService {

    url = 'https://localhost:44328/api/';

    constructor(private httpClient: HttpClient) { }

    private jwt() {
        const authToken = JSON.parse(localStorage.getItem('currentUser')).token;
        return new HttpHeaders({Authorization: 'Bearer ' + authToken});
    }

    public getEventById(id: number) {
        // now returns an Observable of student
        return this.httpClient.get<EventInfo>(this.url + 'events/' + id, { headers: this.jwt() });
    }

    public getEvents() {
        return this.httpClient.get(this.url + 'events', { headers: this.jwt() });
    }

    public postEvent(c: EventInfo): Observable<EventInfo> {
        return this.httpClient.post<EventInfo>(this.url + 'events/', c, { headers: this.jwt() });
    }

    public updateEvent(id: number, c: EventInfo): Observable<EventInfo> {
        return this.httpClient.put<EventInfo>(this.url + 'events/' + id, c, { headers: this.jwt() });
    }

    public deleteEvent(id: number): Observable<{}> {
        const url = `${this.url}events/${id}`;
        return this.httpClient.delete(url, { headers: this.jwt() });
    }

    handleError(arg0: string, c: EventInfo): (err: any, caught: Observable<EventInfo>) => never {
        throw new Error('Method not implemented.');
    }
}
