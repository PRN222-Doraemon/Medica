"use strict";

// Build the connection
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/newsHub")
  .withAutomaticReconnect()
  .build();

// Start the connection
connection.start().catch((err) => console.error(err.toString()));

// Handle when news is updated
connection.on("NewsUpdated", function (newsId) {
  // Reload or update the news item
  console.log(`News updated: ${newsId}`);
  // Consider adding refresh functionality here
});

// Handle when new news is added
connection.on("NewsCreated", function (newsId, newsTitle) {
  console.log(`New news added: ${newsId} - ${newsTitle}`);
  // Could show a notification or refresh the list
});
