using System.ComponentModel.DataAnnotations;

namespace ASM2.Models
{
    public class Orders
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Qty")]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Please enter price")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please enter Phone Number")]
        [Display(Name = "Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter orderdate")]
        [Display(Name = "OrderDate")]
        public DateTime Orderdate { get; set; }
        public int BookID { get; set; }
        public int UserID { get; set; }
    }
}
