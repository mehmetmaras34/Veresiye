using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veresiye.Data;
using Veresiye.Model;

namespace Veresiye.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<User> userRepository;
        public UserService(IUnitOfWork unitOfWork,IRepository<User>userRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;

        }
        public IEnumerable<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public User Login(string userName, string password)
        {
            
            {
                userName = userName.ToLower();
                var user = userRepository.Get(x => x.UserName == userName && x.Password == password);
                return user;
             }
           
        }

        public RegisterStatus Register(User user)
        {
            user.UserName = user.UserName.ToLower();
            //validasyonlar
            if (string.IsNullOrEmpty(user.UserName))
            {
                return RegisterStatus.InvalidFields;
            }
            else  
            {
                var newUser = userRepository.Get(x => x.UserName == user.UserName);
                if (newUser != null)
                {
                    return RegisterStatus.UserAlreadyExists;
                }
            }
            
            userRepository.Insert(user);
            unitOfWork.SaveChanse();
            return RegisterStatus.Success;

        }

        bool IUserService.Login(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
    public interface IUserService
    {
        bool Login(string userName, string password);
        RegisterStatus Register(User user);
        IEnumerable<User> GetAll();
    }
    public enum RegisterStatus
    {
        Success=1,
        InvalidFields=2,
        UserAlreadyExists=3
    }
}
