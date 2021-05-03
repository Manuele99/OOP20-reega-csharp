using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Shared.Models
{
    public interface IContract
    {
        /// <summary>
        /// return the contract's ID
        /// </summary>
        int Id { get; }

        /// <summary>
        /// return the address related to this contract
        /// </summary>
        String Address { get; }

        /// <summary>
        /// return the list of provided services
        /// </summary>
        List<ServiceType> Services { get; }

        /// <summary>
        /// return the contract's start date
        /// </summary>
        DateTime StartDate { get; }
    }
}
