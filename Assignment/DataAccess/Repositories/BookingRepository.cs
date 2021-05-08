using Assignment.Data;
using Assignment.DataAccess.Interfaces;
using Assignment.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Booking> GetBookings()
        {
          return  _context.Bookings;
            
        }

        public IdentityUser GetPassenger(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }

        public int InsertBooking(Booking b)
        {
            
            _context.Bookings.Add(b);
            _context.SaveChanges();

            
            return b.BookingId;
        }
    }
}
