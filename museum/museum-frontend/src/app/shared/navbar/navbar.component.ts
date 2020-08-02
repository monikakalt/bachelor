import { Component, OnInit, ViewChild, OnChanges } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { ModalDirective } from 'ngx-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit, OnChanges {

  @ViewChild('loginModal') public loginModal: ModalDirective;
  @ViewChild('registerModal') public rregisterModal: ModalDirective;

  user;
  isAdmin = false;
  constructor(private router: Router, private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    if (localStorage.getItem('currentUser')) {
      const localUser = JSON.parse(localStorage.getItem('currentUser'));
      if (localUser && localUser.id) {
        this.isAdmin = localUser.isAdmin;
      }
    }
  }

  ngOnChanges() {
    if (localStorage.getItem('currentUser')) {
      const localUser = JSON.parse(localStorage.getItem('currentUser'));
      if (localUser && localUser.id) {
        this.isAdmin = localUser.isAdmin;
      }
    }
  }

  public isAuthenticated() {
    // Check whether the current time is past the
    // Access Token's expiry time
    if (!localStorage.getItem('currentUser')) {
      return false;
    }
    this.user = JSON.parse(localStorage.getItem('currentUser'));
    if (this.user && this.user.id) {
      this.isAdmin = this.user.isAdmin;
    }
    return true;
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/']);
    this.user = null;
  }

}
