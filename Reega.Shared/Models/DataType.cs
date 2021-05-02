namespace Reega.Shared.Models
{
    public class DataType
    {
        public static DataType ELECTRICITY => new("electricity", 0, ServiceType.ELECTRICITY);
        public static DataType GAS => new("gas", 1, ServiceType.GAS);
        public static DataType WATER => new("water", 2, ServiceType.WATER);
        public static DataType PAPER=> new("paper", 3, ServiceType.GARBAGE);
        public static DataType GLASS=> new("glass", 4, ServiceType.GARBAGE);
        public static DataType PLASTIC=> new("plastic", 5, ServiceType.GARBAGE);
        public static DataType MIXED=> new("mixed", 6, ServiceType.GARBAGE);

        public string Name { get; }
        public int Value { get; }
        public ServiceType ServiceType { get; }

        private DataType() { }

        private DataType(string name, int value, ServiceType serviceType) 
        {
            this.Name = name;
            this.Value = value;
            this.ServiceType = serviceType;
        }
    }
}
