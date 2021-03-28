import {Component, Input} from "@angular/core";

@Component({
  selector: 'ngx-mat-datetime-picker',
  template: ''
})
export class MockDateTimePicker {
  @Input() stepHour: any;
  @Input() stepMinute: any;
  @Input() defaultTime: any;
  @Input() stepSecond: any;
}
