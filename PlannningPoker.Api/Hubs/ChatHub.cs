using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Entities.Data;
using PlanningPoker.Entities.Models;
using PlanningPoker.Entities.ViewModel;
using PlanningPoker.Repository.Interface;
using PlanningPoker.Repository.Repository;
using System.Collections.Concurrent;

namespace PlannningPoker.Api.Hubs
{   
    public class ChatHub : Hub
    {
        private static List<User> players = new List<User>();
        private static ConcurrentDictionary<string, List<User>> gameGroups = new ConcurrentDictionary<string, List<User>>();

        private readonly IUserRepository _userRepository;

        public ChatHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task JoinGame(string gameToken)
        {
            Planning_pokerContext _db = new Planning_pokerContext();

            // Join the specified game room
            await Groups.AddToGroupAsync(Context.ConnectionId, gameToken);

            // Get the list of users in the game room
            List<User> users = _db.Users.Where(user => user.GameToken == gameToken).ToList();

            // Send the updated user list to all the clients in the game room
            await Clients.Group(gameToken).SendAsync("ReceiveMessage", users);

            // Send a message to all the clients in the game room about the new user joining
            //await Clients.Group(gameToken).SendAsync("ReceiveMessage", $"{Context.ConnectionId} joined the game.", users);
        }

        public async Task LeaveGame(string gameToken)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameToken);
        await Clients.Group(gameToken).SendAsync("ReceiveMessage", $"{Context.ConnectionId} left the game.");
    }

        //public async Task AddUser(string name, string gameToken)
        //{

        //    Planning_pokerContext _db = new Planning_pokerContext();

        //    // Create a new player object
        //    User newPlayer = new User();
            
        //        newPlayer.Name = name;
        //    _db.Add(newPlayer);
        //    _db.SaveChanges();

        //    List<User> gameGroup;
        //    if (gameGroups.TryGetValue(gameToken, out gameGroup))
        //    {
        //        gameGroup.Add(newPlayer);
        //    }
        //    else
        //    {
        //        gameGroup = new List<User> { newPlayer };
        //        gameGroups[gameToken] = gameGroup;
        //    }

        //    List<User> players = _db.Users.ToList();

        //    // Broadcast the updated player list to all clients in the game group
        //    await Clients.Group(gameToken).SendAsync("UpdatePlayers", gameGroup);

        //}

        

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public void SendMessageToClients(string message)
        {
            Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task UpdatePlayers(string gameToken)
        {


            //new code

            

            // Update the players list with the provided updated players for the specific game room
            Planning_pokerContext _db = new Planning_pokerContext();
            List<User> playersFromServer = _db.Users.Where(user => user.GameToken == gameToken).ToList();

            // Broadcast the updated player list to clients in the specific game room
            await Clients.Group(gameToken).SendAsync("sendPlayers", playersFromServer);



            
        }

        //public async Task BroadcastRevealedCards(List<RevealedCard> revealedCards)
        //{
        //    await Clients.All.SendAsync("ReceiveRevealedCards", revealedCards);
        //}

        public async Task BroadcastRevealedCards(string gameToken, List<RevealedCard> cardsArray)
        {
            // Broadcast the revealed card event to the specific game group
            await Clients.Group(gameToken).SendAsync("ReceiveRevealedCards", cardsArray);
        }

        //public async Task BroadcastRevealCardEvent(RevealCardPayload payload, string gameToken)
        //{
        //    await Clients.Group(gameToken).SendAsync("ReceiveRevealCardEvent", payload);
        //}

        public async Task ResetGameState( string gameToken)
        {

            Planning_pokerContext _db = new Planning_pokerContext();
            List<User> updatedUsers = _db.Users.Where(user => user.GameToken == gameToken).ToList();

            // Perform the necessary game state reset for the updatedUsers
            var usrs = _userRepository.UpdateUsersOnNewGame(updatedUsers);

            // Broadcast the updated game state to the clients in the specific game room
            await Clients.Group(gameToken).SendAsync("GameStateReset", usrs);



            //old code
            //var usrs = _userRepository.UpdateUsersOnNewGame(users);



            //await Clients.All.SendAsync("GameStateReset", usrs);
        }
    

}
}

