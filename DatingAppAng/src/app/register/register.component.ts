import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AlertifyService } from '../shared/services/alertify.service';
import { AuthService } from '../shared/services/auth.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService
  ) { }

  ngOnInit(){
  }

  register() {
    this.authService.register(this.model).subscribe( () => {
      this.alertify.success('registration succesful');
    }, error => {
      console.log(error.statusText)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
