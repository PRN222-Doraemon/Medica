// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var courseConnection = new signalR.HubConnectionBuilder()
  .withUrl("/MedicaHubs")
  .configureLogging(signalR.LogLevel.Information)
  .build();

courseConnection.on("ReceiveUpsert", function (course, isUpdate) {
  if (isUpdate) {
    updateCourseCard(course);
  } else {
    appendCourseCard(course);
  }
  console.log("success");
});

courseConnection.start().catch(function (err) {
  return console.error(err.toString());
});
