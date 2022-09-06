using Core.YardSale.Contracts;
using Core.YardSale.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.YardSale
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseRepository _databaseRepository;
        public UserRepository(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }
        public User GetUserByUsername(string username)
        {
            DataTable dt = _databaseRepository.GetDT("usp_User_Get_By_Username", new List<object> { username });

            User user = new()
            {
                UserId = Convert.ToInt32(dt.Rows[0]["user_id"]),
                UserFirstName = dt.Rows[0]["user_first_name"].ToString() ?? "",
                UserLastName = dt.Rows[0]["user_last_name"].ToString() ?? "",
                UserEmail = dt.Rows[0]["user_email"].ToString() ?? "",
                UserUserName = dt.Rows[0]["user_username"].ToString() ?? "",
                UserPassword = dt.Rows[0]["user_password"].ToString() ?? ""
            };

            return user;
        }

        public int RegisterNewUser(User user)
        {
            int retVal = _databaseRepository.GetRetVal("usp_User_Register_New", new List<object> { user.UserFirstName, user.UserFirstName, user.UserEmail, user.UserUserName, user.UserPassword });
            return retVal;
        }

        
    }
}
