import {Component, Inject, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {EventHostModel} from "../../../../_models/Event";
import {PromoService} from "../../../../_Services/promo.service";
import {HttpErrorResponse} from "@angular/common/http";
import {TriggerService} from "../../../../_Services/trigger.service";
import {IteratorStatus} from "../../../../_extensions/iterator/iterator.component";

@Component({
  selector: 'create-promo',
  templateUrl: './create-promo.component.html',
  styleUrls: ['./create-promo.component.css']
})
export class CreatePromoComponent implements OnInit
{

  public createPromoForm: FormGroup = new FormGroup({
    discount: new FormControl(0, [Validators.required]),
    start: new FormControl('', [Validators.required]),
    end: new FormControl('', [Validators.required]),
    active: new FormControl(false,[Validators.required])
  });
  public defaultTime: number[];
  minDate: Date;

  public createError: string;
  public loading: boolean = false;

  constructor (@Inject(MAT_DIALOG_DATA) public data: { event: EventHostModel }, private promoService: PromoService, private dialogRef:MatDialogRef<CreatePromoComponent>, private trigger: TriggerService)
  {
  }

  ngOnInit (): void
  {
    this.minDate = new Date();
    this.defaultTime = [new Date().getHours() + 1, 0, 0];
  }

  public create ()
  {
    this.loading = true;
    this.promoService.Create({
      Active: this.createPromoForm.controls.active.value,
      Discount: this.createPromoForm.controls.discount.value,
      End: this.createPromoForm.controls.end.value,
      Start: this.createPromoForm.controls.start.value,
      EventId: this.data.event.Id
    }).subscribe(r => {
      this.trigger.Iterate("Created Promo",2000,IteratorStatus.good);
      this.dialogRef.close(r);
    }, (e: HttpErrorResponse) => this.createError = e.message, () => this.loading = false)
  }
}
