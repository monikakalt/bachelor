import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Http} from '@angular/http';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chronicle } from 'app/models/chronicle';

@Injectable({
  providedIn: 'root'
})
export class ChronicleService {

  url = 'https://localhost:44328/api/';

  constructor(private router: Router, private http: Http, private httpClient: HttpClient) {
   }

  private jwt() {
    if (JSON.parse(localStorage.getItem('currentUser'))) {
      const authToken = JSON.parse(localStorage.getItem('currentUser')).token;
      return new HttpHeaders({ Authorization: 'Bearer ' + authToken });
    }
  }

  public uploadFile(data: any) {
    const url = `https://localhost:44328/api/Chronicles/upload`;
    return this.http.post(url, data);
}

  public getChronicleById(id: number) {
    return this.httpClient.get<Chronicle>(this.url + 'Chronicles/' + id, { headers: this.jwt() });
  }

  public getRecentChronicle() {
    return this.httpClient.get<Chronicle>(this.url + 'Chronicles/recent', { headers: this.jwt() });
  }

  public getChronicles() {
    return this.httpClient.get(this.url + 'Chronicles');
  }

  public postChronicle(c: Chronicle): Observable<Chronicle> {
    return this.httpClient.post<Chronicle>(this.url + 'Chronicles/', c, { headers: this.jwt() });
}

public updateChronicle(id: number, c: any): Observable<Chronicle> {
    return this.httpClient.put<Chronicle>(`https://localhost:44328/api/Chronicles/` + id, c, { headers: this.jwt() });
}

public deleteChronicle(id: number): Observable<{}> {
    const url = `${this.url}chronicles/${id}`;
    return this.httpClient.delete(url, { headers: this.jwt() });
}
}
