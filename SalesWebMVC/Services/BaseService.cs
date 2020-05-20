using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public abstract class BaseService<T> where T : class
    {
        private readonly SalesWebMVCContext _context;

        protected BaseService(SalesWebMVCContext context)
        {
            _context = context;
        }


    }
}
