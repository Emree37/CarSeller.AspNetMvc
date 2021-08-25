using CarSeller.DataAccessLayer.EntityFramework;
using CarSeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSeller.BusinessLayer
{
    public class Test
    {
        private Repository<CarSellerUser> repo_user = new Repository<CarSellerUser>();
        private Repository<Category> repo_category = new Repository<Category>();
        
        public Test()
        {
            List<Category> categories = repo_category.List();
        }

        public void InsertTest()
        {
            
            int result = repo_user.Insert(new CarSellerUser()
            {
                Name = "aaa",
                Surname = "bbb",
                Email = "aaabbb@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "aaabbb",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "aaabbb"
            });
        }

        public void UpdateTest()
        {
            CarSellerUser user = repo_user.Find(x => x.Username == "aaabbb");
            if(user != null)
            {
                user.Username = "aaabbbupdate";
                repo_user.Update(user);
            }
        }

        public void DeleteTest()
        {
            CarSellerUser user = repo_user.Find(x => x.Username == "aaabbbupdate");
            if(user != null)
            {
                repo_user.Delete(user);
            }
        }
    }
}
