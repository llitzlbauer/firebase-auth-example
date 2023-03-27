import { Injectable } from '@angular/core';
import * as auth from 'firebase/auth';
import { AngularFireAuth } from '@angular/fire/compat/auth';
import { Router } from '@angular/router';
import { BehaviorSubject, distinctUntilChanged, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private onAuthenticatedSubject = new BehaviorSubject<boolean>(false);

  constructor(
    public afAuth: AngularFireAuth,
    public router: Router
  ) {
    this.afAuth.authState.subscribe((profile) => {
      if (profile) {
        this.onAuthenticatedSubject.next(true);
      } else {
        this.onAuthenticatedSubject.next(false);
      }
    });
  }

  get accessToken(): Observable<string> {
    return this.afAuth.idToken.pipe(map(x => x ?? ''));
  }

  get isAuthenticated$(): Observable<boolean> {
    return this.onAuthenticatedSubject
      .asObservable()
      .pipe(distinctUntilChanged());
  }

  get isLoggedIn(): boolean {
    return this.onAuthenticatedSubject.value;
  }


  login(email: string, password: string) {
    this.afAuth.signInWithEmailAndPassword(email, password).then(x => this.router.navigate(['profile']));
  }

  signOut() {
    return this.afAuth.signOut().then(() => {
      this.onAuthenticatedSubject.next(false);
      this.router.navigate(['login']);
    });
  }
}
