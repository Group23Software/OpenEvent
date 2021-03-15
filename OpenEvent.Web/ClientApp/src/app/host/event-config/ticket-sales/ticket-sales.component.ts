import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {EventHostModel} from "../../../_models/Event";
import {BaseChartDirective, Color, Label} from "ng2-charts";
import {ChartDataSets, ChartOptions, ChartPoint, ChartType} from "chart.js";
import {MappedEventAnalytics} from "../../../_models/Analytic";
import * as pluginAnnotations from 'chartjs-plugin-annotation';

@Component({
  selector: 'ticket-sales[Event][Analytics]',
  templateUrl: './ticket-sales.component.html',
  styleUrls: ['./ticket-sales.component.css']
})
export class TicketSalesComponent implements OnInit
{

  @Input() Event: EventHostModel;
  @Input() Analytics: MappedEventAnalytics;

  public ticketSalesData: ChartDataSets[] = [
    {label: "Ticket Sales", data: [] as ChartPoint[]},
    {label: "Page Views", data: [] as ChartPoint[], yAxisID: 'y-axis-1'}
  ];

  ticketSalesLabels: Label[];

  chartColours: Color[] = [
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    },
    { // red
      backgroundColor: 'rgba(255,0,0,0.3)',
      borderColor: 'red',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];

  chartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      xAxes: [{
        type: 'time',
        display: true,
        ticks: {
          major: {
            enabled: true
          }
        }
      }],
      yAxes: [{
        id: 'y-axis-0',
        position: 'left',
      },
        {
          id: 'y-axis-1',
          position: 'right',
          gridLines: {
            color: this.chartColours[1].backgroundColor,
          },
          ticks: {
            fontColor: this.chartColours[1].borderColor,
          }
        }
      ]
    },
    annotation: {
      annotations: [
        {
          type: 'line',
          mode: 'vertical',
          scaleID: 'x-axis-0',
          value: new Date().toDateString(),
          borderColor: 'black',
          borderWidth: 2,
          label: {
            enabled: true,
            fontColor: 'white',
            content: 'Today'
          }
        },
        {
          type: 'line',
          mode: 'vertical',
          scaleID: 'x-axis-0',
          value: '',
          borderColor: '#4caf50',
          borderWidth: 2,
          label: {
            enabled: true,
            fontColor: 'white',
            content: 'Start'
          }
        }
      ]
    }
  };

  chartType: ChartType = 'line';
  chartPlugins = [pluginAnnotations];

  public loading: boolean = true;

  @ViewChild(BaseChartDirective, {static: true}) chart: BaseChartDirective;

  constructor ()
  {
  }

  ngOnInit (): void
  {
    let dates: Map<string, number> = new Map<string, number>();
    let pageViewDates: Map<string, number> = new Map<string, number>();

    let startDate = new Date(this.Event.Created);
    let endDate = new Date(this.Event.EndLocal);

    this.chartOptions.annotation.annotations[1].value = new Date(this.Event.StartLocal).toDateString();

    console.log(this.chartOptions);

    for (let d = new Date(startDate.toDateString()); d <= new Date(endDate.toDateString()); d.setDate(d.getDate() + 1))
    {
      dates.set(d.toDateString(), 0);
      pageViewDates.set(d.toDateString(), 0);
    }

    this.Event.Transactions.forEach(t =>
    {
      if (t.Paid)
      {
        let end = new Date(t.End)
        let fullDate: string = end.toDateString();
        dates.set(fullDate, dates.get(fullDate) + 1);
      }
    });

    console.log(this.Analytics.PageViewEvents);
    this.Analytics.PageViewEvents.forEach(p =>
    {
      // let date = new Date(p.Date);
      // let fullDate: string = new Date(date.getFullYear(), date.getMonth(), date.getDay()).toDateString();
      pageViewDates.set(p.Date.toDateString(), pageViewDates.get(p.Date.toDateString()) + p.PageViews.length);
    });

    let ticketSalesPoints: ChartPoint[] = [];
    let pageViewPoints: ChartPoint[] = [];

    dates.forEach((total, date) =>
    {
      ticketSalesPoints.push({x: date, y: total} as ChartPoint);
    });

    pageViewDates.forEach((total, date) =>
    {
      pageViewPoints.push({x: date, y: total});
    });

    console.log(pageViewDates);

    this.ticketSalesData[0].data = ticketSalesPoints;
    this.ticketSalesData[1].data = pageViewPoints;
    this.loading = false;
  }

  chartHovered ($event: { event: MouseEvent; active: {}[] })
  {

  }

  chartClicked ($event: { event?: MouseEvent; active?: {}[] })
  {

  }
}
