$(document).ready(function () {
    loadTotalFeedbacksRadialChart();
})

function loadTotalFeedbacksRadialChart() {

    // Show the spinner
    $(".chart-spinner").show();

    fetch('/Dashboard/GetTotalFeedbackRadialChartData')
        .then(res => res.json())
        .then(data => {

            // Hide the spinner
            $(".chart-spinner").hide();

            // Total Count
            document.querySelector("#spanTotalFeedbacksCount").innerHTML = data.totalCount;

            // Current Count
            var sectionCurrentCount = document.createElement("span")
            if (data.hasRatioIncreased) {
                sectionCurrentCount.className = "text-success me-1";
                sectionCurrentCount.innerHTML = `
                    <i class="bi bi-arrow-up-right-circle me-1"></i>
                    <span> ${data.countInCurrentMonth}</span>
                `
            } else {
                sectionCurrentCount.className = "text-danger me-1";
                sectionCurrentCount.innerHTML = `
                    <i class="bi bi-arrow-down-right-circle me-1"></i>
                    <span> ${data.countInCurrentMonth}</span>
                `
            }

            document.querySelector("#sectionFeedbacksCurrentCount").append(sectionCurrentCount);
            document.querySelector("#sectionFeedbacksCurrentCount").append("since last month");

            // Radial Chart
            loadRadialChartData("totalFeedbacksRadialChart", "#059669", data);
        })
}

