import { Question } from "./question";

export interface Test {
    id: number;
    text: string;
    questions: Question[];
  }