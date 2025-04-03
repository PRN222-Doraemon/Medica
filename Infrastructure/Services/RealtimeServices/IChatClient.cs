namespace Infrastructure.Services.RealtimeServices
{
    /// <summary>
    /// Those class will be called my the clients
    /// </summary>
    public interface IChatClient
    {
        public Task ReceiveMessage(string sender, string message);
    }
}
