using Moodify.Models;
using Moodify.Data;

namespace Moodify.Services
{
    public class SecurityService
    {
        private readonly UserRepository _userRepository;

        public SecurityService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool IsValid(UserModel userModel)
        {
            var user = _userRepository.GetUserByUsernameAndPassword(userModel.UserName, userModel.Password);
            return user != null;
        }
    }
}