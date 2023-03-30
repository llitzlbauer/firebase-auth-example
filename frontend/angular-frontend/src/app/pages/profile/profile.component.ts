import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/compat/auth';
import { AuthService } from 'src/app/core/services/auth.service';
import { Configuration, UsersService } from 'src/swagger';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {

  

  constructor(
    private auth: AngularFireAuth,
    private usersService: UsersService) {
    
  }

  async ngOnInit(): Promise<void> {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    
    this.auth.idTokenResult.subscribe(token => {
      if(token == null) console.log('error');
      this.usersService.configuration.apiKeys = {Authorization: `Bearer ${token!.token}`};
      this.usersService.usersIdGet(token!.claims['my_user_id']).subscribe(x => {
        console.log(x);
      });
    });   
  }
}
