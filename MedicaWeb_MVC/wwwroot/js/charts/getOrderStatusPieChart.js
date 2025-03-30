let pieChart;

// Initialize Pie Chart
function initPieChart(data) {
  const canvas = document.getElementById("ordersStatusPieChart");
  const ctx = canvas.getContext("2d");

  // Calculate total once at the beginning
  const total = data.series.reduce((a, b) => a + b, 0);

  // Destroy existing chart if it exists
  if (pieChart) {
    pieChart.destroy();
  }

  // Create gradient for Paid Orders
  const paidGradient = ctx.createLinearGradient(0, 0, 0, 350);
  paidGradient.addColorStop(0, "#00E396");
  paidGradient.addColorStop(1, "#00B8D9");

  // Create gradient for Failed Orders
  const failedGradient = ctx.createLinearGradient(0, 0, 0, 350);
  failedGradient.addColorStop(0, "#FF4560");
  failedGradient.addColorStop(1, "#FF6B6B");

  pieChart = new Chart(canvas, {
    type: "doughnut",
    data: {
      labels: data.labels,
      datasets: [
        {
          data: data.series,
          backgroundColor: [paidGradient, failedGradient],
          borderColor: ["#fff", "#fff"],
          borderWidth: 3,
          hoverOffset: 8,
          hoverBorderWidth: 4,
          hoverBorderColor: "#fff",
          hoverBackgroundColor: [paidGradient, failedGradient],
          spacing: 3,
          borderRadius: 8,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: true,
      cutout: "75%",
      plugins: {
        legend: {
          position: "bottom",
          align: "center",
          labels: {
            usePointStyle: true,
            pointStyle: "circle",
            padding: 20,
            font: {
              size: 13,
              weight: "600",
            },
            color: "#666",
            generateLabels: function (chart) {
              const data = chart.data;
              if (data.labels.length && data.datasets.length) {
                return data.labels.map((label, i) => {
                  const value = data.datasets[0].data[i];
                  const total = data.datasets[0].data.reduce((a, b) => a + b, 0);
                  const percentage = ((value / total) * 100).toFixed(1);
                  return {
                    text: `${label}: ${value} (${percentage}%)`,
                    fillStyle: data.datasets[0].backgroundColor[i],
                    strokeStyle: data.datasets[0].borderColor[i],
                    lineWidth: 2,
                    hidden: false,
                    index: i,
                  };
                });
              }
              return [];
            },
          },
        },
        tooltip: {
          backgroundColor: "rgba(0, 0, 0, 0.8)",
          padding: 16,
          titleColor: "#fff",
          titleFont: {
            size: 14,
            weight: "bold",
          },
          bodyColor: "#fff",
          bodyFont: {
            size: 13,
          },
          cornerRadius: 6,
          displayColors: false,
          callbacks: {
            label: function (context) {
              const value = context.parsed;
              const total = context.dataset.data.reduce((a, b) => a + b, 0);
              const percentage = ((value / total) * 100).toFixed(1);
              return `${context.label}: ${value} orders (${percentage}%)`;
            },
          },
        },
      },
      animation: {
        duration: 1500,
        easing: "easeInOutQuart",
        animateScale: true,
        animateRotate: true,
        delay: function (context) {
          return context.dataIndex * 100;
        },
      },
      layout: {
        padding: {
          top: 20,
          bottom: 20,
        },
      },
    },
    plugins: [
      {
        id: "centerText",
        afterDraw: function (chart) {
          const ctx = chart.ctx;
          const total = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);
          const centerX = chart.width / 2;
          const centerY = chart.height / 2;

          // Draw center text
          ctx.save();
          ctx.textAlign = "center";
          ctx.textBaseline = "middle";

          // Draw total orders with gradient color
          const numberGradient = ctx.createLinearGradient(0, centerY - 30, 0, centerY);
          numberGradient.addColorStop(0, "#00E396"); // Paid order color
          numberGradient.addColorStop(1, "#008FFB"); // Blue gradient
          ctx.font = "bold 28px Arial";
          ctx.fillStyle = numberGradient;
          ctx.fillText(total, centerX, centerY - 15);

          // Draw "Total Orders" text with different color
          ctx.font = "14px Arial";
          ctx.fillStyle = "#666";
          ctx.fillText("Total Orders", centerX, centerY + 15);

          ctx.restore();
        },
      },
    ],
  });

  // Update summary text with animations
  const totalElement = document.getElementById("spanTotalOrdersCount");
  const summaryElement = document.getElementById("orderStatusSummary");

  // Animate total count
  let currentTotal = 0;
  const duration = 1500;
  const steps = 60;
  const increment = total / steps;
  const interval = duration / steps;

  const totalInterval = setInterval(() => {
    currentTotal += increment;
    if (currentTotal >= total) {
      currentTotal = total;
      clearInterval(totalInterval);
    }
    totalElement.textContent = Math.round(currentTotal);
  }, interval);

  // Update summary with percentage
  const paidPercentage = ((data.series[0] / total) * 100).toFixed(1);
  const failedPercentage = ((data.series[1] / total) * 100).toFixed(1);
  summaryElement.textContent = `${data.series[0]} Paid (${paidPercentage}%) · ${data.series[1]} Failed (${failedPercentage}%)`;
}

// Function to update chart data
function updatePieChart(data) {
  if (pieChart) {
    pieChart.data.labels = data.labels;
    pieChart.data.datasets[0].data = data.series;
    pieChart.update("none"); // Update without animation
  } else {
    initPieChart(data);
  }
}

// Export functions for use in other files
window.initOrderStatusPieChart = function (data) {
  initPieChart(data);
};

window.updateOrderStatusPieChart = function (data) {
  updatePieChart(data);
};
