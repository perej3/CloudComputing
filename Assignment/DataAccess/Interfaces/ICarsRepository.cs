using Assignment.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Interfaces
{
    public interface ICarsRepository
    {
        IQueryable<Car> GetCars();

        Car GetCar(int id);

        int InsertCar(Car c);

        IdentityUser GetDriver(string email);

        void DeleteCar(int id);
    }
}
