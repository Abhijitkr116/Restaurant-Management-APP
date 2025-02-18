using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantCopy.Models
{
    public class FoodItemModel
    {
        public int? Id { get; set; }
        public string ItemName { get; set; }
        public int? Price { get; set; }
        public int? FoodTypeId { get; set; }
        public string FoodType { get; set; }
    }
}