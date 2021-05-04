using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Shared.Models
{
    public class Contract : IContract
    {
        public int Id { get; }

        public string Address { get; }

        public List<ServiceType> Services { get; }

        public DateTime StartDate { get; }

        public Contract(int id, string address, List<ServiceType> services) : this(id, address, services, DateTime.Now) { }

        public Contract(int id, string address, List<ServiceType> services, DateTime startDate)
        {
            this.Id = id;
            this.Address = address;
            this.Services = services;
            this.StartDate = startDate;
        }
    }
}
