using System;
using System.Collections.Generic;

namespace Reega.Shared.Models
{
    public class ServiceType
    {
        public static ServiceType ELECTRICITY => new("electricity", 0);
        public static ServiceType GAS => new("gas", 1);
        public static ServiceType WATER => new("water", 2);
        public static ServiceType GARBAGE => new("garbage", 3);

        public static IEnumerable<ServiceType> Values
        {
            get
            {
                yield return ELECTRICITY;
                yield return GAS;
                yield return WATER;
                yield return GARBAGE;
            }
        }

        public string Name { get; }
        public int Value { get; }

        private ServiceType(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

        public static bool operator ==(ServiceType svc1, ServiceType svc2) => svc1.Equals(svc2);

        public static bool operator !=(ServiceType svc1, ServiceType svc2) => !(svc1 == svc2);

        public override bool Equals(object obj)
        {
            return obj is ServiceType type &&
                   Name == type.Name &&
                   Value == type.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Value);
        }
    }
}
