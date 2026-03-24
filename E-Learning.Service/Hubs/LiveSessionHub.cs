using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace E_Learning.Service.Hubs
{
    public class LiveSessionHub : Hub
    {
        
        public async Task JoinSession(string sessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        }

      
        public async Task LeaveSession(string sessionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        }

   
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}