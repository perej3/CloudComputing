using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.DataAccess.Interfaces
{
    public interface ILogRepository
    {
        void Log(string message);
    }
}
