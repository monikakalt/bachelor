import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Graduates } from 'app/models/graduates';

@Injectable()
export class GraduatesService {

    url = 'https://localhost:44328/api/';

    constructor(private httpClient: HttpClient) { }

    private jwt() {
        const authToken = JSON.parse(localStorage.getItem('currentUser')).token;
        return new HttpHeaders({ Authorization: 'Bearer ' + authToken });
    }

    public getGraduateById(id: number) {
        return this.httpClient.get<Graduates>(this.url + 'graduates/' + id, { headers: this.jwt() });
    }

    public getGraduates() {
        return this.httpClient.get(this.url + 'graduates');
    }

    public postGraduates(c: Graduates): Observable<Graduates> {
        return this.httpClient.post<Graduates>(this.url + 'graduates/', c, { headers: this.jwt() });
    }

    public updateGraduates(id: number, c: Graduates): Observable<Graduates> {
        return this.httpClient.put<Graduates>(this.url + 'graduates/' + id, c, { headers: this.jwt() })
    }

    public deleteGraduates(id: number): Observable<{}> {
        const url = `${this.url}graduates/${id}`;
        return this.httpClient.delete(url, { headers: this.jwt() });
    }

    handleError(arg0: string, c: Graduates): (err: any, caught: Observable<Graduates>) => never {
        throw new Error('Method not implemented.');
    }
}