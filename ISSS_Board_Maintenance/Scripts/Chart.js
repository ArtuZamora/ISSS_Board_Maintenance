$(document).ready(function () {
    Highcharts.chart('pieCharts', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Code Hubs Analysis'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            data: [{
                name: 'Jan',
                y: 12,
            }, {
                name: 'Feb',
                y: 15
            }, {
                name: 'March',
                y: 25
            }, {
                name: 'April',
                y: 90
            }, {
                name: 'May',
                y: 45
            }, {
                name: 'June',
                y: 84.6
            }, {
                name: 'July',
                y: 70
            }, {
                name: 'August',
                y: 50
            }, {
                name: 'September',
                y: 60
            }]
        }]
    });
});