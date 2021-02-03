import {EventEmitter, Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TriggerService
{

  public isDark: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor ()
  {
  }
}
