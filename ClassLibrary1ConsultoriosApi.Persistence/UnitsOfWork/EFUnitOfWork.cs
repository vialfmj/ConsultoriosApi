using ConsultoriosApi.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1ConsultoriosApi.Persistence.UnitsOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ConsultoriosApiDbContext context;

        public EFUnitOfWork(ConsultoriosApiDbContext context)
        {
            this.context = context;
        }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }

        public Task RollBack()
        {
            return Task.CompletedTask;
        }
    }
}
