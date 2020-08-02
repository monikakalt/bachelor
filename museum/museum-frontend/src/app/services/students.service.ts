import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Http } from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Student } from 'app/models/student';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  url = 'https://localhost:44328/api/';
  private studentsData: BehaviorSubject<Student[]>;
  private dataStore: {
    students: Student[]
  };

  constructor(private router: Router, private http: Http, private httpClient: HttpClient) {
    this.dataStore = { students: [] };
    this.studentsData = new BehaviorSubject([]) as BehaviorSubject<Student[]>;
   }

  get students() {
    return this.studentsData.asObservable();
  }

  private jwt() {
    const authToken = JSON.parse(localStorage.getItem('currentUser')).token;
    return new HttpHeaders({Authorization: 'Bearer ' + authToken});
  }

  public uploadFile(data: any) {
    const url = `https://localhost:44328/api/Students/upload`;
    return this.http.post(url, data);
  }

  public getStudentById(id: number) {
    // now returns an Observable of student
    return this.httpClient.get<Student>(this.url + 'students/' + id, { headers: this.jwt() });
  }

  public getStudents() {
    return this.httpClient.get(this.url + 'students', { headers: this.jwt() });
  }

  public postStudent(c: Student): Observable<Student> {
    return this.httpClient.post<Student>(this.url + 'students/', c, { headers: this.jwt() });
}

public updateStudent(id: number, c: Student): Observable<Student> {
    return this.httpClient.put<Student>(this.url + 'students/' + id, c, { headers: this.jwt() });
}

public deleteStudent(id: number): Observable<{}> {
    const url = `${this.url}students/${id}`;
    return this.httpClient.delete(url /*, httpOptions*/);
}
}
