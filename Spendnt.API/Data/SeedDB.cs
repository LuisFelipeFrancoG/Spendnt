
using Microsoft.EntityFrameworkCore;
using Spendnt.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spendnt.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;

        public SeedDB(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
           

        }

        private async Task SeedCategoriasAsync()
        {
           
        }

    }
}
