import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ClassInfo } from 'app/models/classInfo';

@Injectable()
export class ClassInfoService {

  url = 'https://localhost:44328/api/';

  constructor(private httpClient: HttpClient) { }

  private jwt() {
    const authToken = JSON.parse(localStorage.getItem('currentUser')).token;
    return new HttpHeaders({ Authorization: 'Bearer ' + authToken });
  }


  public getClassById(id: number) {
    // now returns an Observable of student
    return this.httpClient.get<ClassInfo>(this.url + 'schoolclasses/' + id, { headers: this.jwt() });
  }

  public getClasses() {
    return this.httpClient.get(this.url + 'schoolclasses', { headers: this.jwt() });
  }

  public postClass(c: ClassInfo): Observable<ClassInfo> {
    // return this.httpClient.post<class>(this.url, class, httpOptions)
    return this.httpClient.post<ClassInfo>(this.url + 'schoolclasses/', c, { headers: this.jwt() });
  }

  public updateClass(id: number, c: ClassInfo): Observable<ClassInfo> {
    return this.httpClient.put<ClassInfo>(this.url + 'schoolclasses/' + id, c, { headers: this.jwt() });
  }

  public deleteClass(id: number): Observable<{}> {
    const url = `${this.url}schoolclasses/${id}`; // DELETE api/heroes/42
    return this.httpClient.delete(url, { headers: this.jwt() });
  }

  handleError(arg0: string, c: ClassInfo): (err: any, caught: Observable<ClassInfo>) => never {
    throw new Error('Method not implemented.');
  }
}