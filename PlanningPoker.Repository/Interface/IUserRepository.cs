using PlanningPoker.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningPoker.Repository.Interface
{
    public interface IUserRepository
    {
        public void AddUser(User user);

        public IEnumerable<User> GetPlayers();

        public User GetUser();

        public string GenerateToken();

        public void UpdateUser(User user);

        public List<User> UpdateUsersOnNewGame(List<User> users);



    }
}
