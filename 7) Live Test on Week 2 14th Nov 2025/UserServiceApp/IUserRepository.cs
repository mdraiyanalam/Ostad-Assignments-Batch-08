using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceApp
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        bool SaveUser(User user);
    }
}
