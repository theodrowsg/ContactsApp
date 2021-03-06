import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelperService = new JwtHelperService();

  constructor(private authService: AuthService) {}

  ngOnInit() {
      this.authService.decodedToken = this.jwtHelperService.decodeToken(localStorage.getItem('token'));
      this.authService.profilePhotoUrl = localStorage.getItem('ppUrl');
      this.authService.changeMemberPhoto(this.authService.profilePhotoUrl);
  }
}
