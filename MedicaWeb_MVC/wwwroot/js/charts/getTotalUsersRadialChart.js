$(document).ready(function () {
    loadTotalUsersRadialChart();
})

function loadTotalUsersRadialChart() {

    // Show the spinner
    $(".chart-spinner").show();

    fetch('/Dashboard/GetRegisteredUserRadialChartData')
        .then(res => res.json())
        .then(data => {

            // Hide the spinner
            $(".chart-spinner").hide();

            // Total Count
            document.querySelector("#spanTotalUsersCount").innerHTML = data.totalCount;

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

            document.querySelector("#sectionUsersCurrentCount").append(sectionCurrentCount);
            document.querySelector("#sectionUsersCurrentCount").append("since last month");

            // Radial Chart
            loadRadialChartData("totalUsersRadialChart", "#059669", data);
        })
}

