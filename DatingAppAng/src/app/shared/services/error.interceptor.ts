import { Injectable } from '@angular/core';
import { HttpInterceptor } from '@angular/common/http';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(
    req: import('@angular/common/http').HttpRequest<any>,
    next: import('@angular/common/http').HttpHandler
  ): import ('rxjs').Observable<import('@angular/common/http').HttpEvent<any>>{
    throw new Error("Method not implemented");

  }
}
