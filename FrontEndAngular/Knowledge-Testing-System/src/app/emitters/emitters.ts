import { EventEmitter } from "@angular/core";

export class Emitters{
    static authEmitter = new EventEmitter<boolean>();
    static resultEmitter = new EventEmitter<any>();
    static testEmitter = new EventEmitter<any>();
    static answerEmitter = new EventEmitter<any>();
    static adminRoleEmitter = new EventEmitter<boolean>();
}