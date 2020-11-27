import { Component, EventEmitter, OnInit, Output } from '@angular/core';
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
    private authService: AuthService
  ) { }

  ngOnInit(){
  }

  register() {
    this.authService.register(this.model).subscribe( () => {
      console.log('registered')
    }, error => {
      console.log(error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log("canceled")
  }

}
