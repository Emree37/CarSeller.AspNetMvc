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
    [Table("Categories")]
    public class Category : MyEntityBase
    {
        [DisplayName("Kategori"),
            Required,
            StringLength(50)]
        public string Title { get; set; }

        [DisplayName("Açıklama"),
            StringLength(150)]
        public string Description { get; set; }

        public virtual List<Car> Cars { get; set; }

        public Category()
        {
            Cars = new List<Car>();
        }
        
    }
}