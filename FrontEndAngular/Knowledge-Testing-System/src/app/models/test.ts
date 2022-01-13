import { Question } from "./question";

export class Test {
    id: number;
    name: string;
    numberOfQuestions: number;
    timeToEnd: number;
    questions: Question[];
  }