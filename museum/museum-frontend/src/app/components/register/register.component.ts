import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'app/models/user';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap';
import { UserService } from 'app/services/user.service';
import { AlertService } from 'app/services/alert.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
declare var $: any;
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

  loading = false;
  registerForm: FormGroup;
  user: User = new User();
  private currentUserSubject: BehaviorSubject<User>;
  authenticated: boolean;

  @ViewChild('registerModal') public modal: ModalDirective;

  constructor(private formBuilder: FormBuilder,
              private userService: UserService,
              private alertService: AlertService,
              private router: Router) {

    this.registerForm = formBuilder.group({
      fullName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      password: [null, Validators.required],
      confirmPassword: [null, Validators.required],
    });
  }

  submitForm(value: any): void {
    this.user.fullName = value.fullName;
    this.user.password = value.password;
    this.user.email = value.email;

    this.userService.postUser(this.user).subscribe(res => {
      Swal.fire({
        title: 'Registracija sėkminga!',
        text: 'Galite prisijungti',
        type: 'success',
        confirmButtonText: 'Gerai',
        confirmButtonColor: '#3f7e2e'
      });
      $('#registerModal').modal('hide');
     // this.currentUserSubject.next(null);
      this.router.navigate(['']);
      this.registerForm.reset();
    },
      (err) => {
        this.alertService.error(err._body);
        this.loading = false;
      });
  }

  cancel() {
    this.registerForm.reset();
  }

}
