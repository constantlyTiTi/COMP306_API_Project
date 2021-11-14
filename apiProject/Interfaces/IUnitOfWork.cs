using apiProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    interface IUnitOfWork:IDisposable
    {
        IRateRepo Rate { get; set; }
        ISP_Call SP_Call { get; }
        IUserRepo User { get; set; }
        IOrderRepo Order { get; set; }
        IItemFileRepo Item { get; set; }
 
    }
}
