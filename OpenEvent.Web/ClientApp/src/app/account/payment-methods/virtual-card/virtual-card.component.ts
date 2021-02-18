import {Component, Input, OnInit} from '@angular/core';
import {PaymentMethodViewModel} from "../../../_models/PaymentMethod";

@Component({
  selector: 'virtual-card',
  templateUrl: './virtual-card.component.html',
  styleUrls: ['./virtual-card.component.css']
})
export class VirtualCardComponent implements OnInit {

  @Input() Card: PaymentMethodViewModel;

  constructor() { }

  ngOnInit(): void {
  }

  delete ()
  {

  }

  makeDefault ()
  {

  }
}
