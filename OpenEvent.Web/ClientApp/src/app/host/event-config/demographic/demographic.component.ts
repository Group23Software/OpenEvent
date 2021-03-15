import {Component, Input, OnInit} from '@angular/core';
import {MappedEventAnalytics} from "../../../_models/Analytic";
import {ChartOptions, ChartType} from "chart.js";
import {Color, Label} from "ng2-charts";
// import * as pluginDataLabels from 'chartjs-plugin-datalabels';

@Component({
  selector: 'demographic[Analytics]',
  templateUrl: './demographic.component.html',
  styleUrls: ['./demographic.component.css']
})
export class DemographicComponent implements OnInit
{

  @Input() Analytics: MappedEventAnalytics;
  public demographicData: number[] = [];
  public demographicLabels: Label[] = [];
  public chartType: ChartType = 'pie';
  public demographicOptions: ChartOptions = {
    responsive: true,
    legend: {
      position: 'top',
    },
  };
  // demographicPlugins = [pluginDataLabels];

  constructor ()
  {
  }

  ngOnInit (): void
  {
    this.Analytics.Demographics.forEach(d => {
      this.demographicData.push(d.Count);
      this.demographicLabels.push(d.Age.toString());
    })
  }

}
