using museum.Application.main.users.dto;
using NUnit.Framework;

namespace Tests.userTests
{
    [TestFixture]
    public class UsersTests: ApiBaseTest
    {
        private readonly string prefix = "users/";
        [Test]
        public void Register()
        {
            var user = new UserDTO
            {
                Email = "test@gmail.com",
                FullName = "test",
                Password = "test",
            };

            try
            {
               this.Post(this.url, user);
            }
            catch (System.Exception)
            {

                Assert.Fail("Posting user via API failed!");
                return;
            }
        }

        [Test]
        public void GetUserById()
        {
            this.Authenticate();
            try
            {
                var user = new UserDTO
                {
                    Email = "test@gmail.com",
                    FullName = "test",
                    Id = 1
                };
                var url = this.url + this.prefix + user.Id;
                var fetchedUser = this.Get<UserDTO>(url, true);
                Assert.NotNull(fetchedUser);
                Assert.AreEqual(user.Email, fetchedUser.Email);
                Assert.AreEqual(user.FullName, fetchedUser.FullName);
            }
            catch
            {
                Assert.Fail("Reading user via API failed!");
                return;
            }
        }

        [Test]
        public void Authenticate()
        {
            this.Authenticate();
            Assert.NotNull(this.authToken);

        }
    }
}
