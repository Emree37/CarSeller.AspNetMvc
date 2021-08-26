using CarSeller.BusinessLayer.Abstract;
using CarSeller.DataAccessLayer.EntityFramework;
using CarSeller.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSeller.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            CarManager carManager = new CarManager();
            LikeManager likeManager = new LikeManager();
            CommentManager commentManager = new CommentManager();

            // Kategori ile ilişkili notların silinmesi gerekiyor.
            foreach (Car car in category.Cars.ToList())
            {

                // Note ile ilişikili like'ların silinmesi.
                foreach (Like like in car.Likes.ToList())
                {
                    likeManager.Delete(like);
                }

                // Note ile ilişkili comment'lerin silinmesi
                foreach (Comment comment in car.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }

                carManager.Delete(car);
            }

            return base.Delete(category);
        }
    }
}
