import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { getAuth } from '@firebase/auth';
import { FirebaseSignInProvider } from '@firebase/util';
import { Observable, tap } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticatedGuard implements CanActivate {
  
  constructor(
    private router: Router,
    private authService: AuthService
  ) {
  }
  
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if(this.authService.isLoggedIn) return true;
      return this.authService.isAuthenticated$.pipe(
        tap((isAuthenticated) => {
            if(!isAuthenticated) {
              this.router.navigate(['login']);
            }
        })
      );
  }
  
}
