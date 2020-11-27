import { Component, OnInit } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(response => {
      console.log("logged")
    }, error => {
      console.log("error")
    });
  }


  loggedIn() {
    const token = localStorage.getItem('token');
    return token;
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out')
  }
}
