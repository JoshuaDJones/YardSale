using Core.YardSale.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Contracts
{
    public interface IUserRepository
    {
        public User GetUserByUsername(string username);
        public int RegisterNewUser(User user);
    }
}
