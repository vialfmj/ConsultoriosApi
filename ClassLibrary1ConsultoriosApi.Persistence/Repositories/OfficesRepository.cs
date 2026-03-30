using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1ConsultoriosApi.Persistence.Repositories
{
    public class OfficesRepository : Repository<Office>, IOfficesRepository
    {
        public OfficesRepository(ConsultoriosApiDbContext context) : base(context)
        {
            
        }
    }
}
