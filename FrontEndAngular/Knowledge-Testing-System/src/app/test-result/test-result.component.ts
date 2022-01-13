import { Component, OnInit } from '@angular/core';
import { Emitters } from '../emitters/emitters';
import { Result } from '../models/result';
import { Test } from '../models/test';

@Component({
  selector: 'app-test-result',
  templateUrl: './test-result.component.html',
  styleUrls: ['./test-result.component.css']
})
export class TestResultComponent implements OnInit {

  public Test: Test;
  public Result: Result;
  public Answer: Result;
  public isCorrect: number = 0; //Mark the non-chosen(0)\correct(1)\incorrect(2) answer
  constructor() { }

  ngOnInit(): void {

    Emitters.resultEmitter.subscribe((value: any) => {  //Get the value of result and passed test correct answers
      this.Result = JSON.parse(value);
    },
      (err) => {
        console.log(err);
      });
    Emitters.testEmitter.subscribe((value: any) => {  //Get the value of passed test
      this.Test = value;
    },
      (err) => {
        console.log(err);
      });
    Emitters.answerEmitter.subscribe((value: any) => { //Get the value of chosen answers for passed test
      this.Answer = value;
    },
      (err) => {
        console.log(err);
      });
  }

  markIncorrectAnswers(): void{
    this.Answer.questionAnswers.forEach(element => {
      document.getElementById("option" + element.answerId).setAttribute("style","background-color: red");
    });
  }

  markCorrectAnswers(): void {
    this.Result.questionAnswers.forEach(element => {
      document.getElementById("option" + element.answerId).setAttribute("style","background-color: green");
    });
  }
}
