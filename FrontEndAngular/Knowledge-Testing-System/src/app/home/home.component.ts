import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Emitters } from '../emitters/emitters';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  message = '';

  constructor(
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.http.get('https://localhost:44334/User/me', {withCredentials: true}).subscribe(
      (res: any) => {
        console.log(res);
        this.message = `Hi ${res[0].email}`;
        Emitters.authEmitter.emit(true);
      },
      err => {
        this.message = "You are not logged in";
        Emitters.authEmitter.emit(false);
      }
    )
  }

}
