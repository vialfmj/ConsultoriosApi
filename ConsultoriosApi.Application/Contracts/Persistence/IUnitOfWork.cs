using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        Task Commit();
        Task RollBack();
    }
}
