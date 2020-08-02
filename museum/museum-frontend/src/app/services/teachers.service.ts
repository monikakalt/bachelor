import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, ObservableInput } from 'rxjs';
import { Teacher } from 'app/models/teacher';

@Injectable()
export class TeachersService {

  url = 'https://localhost:44328/api/';
  newDataAdded = new EventEmitter<string>();

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      Authorization: 'my-auth-token'
    })
  };

  private jwt() {
    const authToken = JSON.parse(localStorage.getItem('currentUser')).token;
    return new HttpHeaders({Authorization: 'Bearer ' + authToken});
  }

  constructor(private httpClient: HttpClient) { }

  public getTeacherById(id: number) {
    // now returns an Observable of student
    return this.httpClient.get<Teacher>(this.url + 'teachers/' + id, { headers: this.jwt() });
  }

  public getTeachers() {
    return this.httpClient.get(this.url + 'teachers');
  }

  public postTeacher(teacher: Teacher): Observable<Teacher> {
    // return this.httpClient.post<Teacher>(this.url, teacher, httpOptions)
    return this.httpClient.post<Teacher>(this.url + 'teachers/', teacher, { headers: this.jwt() });
  }

  public updateTeacher(id: number, teacher: Teacher): Observable<Teacher> {
    return this.httpClient.put<Teacher>(this.url + 'teachers/' + id, teacher, { headers: this.jwt() });
  }

  public deleteTeacher(id: number): Observable<{}> {
    const url = `${this.url}teachers/${id}`; // DELETE api/heroes/42
    return this.httpClient.delete(url, { headers: this.jwt() });
  }

  handleError(arg0: string, teacher: Teacher): (err: any, caught: Observable<Teacher>) => never {
    throw new Error('Method not implemented.');
  }

}
