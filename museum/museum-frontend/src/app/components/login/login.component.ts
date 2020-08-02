import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user';
import { AuthenticationService } from '../../services/authentication.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertService } from '../../services/alert.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import Swal from 'sweetalert2';

declare var $: any;
@Component({
  // tslint:disable-next-line:component-selector
  selector: 'loginForm',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  authenticated: boolean;
  user: any;
  us: User;
  loading = false;
  returnUrl: string;
  logError = false;
  error;
  @ViewChild('loginModal') public modal: ModalDirective;

  constructor(private formBuilder: FormBuilder, private authenticationService: AuthenticationService,
              private router: Router,
              private alertService: AlertService
  ) {
    this.loginForm = formBuilder.group({
      email: [null, [Validators.required, Validators.email]],
      password: [null, Validators.required],
    });

  }

  ngOnInit() {
    if (localStorage.getItem('currentUser')) {
      this.authenticated = true;
      this.user = JSON.parse(localStorage.getItem('currentUser'));
    }
  }

  submitForm(value: any) {
    this.loading = true;
    this.authenticationService.login(value.email, value.password)
      .subscribe((data: any) => {
        if (data) {
          $('#loginModal').modal('hide');
          this.router.navigate(['/']);
          this.loginForm.reset();
        }
        },
        (err) => {
          this.alertService.error(err._body);
          this.logError = true;
          this.loading = false;
        });
  }

  public logout(): void {
    this.authenticationService.logout();
    this.router.navigate(['/']);

  }

  cancel() {
    this.loginForm.reset();
  }

}
