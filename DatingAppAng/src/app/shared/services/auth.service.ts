import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = environment.url;

  constructor(
    private http: HttpClient
  ) {}

  login(model: any) {
    return this.http.post(this.baseUrl + 'auth/login', model).pipe(
      map((response: any) => {
        const user = response;
        if(user) {
          console.log(localStorage)
          localStorage.setItem('token', user.token);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'auth/register', model);
  }

}
