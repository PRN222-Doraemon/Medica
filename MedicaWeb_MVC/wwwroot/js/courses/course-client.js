var connection = new signalR.HubConnectionBuilder()
    .withUrl("/MedicaHubs")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveUpsert", function (course, isUpdate) {
    if (isUpdate) {
        updateCourseCard(course)
    }
    else {
        appendCourseCard(course);
    }
    console.log("success");
});

connection.start().then(function () {
    console.log("connect to SignalR Hub")
}).catch(function (err) {
    return console.error(err.toString())
})

function appendCourseCard(course) {
    console.log("add course");
    let cardHtml = `
        <div class="col">
            <div class="card h-100 shadow-sm">
                <img src="${course.imgUrl}" class="card-img-top" alt="Course Image"
                     style="height: 200px; object-fit: cover;">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-start mb-2">
                        <h5 class="card-title mb-0">${course.name}</h5>
                        <span class="badge rounded-pill ${course.status === "Active" ? "bg-success" : "bg-secondary"}">
                            ${course.status}
                        </span>
                    </div>
                    <p class="card-text text-muted small mb-2">
                        Category: <span class="fw-medium">${course.categoryName}</span>
                    </p>
                    <p class="card-text text-muted small mb-2">
                        Upload by <span class="fw-medium">${course.createdByUserName}</span>
                    </p>
                    <div class="d-flex align-items-center gap-2 text-muted small">
                        <i class="bi bi-clock"></i>
                        <span>Updated ${course.updatedAt}</span>
                    </div>
                    <span class="fw-bold text-dark fs-5">
                        ₫ ${Number(course.price).toLocaleString()}
                    </span>
                </div>
                <div class="card-footer bg-white border-top-0">
                    <a href="/Courses/Details/${course.id}" class="btn btn-outline-primary btn-sm w-100">
                        View Details
                    </a>
                </div>
            </div>
        </div>
    `;

    $("#courseList").append(cardHtml);
}
function updateCourseCard(course) {
    let card = $("#" + course.id);
    if (card.length) {
        card.find(".card-img-top").attr("src", course.imgUrl);
        card.find(".card-title").text(course.name);
        card.find(".badge")
            .text(course.status)
            .removeClass("bg-success bg-secondary")
            .addClass(course.status === "Active" ? "bg-success" : "bg-secondary");
        card.find(".card-text:contains('Category') span").text(course.categoryName);
        card.find(".card-text:contains('Upload by') span").text(course.createdByUserName);
        card.find(".bi-clock").next().text("Updated " + course.updatedAt);
        card.find(".fw-bold.text-dark").html("₫ " + new Intl.NumberFormat().format(course.price));
        card.find(".btn").attr("href", `/Courses/Details/${course.id}`);
    } else {
        console.warn("Course card not found: #" + course.id);
    }
}

