using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceApp
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public string GetUserEmail(int userId)
        {
            var user = _repository.GetUserById(userId);

            if (user == null)
                return "User not found";

            if (!user.IsActive)
                return "User is inactive";

            return user.Email;
        }
        public bool ActivateUser(int userId)
        {
            var user = _repository.GetUserById(userId);

            if (user == null)
                return false;

            user.IsActive = true;
            return _repository.SaveUser(user);
        }
    }
}
