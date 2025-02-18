using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantCopy.Models
{
    public class BookingModel
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public int Duration { get; set; } // Duration in minutes
        public int NumberOfGuests { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}