﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }

        public int Id { get; set; }
        public  string OrderNo { get; set; }
        [Required]
        public  string Name{ get; set; }
        [Required]
        [Display(Name="Phone No")]
        public  string PhoneNo{ get; set; }
        [Required]
        [EmailAddress]
        public  string Email { get; set; }
        [Required]
        public  string Address{ get; set; }
        public DateTime orderDate { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
