using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSeller.Entities
{
    [Table("CarSellers")]
    public class CarSellerUser : MyEntityBase
    {
        [DisplayName("İsim"),
            StringLength(25)]
        public string Name { get; set; }

        [DisplayName("Soyad"),
            StringLength(25)]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),
            Required,
            StringLength(25)]
        public string Username { get; set; }

        [DisplayName("E-Posta"),
            Required,
            StringLength(70)]
        public string Email { get; set; }

        [DisplayName("Şifre"),
            Required,
            StringLength(25)]
        public string Password { get; set; }

        [StringLength(30)]
        public string ProfileImageFilename { get; set; }

        [DisplayName("Aktif")]
        public bool IsActive { get; set; }

        [DisplayName("Yönetici")]
        public bool IsAdmin { get; set; }

        [Required]
        public Guid ActivateGuid { get; set; }


        public virtual List<Car> Cars { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Like> Likes { get; set; }
    }
}