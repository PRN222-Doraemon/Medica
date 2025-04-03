using Core.Constants;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace MedicaWeb_MVC.Hubs;

class UserData
{
    public string Username { get; set; }
    public string Role { get; set; }    
}

public class WebRTCHub(
    ILogger<WebRTCHub> _logger,
    IAccountService _accountService
    ) : Hub<IWebRTCClientNotification>
{
    // ConcurrentDictionary is used here because:
    // 1. Multiple users can join/leave rooms simultaneously from different threads
    // 2. It provides thread-safe operations without explicit locking
    // 3. It handles concurrent access patterns common in SignalR hubs
    // 4. Outer dictionary: roomId -> room participants (connectionId -> username)
    // 5. Inner dictionary: connectionId -> username
    private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, UserData>> _rooms = new();


    /// <summary>
    /// Adds a user to a WebRTC room and notifies other participants
    /// </summary>
    /// <param name="roomId">The ID of the room to join</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task JoinRoom(string roomId)
    {
        var connectionId = Context.ConnectionId;
        var user = await _accountService.GetUserByClaimsAsync(Context.User);
        var username = user.UserName ?? "Anonymous";
        var role = Context.User.IsInRole(AppCts.Roles.Lecturer) ? "Lecturer" : "Student";
        _logger.LogInformation("Adding user {username} ({connectionId}) to group {RoomId}", username, connectionId, roomId);

        // var isRoomExists = await _mediator.Send(new CheckIsRoomExistsQuery() { RoomId = roomId });

        // if (!isRoomExists)
        // {
        //     _logger.LogInformation("Room {RoomId} does not exist", roomId);
        //     await Clients.Caller.RoomDoesNotExist(roomId);
        //     return;
        // }

        await Groups.AddToGroupAsync(connectionId, roomId);

        var room = _rooms.GetOrAdd(roomId, _ => new ConcurrentDictionary<string, UserData>());
        room.TryAdd(connectionId, new UserData { Username = username, Role = role });

        //Notify other users in the room that a new user has joined
        await Clients.OthersInGroup(roomId).UserJoined(connectionId, username, role);
       

        //Notify the new user about other users in the room
        foreach (var users in room.Where(u => u.Key != connectionId))
        {
            await Clients.Caller.UserJoined(users.Key, users.Value.Username, users.Value.Role);
        }
    }

    /// <summary>
    /// Removes a user from a WebRTC room and notifies other participants
    /// </summary>
    /// <param name="roomId">The ID of the room to leave</param>
    /// <param name="username">The username of the user leaving the room</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task LeaveRoom(string roomId)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation("Removing user {connectionId} from group {RoomId}", connectionId, roomId);
        await Groups.RemoveFromGroupAsync(connectionId, roomId);

        var leftUser = _rooms.TryGetValue(roomId, out var room) ? room[connectionId].Username : "Anonymous";
        _logger.LogInformation("Removing user {connectionId} from room {RoomId}", connectionId, roomId);
        if (_rooms.TryGetValue(roomId, out room))
        {
            room.TryRemove(connectionId, out _);

            if (room.IsEmpty)
            {
                _rooms.TryRemove(roomId, out _);
            }
        }
        _logger.LogInformation("Notifying other users in room {RoomId} that user {connectionId} has left", roomId, connectionId);
        await Clients.OthersInGroup(roomId).UserLeft(connectionId, leftUser);
    }

    /// <summary>
    /// Sends a WebRTC offer to all other users in a room
    /// </summary>
    /// <param name="roomId">The ID of the room</param>
    /// <param name="offer">The SDP offer to send</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task SendOffer(string roomId, string offer, string usernameOffer, string roleOffer)
    {
        _logger.LogInformation("{fromConnectionId} sending offer to {RoomId}", Context.ConnectionId, roomId);

        var connectionId = Context.ConnectionId;

        await Clients.OthersInGroup(roomId).ReceiveOffer(offer, connectionId, usernameOffer, roleOffer);
    }

    /// <summary>
    /// Sends a WebRTC answer to a specific user
    /// </summary>
    /// <param name="toConnectionId">The connection ID of the recipient</param>
    /// <param name="answer">The SDP answer to send</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task SendAnswer(string toConnectionId, string answer)
    {
        _logger.LogInformation("{fromId} sending answer to {ConnectionId}", Context.ConnectionId, toConnectionId);

        var connectionId = Context.ConnectionId;

        await Clients.Client(toConnectionId).ReceiveAnswer(answer, connectionId);
    }

    /// <summary>
    /// Sends an ICE candidate to a specific user
    /// </summary>
    /// <param name="toConnectionId">The connection ID of the recipient</param>
    /// <param name="iceCandidate">The ICE candidate to send</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task SendIceCandidate(string toConnectionId, string iceCandidate)
    {
        //_logger.LogInformation("Sending ice candidate to {ConnectionId}", toConnectionId);

        var connectionId = Context.ConnectionId;

        await Clients.Client(toConnectionId).ReceiveIceCandidate(iceCandidate, connectionId);
    }

    /// <summary>
    /// Handles client disconnection by removing the user from all rooms
    /// </summary>
    /// <param name="exception">The exception that caused the disconnection, if any</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        //_logger.LogInformation("User disconnected: {ConnectionId}", connectionId);

        foreach (var room in _rooms.Where(r => r.Value.ContainsKey(connectionId)))
        {
            await LeaveRoom(room.Key);
        }
        await base.OnDisconnectedAsync(exception);
    }
}