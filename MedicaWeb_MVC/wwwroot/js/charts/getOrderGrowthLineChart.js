let lineChart;
let currentDateRange = "30"; // Default to last 30 days

// Function to get date range parameters
function getDateRangeParams() {
  const today = new Date();
  let startDate = new Date();

  if (currentDateRange === "custom") {
    const startInput = document.getElementById("startDate");
    const endInput = document.getElementById("endDate");
    return {
      startDate: startInput.value,
      endDate: endInput.value,
    };
  } else {
    startDate.setDate(today.getDate() - parseInt(currentDateRange));
    return {
      startDate: startDate.toISOString().split("T")[0],
      endDate: today.toISOString().split("T")[0],
    };
  }
}

// Function to update chart with new date range
function updateChartWithDateRange() {
  toggleSpinner(true);
  const params = getDateRangeParams();

  fetch(`/Dashboard/GetOrderGrowthLineData?startDate=${params.startDate}&endDate=${params.endDate}`)
    .then((response) => response.json())
    .then((data) => {
      if (lineChart) {
        lineChart.data.labels = data.dates;
        lineChart.data.datasets[0].data = data.series[0].data;
        lineChart.update();
      } else {
        initLineChart(data);
      }
    })
    .finally(() => toggleSpinner(false));
}

// Initialize Line Chart
function initLineChart(data) {
  const ctx = document.getElementById("orderGrowthChart").getContext("2d");

  // Destroy existing chart if it exists
  if (lineChart) {
    lineChart.destroy();
  }

  // Prepare data
  const dates = data.dates;
  const orderData = data.series[0].data;

  // Create gradient background
  const gradient = ctx.createLinearGradient(0, 0, 0, 350);
  gradient.addColorStop(0, "rgba(0, 143, 251, 0.2)");
  gradient.addColorStop(1, "rgba(0, 143, 251, 0)");

  lineChart = new Chart(ctx, {
    type: "line",
    data: {
      labels: dates,
      datasets: [
        {
          label: "Orders",
          data: orderData,
          borderColor: "#008FFB",
          backgroundColor: gradient,
          borderWidth: 3,
          pointBackgroundColor: "#008FFB",
          pointBorderColor: "#fff",
          pointBorderWidth: 2,
          pointRadius: 4,
          pointHoverRadius: 8,
          pointHoverBackgroundColor: "#fff",
          pointHoverBorderColor: "#008FFB",
          pointHoverBorderWidth: 2,
          pointShadowOffsetX: 1,
          pointShadowOffsetY: 1,
          pointShadowBlur: 5,
          pointShadowColor: "rgba(0, 0, 0, 0.3)",
          fill: true,
          tension: 0.4,
          spanGaps: true,
          segment: {
            borderDash: (ctx) => (ctx.p1.parsed.x === ctx.p0.parsed.x ? [6, 6] : undefined),
          },
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: true,
      interaction: {
        mode: "index",
        intersect: false,
      },
      plugins: {
        legend: {
          position: "top",
          align: "end",
          labels: {
            usePointStyle: true,
            pointStyle: "circle",
            padding: 20,
            font: {
              size: 12,
              weight: "500",
            },
            color: "#666",
          },
        },
        tooltip: {
          backgroundColor: "rgba(0, 0, 0, 0.8)",
          padding: 12,
          titleColor: "#fff",
          titleFont: {
            size: 14,
            weight: "bold",
          },
          bodyColor: "#fff",
          bodyFont: {
            size: 13,
          },
          cornerRadius: 4,
          displayColors: false,
          callbacks: {
            label: function (context) {
              return `${context.dataset.label}: ${context.parsed.y} orders`;
            },
            title: function (context) {
              return context[0].label;
            },
          },
        },
        zoom: {
          zoom: {
            wheel: {
              enabled: true,
              modifierKey: "ctrl",
              speed: 0.1,
            },
            pinch: {
              enabled: true,
            },
            mode: "x",
          },
          pan: {
            enabled: true,
            mode: "x",
          },
        },
      },
      scales: {
        x: {
          grid: {
            display: false,
            drawBorder: false,
          },
          ticks: {
            maxRotation: 45,
            minRotation: 45,
            color: "#666",
            font: {
              size: 11,
            },
            padding: 10,
          },
          border: {
            display: false,
          },
        },
        y: {
          type: "linear",
          display: true,
          position: "left",
          beginAtZero: true,
          grid: {
            color: "rgba(0, 0, 0, 0.05)",
            drawBorder: false,
          },
          ticks: {
            font: {
              size: 12,
              weight: "500",
            },
            color: "#666",
            stepSize: 1,
            callback: function (value) {
              return Math.round(value) + " orders";
            },
          },
        },
      },
      animation: {
        duration: 1000,
        easing: "easeInOutQuart",
        delay: function (context) {
          return context.dataIndex * 50;
        },
      },
      layout: {
        padding: {
          top: 20,
          right: 20,
          bottom: 20,
          left: 20,
        },
      },
    },
  });

  // Add custom legend click handler
  lineChart.options.plugins.legend.onClick = function (e, legendItem, legend) {
    const index = legendItem.datasetIndex;
    const chart = legend.chart;
    const meta = chart.getDatasetMeta(index);

    // Toggle visibility
    meta.hidden = meta.hidden === null ? !chart.data.datasets[index].hidden : !meta.hidden;
    chart.update();
  };
}

// Initialize date range controls
function initDateRangeControls() {
  // Initialize date inputs with default values
  const today = new Date();
  const thirtyDaysAgo = new Date();
  thirtyDaysAgo.setDate(today.getDate() - 30);

  document.getElementById("startDate").value = thirtyDaysAgo.toISOString().split("T")[0];
  document.getElementById("endDate").value = today.toISOString().split("T")[0];

  // Add event listeners for date range controls
  document.getElementById("dateRangeSelect").addEventListener("change", function (e) {
    currentDateRange = e.target.value;
    const customDateRange = document.getElementById("customDateRange");

    if (currentDateRange === "custom") {
      customDateRange.classList.remove("d-none");
    } else {
      customDateRange.classList.add("d-none");
    }

    updateChartWithDateRange();
  });

  // Add event listeners for custom date inputs
  document.getElementById("startDate").addEventListener("change", function () {
    if (currentDateRange === "custom") {
      updateChartWithDateRange();
    }
  });

  document.getElementById("endDate").addEventListener("change", function () {
    if (currentDateRange === "custom") {
      updateChartWithDateRange();
    }
  });
}

// Export functions for use in other files
window.initOrderGrowthLineChart = function (data) {
  initLineChart(data);
};

window.updateOrderGrowthLineChart = function () {
  updateChartWithDateRange();
};

window.initOrderGrowthDateRangeControls = function () {
  initDateRangeControls();
};
