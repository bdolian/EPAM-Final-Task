import { Option } from "./option";

export interface Question {
    id: number;
    text: string;
    correctOptionId: number;
    testId: number;
    options: Option[];
  }