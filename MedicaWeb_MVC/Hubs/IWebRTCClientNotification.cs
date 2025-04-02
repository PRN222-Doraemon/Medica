namespace MedicaWeb_MVC.Hubs
{
    public interface IWebRTCClientNotification
    {
        /// <summary>
        /// Receives a WebRTC offer from a peer
        /// </summary>
        /// <param name="offer">The SDP offer</param>
        /// <param name="fromConnectionId">The connection ID of the sender</param>
        /// <param name="usernameOffer">The username of the sender</param>
        /// <param name="roleOffer">The role of the sender</param>
        Task ReceiveOffer(string offer, string fromConnectionId, string usernameOffer, string roleOffer);

        /// <summary>
        /// Receives a WebRTC answer from a peer
        /// </summary>
        /// <param name="answer">The SDP answer</param>
        /// <param name="fromConnectionId">The connection ID of the sender</param>
        Task ReceiveAnswer(string answer, string fromConnectionId);

        /// <summary>
        /// Notifies when a user joins the WebRTC session
        /// </summary>
        /// <param name="connectionId">The connection ID of the user</param>
        /// <param name="username">The username of the user</param>
        Task UserJoined(string connectionId, string username, string userRole);

        /// <summary>
        /// Notifies when a user leaves the WebRTC session
        /// </summary>
        /// <param name="connectionId">The connection ID of the user</param>
        Task UserLeft(string connectionId, string username);

        /// <summary>
        /// Receives an ICE candidate from a peer
        /// </summary>
        /// <param name="iceCandidate">The ICE candidate</param>
        /// <param name="connectionId">The connection ID of the sender</param>
        Task ReceiveIceCandidate(string iceCandidate, string connectionId);

        /// <summary>
        /// Notifies when a room does not exist
        /// </summary>
        /// <param name="roomId">The ID of the room</param>
        Task RoomDoesNotExist(string roomId);
    }
}