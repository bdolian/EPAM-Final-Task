import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Test } from '../models/test';
import { Constants } from '../static-files/constants';

@Component({
  selector: 'app-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.css']
})
export class TestsComponent implements OnInit {
  public id: number;
  public Test: Test;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) { 
    
  }

  ngOnInit(): void {
    this.id = parseInt(this.route.snapshot.paramMap.get('id'));

    this.http.get(Constants.APIPath + 'Test/getTestById', 
    {
      withCredentials: true, 
      params: new HttpParams().set("id", this.id.toString())
    })
    .subscribe(res => {
      console.log(res);
      this.Test = <Test>res[0];
      console.log(this.Test);
    })
  }

}
