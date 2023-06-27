// ng g interceptor interceptors/jwt
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../services/account.service';
import { User } from '../models/identity/User';


@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  // la no app.module providers tem q colocar { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor }
  // sempre que fizer um request pra api, vai ser interceptado por aki e passar como parametro pra esse metodo
  // clonagem da requisiçao request e retornando a requisiçao next com a requisiçao request clonada, porem agr com adiçao de headers com autenticaçao
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User | null;

    //this.currentUserSource.next(user); la do accountService, isso quer dizer q vai retornar um user qndo der subscribe
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      currentUser = user;

      if (currentUser != null){
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`
            }
          }
        );
      }
    }
    );

    return next.handle(request);
  }
}
