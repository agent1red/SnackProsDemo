using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SnackPros.Models
{
    public class ShoppingCart
    {
        //Constructor
        public ShoppingCart()
        {
            //Assign Count = 1 for shopping cart 
            Count = 1;
        }
        public int Id { get; set; }

        public int MenuItemId { get; set; }
        [NotMapped]
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }

        public string ApplicationUserId { get; set; }
        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }



        [Range(1,100, ErrorMessage = "Please select a count between 1 and 100")]
        public int Count { get; set; }
    }
}
