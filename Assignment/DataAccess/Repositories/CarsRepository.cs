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
    public class CarsRepository : ICarsRepository
    {
        private readonly ApplicationDbContext _context; 
        public CarsRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public void DeleteCar(int id)
        {
            _context.Cars.Remove(GetCar(id));
            _context.SaveChanges();
        }

        public Car GetCar(int id)
        {
            return _context.Cars.SingleOrDefault(x => x.RegistrationPlate == id);
        }

        public IQueryable<Car> GetCars()
        {
            return _context.Cars;
        }

        public IdentityUser GetDriver(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email);
        }

        public int InsertCar(Car c)
        {
            IQueryable<Car> savedCars = GetCars();
            c.RegistrationPlate = savedCars.Count() + 1;
            _context.Cars.Add(c);
            _context.SaveChanges();

            return c.RegistrationPlate;
        }
    }
}
