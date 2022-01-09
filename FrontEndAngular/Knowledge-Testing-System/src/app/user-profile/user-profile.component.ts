import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Emitters } from '../emitters/emitters';
import { User } from '../models/user';
import { UserProfile } from '../models/user-profile';
import { UserProfileTest } from '../models/user-profile-tests';
import { Constants } from '../static-files/constants';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  user: User;
  userProfile: UserProfile;
  userProfileTest: UserProfileTest

  constructor(
    private http:HttpClient
  ) { }

  ngOnInit(): void {
    this.http.get(Constants.APIPath + 'User/me', {withCredentials: true}).subscribe(
      (res: any) => {
        this.user = res.user;
        this.userProfile = res.userProfile;
        this.userProfileTest = res.userProfileTest;
        Emitters.authEmitter.emit(true);
      },
      (err: any) => {
        Emitters.authEmitter.emit(false);
      }
    )
  }

}
