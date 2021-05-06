using System;
using System.Collections.Generic;
using System.Linq;

namespace Reega.Shared.Models
{
    public class DataType
    {
        public static DataType ELECTRICITY => new("electricity", 0, ServiceType.ELECTRICITY);
        public static DataType GAS => new("gas", 1, ServiceType.GAS);
        public static DataType WATER => new("water", 2, ServiceType.WATER);
        public static DataType PAPER => new("paper", 3, ServiceType.GARBAGE);
        public static DataType GLASS => new("glass", 4, ServiceType.GARBAGE);
        public static DataType PLASTIC => new("plastic", 5, ServiceType.GARBAGE);
        public static DataType MIXED => new("mixed", 6, ServiceType.GARBAGE);

        public static IEnumerable<DataType> Values
        {
            get
            {
                yield return ELECTRICITY;
                yield return GAS;
                yield return WATER;
                yield return PAPER;
                yield return GLASS;
                yield return PLASTIC;
                yield return MIXED;

            }
        }

        public string Name { get; }
        public int Value { get; }
        public ServiceType ServiceType { get; }

        private DataType(string name, int value, ServiceType serviceType)
        {
            Name = name;
            Value = value;
            ServiceType = serviceType;
        }

        public static IList<DataType> GetDataTypesByService(ServiceType svcType)
        {
           return DataType.Values.Where(e => e.ServiceType == svcType).ToList();
        }

        public override bool Equals(object obj)
        {
            return obj is DataType type &&
                   Name == type.Name &&
                   Value == type.Value &&
                   EqualityComparer<ServiceType>.Default.Equals(ServiceType, type.ServiceType);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value, ServiceType);
        }

        public static bool operator ==(DataType left, DataType right) =>
            left.Equals(right);

        public static bool operator !=(DataType left, DataType right) => !(left == right);
    }
}
