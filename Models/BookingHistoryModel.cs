using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantCopy.Models
{
    public class BookingHistoryModel
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingStartTime { get; set; }
        public TimeSpan BookingEndTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; }

        // New properties for formatted times
        public string FormattedBookingStartTime { get; set; }
        public string FormattedBookingEndTime { get; set; }
    }
}