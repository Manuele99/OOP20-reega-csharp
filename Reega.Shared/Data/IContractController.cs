using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Shared.Models

namespace Reega.Shared.Data
{
    public interface IContractController
    {
        List<IContract> UserContracts { get; }

        List<IContract> GetContractsForUser(String fiscalCode);

        List<IContract> AllContracts { get; }

        //IContract AddContract(NewContract contract);

        //void RemoveContract(int id);

        //List<Contract> searchContract(String keyword);

        //List<MonthlyReport> getBillsForContracts(List<Integer> contractIDs)

    }
}
