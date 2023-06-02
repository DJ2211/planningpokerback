using PlanningPoker.Entities.Data;
using PlanningPoker.Entities.Models;
using PlanningPoker.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningPoker.Repository.Repository
{

    public class UserRepository : IUserRepository
    {
        
        public readonly Planning_pokerContext _context = new Planning_pokerContext();
        public UserRepository()
        {
        }

        public void AddUser(User user)
        {
            
            _context.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            var user1 = _context.Users.FirstOrDefault(userDB => userDB.Userid == user.Userid);
            user1.SelectedCard = user.SelectedCard;
            user1.IsCardSelected = user.IsCardSelected;
            _context.Update(user1);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetPlayers()
        {

            var users = _context.Users.ToList();
            return users;
        }

        public User GetUser()
        {
            var user = _context.Users.ToList().LastOrDefault();
            return user;
        }


        //to generate random token on every new game 
        public string GenerateToken()
        {
             Random random = new Random();
            var length = 32;
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] token = new char[length];
            

            for (int i = 0; i < length; i++)
            {
                token[i] = chars[random.Next(chars.Length)];
            }

            return new string(token);
        }
    }
}
