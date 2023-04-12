using System.ComponentModel.DataAnnotations;


namespace OnlineShop.Models
{
    public class SpecialTag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string SpecialTagName { get; set; }
    }
}
