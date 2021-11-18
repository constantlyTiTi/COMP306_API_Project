using apiProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRateRepo Rate { get; set; }
        ISP_Call SP_Call { get; }
        IUserRepo User { get; set; }
        IOrderItemRepo OrderItem { get; set; }
        IItemFileRepo ItemFile { get; set; }
        IOrderDetailsRepo OrderDetails { get; set; }
        IItemRepo Item { get; set; }
    }
}
