$(document).ready(function () {
    loadTotalCoursesRadialChart();
})

function loadTotalCoursesRadialChart() {

    // Show the spinner
    $(".chart-spinner").show();

    fetch('/Dashboard/GetTotalCoursesRadialChartData')
        .then(res => res.json())
        .then(data => {

            // Hide the spinner
            $(".chart-spinner").hide();

            // Total Count
            document.querySelector("#spanTotalCoursesCount").innerHTML = data.totalCount;

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

            document.querySelector("#sectionCoursesCurrentCount").append(sectionCurrentCount);
            document.querySelector("#sectionCoursesCurrentCount").append("since last month");

            // Radial Chart
            loadRadialChartData("totalCoursesRadialChart", "#059669", data);
        })
}

