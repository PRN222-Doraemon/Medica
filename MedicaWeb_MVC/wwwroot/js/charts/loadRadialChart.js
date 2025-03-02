function loadRadialChartData(chartId, chartColorHex, data) {

    let color = data.series[0] >= 0 ? chartColorHex : "#ff4560";

    var options = {
        series: [Math.abs(data.series[0])],
        chart: {
            height: 180,
            width: 180,
            type: 'radialBar',
            sparkline: {
                enabled: true
            },
            offsetY: -10,
        },
        colors: [color],
        plotOptions: {
            radialBar: {
                dataLabels: {
                    value: {
                        offsetY: -10,
                        fontSize: "16px",
                        fontWeight: "bold",
                        color: "#333",
                        formatter: function (val) {
                            return data.series[0] <= 0 ? `-${val}%` : `+${val}%`;
                        }
                    },
                }
            },
        },
        labels: [""]
    };

    var chart = new ApexCharts(document.querySelector("#" + chartId), options);
    chart.render();
}
