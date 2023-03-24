using FirebaseAdmin.Auth;
using Lib.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.BusinessLogic.Implementations
{
    public class UserService : IUserService
    {
        protected readonly DataContext context;
        protected readonly FirebaseAuth firebaseAuth;

        public UserService(DataContext dataContext, FirebaseAuth firebaseAuth)
        {
            this.context = dataContext;
            this.firebaseAuth = firebaseAuth;
        }

        public async Task<User> Add(RegisterUserModel model)
        {
            var userRecord = new UserRecordArgs
            {
                DisplayName = model.Email,
                Email = model.Email,
                Password = model.Password
            };

            var firebaseUser = await firebaseAuth.CreateUserAsync(userRecord);

            var user = new User
            {
                FirebaseUid = firebaseUser.Uid,
                Firstname = model.Firstname,
                Lastname = model.Lastname
            };

            await context.Users.AddAsync(user);

            await firebaseAuth.SetCustomUserClaimsAsync(user.FirebaseUid, new Dictionary<string, object> { { "user_id", user.Id.ToString() } });

            await context.SaveChangesAsync();

            return user;
        }

        public User Get(Guid id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) throw new Exception("user not found");
            return user;
        }
    }
}
