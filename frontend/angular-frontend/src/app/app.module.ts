import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/login/login.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { AdminComponent } from './pages/admin/admin.component';
import { OrdersComponent } from './pages/orders/orders.component';
import { initializeApp, provideFirebaseApp } from '@angular/fire/app';
import { environment } from '../environments/environment';
import { provideAuth, getAuth } from '@angular/fire/auth';
import { Configuration, UsersService } from 'src/swagger';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AngularFireAuth, AngularFireAuthModule } from '@angular/fire/compat/auth';
import { FormsModule } from '@angular/forms';
import { AuthService } from 'src/app/core/services/auth.service';
import { AngularFireModule } from '@angular/fire/compat';
import { map, take } from 'rxjs';

export function usersServiceFactory(httpClient: HttpClient, afAuth: AngularFireAuth) {
  const basePath = 'https://localhost:7050';
  return new UsersService(httpClient, basePath, new Configuration({withCredentials: true}));
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ProfileComponent,
    AdminComponent,
    OrdersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    AngularFireAuthModule,
    AngularFireModule.initializeApp(environment.firebase),
    // provideFirebaseApp(() => initializeApp(environment.firebase)),
    // provideAuth(() => getAuth())
  ],
  providers: [
    {
      provide: UsersService,
      useFactory: usersServiceFactory,
      deps: [HttpClient, AngularFireAuth]
    },
    AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
