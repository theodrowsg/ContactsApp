import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
    model: any = {};
    photoUrl: string;
  constructor(public authService: AuthService, private alertifyService: AlertifyService,
           private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
    this.alertifyService.success('Logged in successfully');
    }, error => {
      this.alertifyService.error('Failed to login');
    },
    () => this.router.navigate(['\members']));
  }

  loggedIn() {
    return this.authService.loggedIn();
  }
   logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('ppUrl');
    this.authService.decodedToken = null;
    this.authService.profilePhotoUrl = null;
    this.alertifyService.message('Thank you for visting');
    this.router.navigate(['\home']);
   }
}
