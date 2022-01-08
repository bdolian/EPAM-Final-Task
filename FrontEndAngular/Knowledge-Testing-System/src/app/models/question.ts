import { Option } from "./option";

export class Question {
    id: number;
    text: string;
    correctOptionId: number;
    testId: number;
    numberOfOptions: number;
    options: Option[];
  }