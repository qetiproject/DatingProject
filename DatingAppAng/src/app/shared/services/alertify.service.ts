import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

  confirm(messgae: string, okCallBack: () => any) {
    alertify.confirm(messgae, (e: any) => {
      if(e) {
        okCallBack();
      } else {}
    })
  }

  success(message: string) {
    alertify.success(message);
  }

  error(message: string) {
    alertify.success(message);
  }

  warning(message: string) {
    alertify.success(message);
  }

  message(message: string) {
    alertify.success(message);
  }

}
