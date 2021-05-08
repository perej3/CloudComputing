using Assignment.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Interfaces
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetBookings();
        int InsertBooking(Booking b);

        IdentityUser GetPassenger(string email);
    }
}
