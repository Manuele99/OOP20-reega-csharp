namespace Reega.Shared.Models
{
    public class ServiceType
    {
        private ServiceType()
        {
        }

        private ServiceType(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public static ServiceType ELECTRICITY => new("electricity", 0);
        public static ServiceType GAS => new("gas", 1);
        public static ServiceType WATER => new("water", 2);
        public static ServiceType GARBAGE => new("garbage", 3);

        public string Name { get; }
        public int Value { get; }
    }
}
