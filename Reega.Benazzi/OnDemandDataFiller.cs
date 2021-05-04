using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Shared.Models;
using Reega.Shared.Data;

namespace Reega.Benazzi
{
    public class OnDemandDataFiller : IDataFiller
    {
        private const long _serviceStepping = 3_600_000L; // one hour in ms
        private const long _garbageStepping = 86_400_000L; // one day in ms
        private readonly long _startDate = DateTimeOffset.Now.ToUnixTimeMilliseconds() - 2_592_600_000L; // 30 days and 10 min ago in ms

        private readonly IDictionary<IUsageSimulator, ISet<Data>> _usageDataDictionary;
        private readonly IDataController _database;
        private readonly long _currentDate;

        public OnDemandDataFiller(IDataController dataController, IContractController contractController)
        {
            this._currentDate = DateTimeOffset.Now.AddMinutes(1).ToUnixTimeMilliseconds();
            this._database = dataController;
            this._usageDataDictionary = new Dictionary<IUsageSimulator, ISet<Data>>();
            this.AddContracts(contractController.AllContracts);
        }

        private void AddContracts(IList<IContract> contracts)
        {
            foreach (IContract contract in contracts)
            {
                List<DataType> svcList = new List<DataType>();
                ISet<Data> dataSet = new HashSet<Data>();
                foreach (ServiceType svc in contract.Services)
                {
                    svcList.AddRange(DataType.GetDataTypesByService(svc));
                }
                foreach (DataType svc in svcList)
                {
                    dataSet.Add(new Data(contract.Id, svc));
                }
                this._usageDataDictionary.Add(new SelectiveUsageSimulator(svcList), dataSet);
            }
        }

        public void Fill()
        {
            foreach (KeyValuePair<IUsageSimulator, ISet<Data>> entry in this._usageDataDictionary)
            {
                foreach (Data data in entry.Value)
                {
                    this.GenerateValues(entry.Key, data);
                    this._database.PutUserData(data);
                }
            }
        }

        private void GenerateValues(IUsageSimulator simulator , Data data)
        {
            Dictionary<long, double> simulations = new();
            long stepping = data.Type.ServiceType == ServiceType.GARBAGE ? _garbageStepping : _serviceStepping;
            long? dataDate = this._database.GetLatestData(data.ContractId, data.Type);
            if (dataDate is null || dataDate == 0L)
            {
                dataDate = this._startDate;
            }
            else
            {
                dataDate += stepping;
            }
            while (dataDate <= this._currentDate)
            {
                simulations.Add(dataDate.Value, simulator.GetUsage(data.Type).Value);
                dataDate += stepping;
            }
            data.AddRecords(simulations);
        }
    }
}
