using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Entities.Data;
using PlanningPoker.Entities.Models;
using PlanningPoker.Repository.Interface;
using PlannningPoker.Api.Hubs;

namespace PlannningPoker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase

    {
        private readonly IHubContext<ChatHub> _hubContext;

        public readonly Planning_pokerContext _context = new Planning_pokerContext();

        public readonly IUserRepository _userRepository;



        public UserController(IUserRepository userRepository, IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
            _userRepository = userRepository;
        }


        [HttpGet("CheckToken")]
        public async Task<OkObjectResult> CheckToken(string token)
        {
            // Check if the token exists in the database
            bool tokenExists = _context.Users.Any(user => user.GameToken == token);

            
                return Ok(tokenExists); // Token exists in the database
            
            
        }

        [HttpGet("GetToken")]
        public string Token()
        {
            var token = _userRepository.GenerateToken();
            return token;
        }

        [HttpPost("AddUser")]
        public async Task<OkObjectResult> AddUser(User user)
        {

            _userRepository.AddUser(user);
            var freshUser = _userRepository.GetUser();


            //await _hubContext.Clients.All.SendAsync("ReceiveData", user);

            return Ok(freshUser);
        }

        [HttpPost("AddCreator")]
        public async Task<OkObjectResult> AddCreator(User user)
        {
            _userRepository.AddUser(user);
            var freshUser = _userRepository.GetUser();

            //await _hubContext.Clients.All.SendAsync("ReceiveData", user);

            return Ok(freshUser);
        }



        [HttpPatch("UpdateUser")]
        public async Task<OkObjectResult> UpdateUser(User user)
        {
             _userRepository.UpdateUser(user);
            var freshUser =  _userRepository.GetUpdatedUser(user);

            return Ok(freshUser);

        }
        [HttpGet("UpdatePlayers")]
        public async Task<IActionResult> UpdatePlayers([FromBody] List<User> updatedPlayers)
        {
            updatedPlayers = _context.Users.ToList();

            // Invoke the UpdatePlayers method on the ChatHub to broadcast the updates
            await _hubContext.Clients.All.SendAsync("UpdatePlayers", updatedPlayers);

            return Ok();
        }

        [HttpPost("SendDataToClients")]
        public async Task<IActionResult> SendDataToClients([FromBody] List<User> data)
        {
            // Send the data to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveData", data);

            return Ok();
        }

        [HttpGet("GetPlayers")]
        public IEnumerable<User> GetPlayers(string gameToken)
         
        {
            Console.WriteLine("called get players");
            //var users = _userRepository.GetPlayers();
            var users = _context.Users.Where(user => user.GameToken == gameToken).ToList();
            return users;


        }

        [HttpGet("GetUser")]
        public User GetUser()
        {
            var user = _userRepository.GetUser();
            return user;
        }

        [HttpGet("GetGameCreator/{id}")]
        public User GetPlayer(int id)
        {
            var player = _context.Users.FirstOrDefault(p => p.Userid == id);

         

            return player;
        }




      
    }
}