using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.YardSale.Contracts
{
    public interface IDatabaseRepository
    {
        DataTable GetDT(string storedProcName, List<object>? parameters = null, string connectionStringName = "DatabaseString");
        int GetRetVal(string storedProcName, List<object>? parameters = null, string connectionStringName = "DatabaseString");

    }
}
