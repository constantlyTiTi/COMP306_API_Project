using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface ISP_Call
    {
        Task Dispose();

        //scalar return int or bool
        Task<T> Single<T>(string prcedureName, DynamicParameters param = null);

        Task Execute(string prcedureName, DynamicParameters param = null);
        Task<T> OneRecord<T>(string prcedureName, DynamicParameters param = null);
        IEnumerable<T> List<T>(string prcedureName, DynamicParameters param = null);
        
        //returns two tables
        Tuple<IEnumerable<T1>,IEnumerable<T2>> List<T1,T2>(string prcedureName, DynamicParameters param = null);
    }
}
