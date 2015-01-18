using Jwt.Dao.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Dao.EntityFramework.Implementation
{
    public class DbContextFactory : IDbContextFactory
    {
        public TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            return Activator.CreateInstance <TDbContext>();
        }
    }
}
