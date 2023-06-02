using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Entities.Data;
using PlanningPoker.Entities.Models;

namespace PlannningPoker.Api.Hubs
{   
    public class ChatHub : Hub
    {
        private static List<User> players = new List<User>();

        public async Task AddUser(string name)
        {

            Planning_pokerContext _db = new Planning_pokerContext();

            // Create a new player object
            User newPlayer = new User();
            
                newPlayer.Name = name;
            _db.Add(newPlayer);
            _db.SaveChanges();

            players = _db.Users.ToList();

            // Broadcast the updated player list to all clients
            await Clients.All.SendAsync("UpdatePlayers", players);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public void SendMessageToClients(string message)
        {
            Clients.All.SendAsync("ReceiveMessage", message);
        }

    }
}

