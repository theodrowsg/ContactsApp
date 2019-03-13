import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
   model: any = {};
   user: User;
   @Output() cancelRegister = new EventEmitter();
   registerForm: FormGroup;
   bsConfig: Partial<BsDatepickerConfig>;
  constructor(private authService: AuthService, private router: Router,
    private alertifyService: AlertifyService, private fb: FormBuilder) { }

  ngOnInit() {
    this.createRegisterForm();
    this.bsConfig = {
      containerClass: 'theme-red'
    };
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['male', Validators.required],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {validator: this.passwordMatchValidator});
  }

  register() {

    if (this.registerForm.valid) {
        this.user = Object.assign({}, this.registerForm.value);
        this.authService.register(this.user).subscribe(() => {
          this.alertifyService.success('Registration successful');
      },  error  => {
            this.alertifyService.error(error);
        }, () => {
              this.authService.login(this.user).subscribe(() => {
              this.router.navigate(['/members']);
     });
   });
  }
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
