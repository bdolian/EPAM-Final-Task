import { Component, OnInit } from '@angular/core';
import { Test } from '../models/test';

@Component({
  selector: 'app-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.css']
})
export class TestsComponent implements OnInit {

  idToSet = 1;
  test: Test = {
    id: this.idToSet,
    text: 'What pokemon are you?',
    questions: [{id:1, text: 'TestQuestion?', correctOptionId:2, testId:this.idToSet, 
    options: 
      [
        {id:1, text: 'Option 1', questionId: 1}, 
        {id:2, text: 'Correct option', questionId: 1}
      ]
    }]
  };

  constructor() { 
    
  }

  ngOnInit(): void {

  }

}
