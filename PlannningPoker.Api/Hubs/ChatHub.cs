using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Entities.Data;
using PlanningPoker.Entities.Models;
using PlanningPoker.Entities.ViewModel;
using PlanningPoker.Repository.Interface;
using PlanningPoker.Repository.Repository;

namespace PlannningPoker.Api.Hubs
{   
    public class ChatHub : Hub
    {
        private static List<User> players = new List<User>();

        private readonly IUserRepository _userRepository;

        public ChatHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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

        public async Task UpdatePlayers(List<User> updatedPlayers)
        {
            // Update the players list with the provided updated players
            Planning_pokerContext _db = new Planning_pokerContext();
            List<User> playersFromServer = _db.Users.ToList();

            // Broadcast the updated player list to all clients
            await Clients.All.SendAsync("UpdatePlayers", playersFromServer);
        }

        public async Task BroadcastRevealedCards(List<RevealedCard> revealedCards)
        {
            await Clients.All.SendAsync("ReceiveRevealedCards", revealedCards);
        }

        
        public async Task BroadcastRevealCardEvent(RevealCardPayload payload)
        {
            await Clients.All.SendAsync("ReceiveRevealCardEvent", payload);
        }

        public async Task ResetGameState(List<User> users)
        {

            var usrs = _userRepository.UpdateUsersOnNewGame(users);

           

            await Clients.All.SendAsync("GameStateReset", usrs);
        }
    

}
}

