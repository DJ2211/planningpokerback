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

        [HttpGet("GetToken")]
        public string Token()
        {
            var token = _userRepository.GenerateToken();
            return token;
        }

        [HttpPost("AddUser")]
        public  async Task<OkObjectResult> AddUser(User user)
        {

            _userRepository.AddUser(user);
            var freshUser = _userRepository.GetUser();


            await _hubContext.Clients.All.SendAsync("ReceiveData", user);

            return Ok(freshUser);
        }

        [HttpPatch("UpdateUser")]
        public async Task UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);

            await _hubContext.Clients.All.SendAsync("UpdateUser", user);

            //return Ok();
            
        }
        [HttpPost("UpdatePlayers")]
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
        public IEnumerable<User> GetPlayers()
        {
            Console.WriteLine("called get players");
            var users = _userRepository.GetPlayers();
            return users;


        }

        [HttpGet("GetUser")]
        public User GetUser()
        {
            var user = _userRepository.GetUser();
            return user;
        }




        ////methods of hubs
        //[HttpGet("PerformAction")]
        //public IActionResult PerformAction()
        //{
         
        //    // Send a message to all clients
        //    _hubContext.Clients.All.SendAsync("ReceiveMessage", "Action performed!");

        //    return Ok();
        //}
    }
}