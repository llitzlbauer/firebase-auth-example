import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { UsersService } from 'src/swagger';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {

  

  constructor(
    private usersService: UsersService,
    private authService: AuthService) {
    
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.usersService.usersIdGet('824C2BA4-80BB-44E5-67AF-08DB2CB35B58').subscribe(x => {
      console.log(x);
    });
  }
}
