var connection = new signalR.HubConnectionBuilder()
    .withUrl("/MedicaHubs")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveUpsert", function () {
    console.log("success");
});

connection.start().then(function () {
    console.log("connect to SignalR Hub")
}).catch(function (err) {
    return console.error(err.toString())
})