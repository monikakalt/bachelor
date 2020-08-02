import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from 'app/models/user';
import { Observable } from 'rxjs';

@Injectable()
export class UserService {
  url = 'https://localhost:44328/api/';

  constructor(private http: Http, private httpClient: HttpClient) { }
  private jwt() {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const authToken = localStorage.getItem('currentUser');
    headers.append('Authorization', `Bearer ${authToken}`);
    return headers;
  }


  public getUserById(id: number) {
    // now returns an Observable of student
    return this.httpClient.get<User>(this.url + 'users/' + id, { headers: this.jwt()});
  }

  public getUsers() {
    return this.httpClient.get(this.url + 'users');
  }

  public postUser(u: User): Observable<User> {
    return this.httpClient.post<User>(this.url + 'users', u);
  }

  public updateUser(id: number, u: User): Observable<User> {
    return this.httpClient.put<User>(this.url + 'users/' + id, u /*httpOptions*/)
  }

  public deleteUser(id: number): Observable<{}> {
    const url = `${this.url}users/${id}`; // DELETE api/heroes/42
    return this.httpClient.delete(url /*, httpOptions*/)
  }

  handleError(arg0: string, u: User): (err: any, caught: Observable<User>) => never {
    throw new Error('Method not implemented.');
  }

}
