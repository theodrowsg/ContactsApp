import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import {BehaviorSubject} from 'rxjs';
import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl +  'auth/';
  jwtHelper = new JwtHelperService();
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();
  decodedToken: any;
  profilePhotoUrl;
constructor(private http: HttpClient) { }

login(model: any) {
  return this.http.post(this.baseUrl + 'login', model)
  .pipe(
   map((response: any) => {
     const user = response;
     if (user) {
       localStorage.setItem('token', user.token);
       localStorage.setItem('ppUrl', user.ppUrl);
       this.decodedToken = this.jwtHelper.decodeToken(user.token);
       this.profilePhotoUrl = user.ppUrl;
       this.changeMemberPhoto(user.ppUrl);
     }

   })
  );

}

register(user: User) {
return this.http.post(this.baseUrl + 'register', user);
}

loggedIn() {
  return !this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
}

changeMemberPhoto(photoUrl: string) {
  this.photoUrl.next(photoUrl);
}

}
