let chatConnection;
let currentUserId;
let adminId = 1;

function initializeChat(userId) {
    currentUserId = userId;
    console.log("Initializing chat for user:", userId);

    chatConnection = new signalR.HubConnectionBuilder()
        .withUrl("/ChatHub")
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000, 60000])
        .configureLogging(signalR.LogLevel.Information)
        .build();

    chatConnection.on("ReceiveMessage", function (chatRoom) {
        console.log("Received message:", chatRoom);
        updateChatMessages(chatRoom);
    });

    chatConnection
        .start()
        .then(() => {
            console.log("Connected to chat hub successfully");
        })
        .catch((err) => {
            console.error("Error connecting to chat hub:", err);
        });
}

function toggleChat() {
    const popup = document.getElementById("chatPopup");
    popup.style.display = popup.style.display === "none" ? "flex" : "none";
}

function sendMessage() {
    if (!chatConnection || chatConnection.state !== signalR.HubConnectionState.Connected) {
        console.error("Not connected to chat hub. Connection state:", chatConnection?.state);
        return;
    }

    const messageInput = document.getElementById("messageInput");
    const message = messageInput.value.trim();

    if (message) {
        console.log("Sending message:", message, "from user:", currentUserId, "to admin:", adminId);
        chatConnection
            .invoke("SendMessage", currentUserId, adminId, message)
            .then(() => {
                console.log("Message sent successfully");
                messageInput.value = "";
            })
            .catch((err) => {
                console.error("Error sending message:", err);
            });
    }
}

function updateChatMessages(chatRoom) {
    const messagesDiv = document.getElementById("chatMessages");
    messagesDiv.innerHTML = "";

    chatRoom.messages.forEach((message) => {
        const messageDiv = document.createElement("div");
        messageDiv.className = `message ${message.senderId === currentUserId ? "sent" : "received"}`;
        messageDiv.textContent = message.message;
        messagesDiv.appendChild(messageDiv);
    });

    messagesDiv.scrollTop = messagesDiv.scrollHeight;
}

document.addEventListener("DOMContentLoaded", function () {
    const messageInput = document.getElementById("messageInput");
    if (messageInput) {
        messageInput.addEventListener("keypress", function (e) {
            if (e.key === "Enter") {
                sendMessage();
            }
        });
    }
});
