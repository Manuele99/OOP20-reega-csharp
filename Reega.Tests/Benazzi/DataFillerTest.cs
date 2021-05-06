using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reega.Benazzi;
using Reega.Shared.Data;
using Reega.Shared.Models;
using Xunit;

namespace Reega.Tests.Benazzi
{
    public class DataFillerTest : IClassFixture<DataFixture>
    {
        private const long _serviceStepping = 3_600_000L; // one hour in ms
        private const long _garbageStepping = 86_400_000L; // one day in ms

        private readonly long _timeNow;

        private DataFixture DataFixture { get; }
        private IDataFiller DataFiller { get; }

        public DataFillerTest(DataFixture fixture)
        {
            this.DataFixture = fixture;
            this._timeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.DataFiller = new OnDemandDataFiller(DataFixture.DataController, DataFixture.ContractController);
            DataFixture.DataController.Reset();
        }

        /// <summary>
        /// check if the number of contracts the data has been generated for is right
        /// </summary>
        [Fact]
        public void ContractNumber()
        {
            this.DataFiller.Fill(); //after this data should be in the DataController
            Dictionary<IContract, ISet<Data>> dataDictionary = this.DataFixture.DataController.ContractData;
            // the number of contract in the dictionary should be the same as the number of the ones declared in Contracts
            Assert.Equal(dataDictionary.Keys.Count, Contracts.ContractList.Count); 
           
        }

        /// <summary>
        /// check if the number of data values generated for each constract is right
        /// </summary>
        [Fact]
        public void DataNumber()
        {
            this.DataFiller.Fill(); //after this data should be in the DataController
            Dictionary<IContract, ISet<Data>> dataDictionary = this.DataFixture.DataController.ContractData;
            //check foreach data of each contract if the number of values generated is right
            foreach (IContract contract in Contracts.ContractList)
            {
                int serviceCount = (int)Math.Floor((double)(this._timeNow - new DateTimeOffset(contract.StartDate).ToUnixTimeMilliseconds()) / (double)_serviceStepping);
                int garbageCount = (int)Math.Floor((double)(this._timeNow - new DateTimeOffset(contract.StartDate).ToUnixTimeMilliseconds()) / (double)_garbageStepping);
                foreach (Data data in dataDictionary[contract])
                {
                    if (data.Type.ServiceType == ServiceType.GARBAGE)
                    {
                        Assert.Equal(data.DataValuesByTimestamp.Count, garbageCount);
                    }
                    else
                    {
                        Assert.Equal(data.DataValuesByTimestamp.Count, serviceCount);
                    }
                }
            }
        }
    }

    public class DataFixture
    {
        public TestDataController DataController { get; }
        public TestContractController ContractController { get; }

        public DataFixture()
        {
            this.DataController = new();
            this.ContractController = new();
        }
    }

    /// <summary>
    /// Data controller implementation made for testing purposes
    /// </summary>
    public class TestDataController : IDataController
    {
        private Dictionary<IContract, ISet<Data>> _contractData = new();

        //used to check for filled data or not
        public Dictionary<IContract, ISet<Data>> ContractData => this._contractData;

        // non present contractID unhandled
        public long GetLatestData(int contractID, DataType service)
        {
            DateTimeOffset retDate;
            KeyValuePair<IContract, ISet<Data>> contractSetPair = this._contractData.FirstOrDefault(e => e.Key.Id == contractID);
            if (contractSetPair.Key != default
                && contractSetPair.Key.Services.Any(e => e == service.ServiceType)
                && contractSetPair.Value.Any(e => e.Type == service))
            {
                retDate = DateTimeOffset.FromUnixTimeMilliseconds(contractSetPair.Value.Max(e => e.DataValuesByTimestamp.Keys).First());
            } else
            {
               retDate = Contracts.ContractList.Where(e => contractID == e.Id).First().StartDate;
            }
            return retDate.ToUnixTimeMilliseconds();
        }

        public List<Data> GetMonthlyData(int contractID)
        {
            throw new NotImplementedException();
        }

        public void PutUserData(Data data)
        {
            IContract contract = Contracts.ContractList.Where(e => data.ContractId == e.Id).First();
            if(this._contractData.ContainsKey(contract))
            {
                ISet<Data> dataSet = this._contractData[contract];
                Data dataD = dataSet.FirstOrDefault(e => e.Type == data.Type);
                if (dataD != default)
                    dataD.AddRecords(data.DataValuesByTimestamp);
                else
                    dataSet.Add(data);
            } else
            {
                this._contractData.Add(contract, new HashSet<Data>() { data });
            }
        }

        //used after each test
        public void Reset()
        {
            this._contractData = new();
        }
    }

    /// <summary>
    /// ContractController implementation made for testing purposes
    /// </summary>
    public class TestContractController : IContractController
    {
        public List<IContract> UserContracts => Contracts.ContractDictionary["Ernesto"];

        public List<IContract> AllContracts => Contracts.ContractList;

        public List<IContract> GetContractsForUser(string fiscalCode)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Class which constains a list of contracts made for testing
    /// </summary>
    public static class Contracts
    {

        private static readonly List<IContract> _contractList = new List<IContract>()
        {
            new Contract(0, "Guanabana", ServiceType.Values.ToList(), new DateTime(2020, 12, 1)),
            new Contract(1, "Baird Bay", new List<ServiceType>() { ServiceType.ELECTRICITY, ServiceType.WATER}, new DateTime(2021, 05, 2)),
            new Contract(2, "Ravenel, SC", new List<ServiceType>() { ServiceType.GARBAGE}, new DateTime(2021, 05, 2))
        };

        private static readonly Dictionary<string, List<IContract>> _contractDictionary = new Dictionary<string, List<IContract>>()
        {
            {"Ernesto", new List<IContract>() { _contractList[0], _contractList[2] } },
            {"Tony", new List<IContract>() { _contractList[1] } }
        };

        public static List<IContract> ContractList => _contractList;
        public static Dictionary<string, List<IContract>> ContractDictionary => _contractDictionary;
    }
}
