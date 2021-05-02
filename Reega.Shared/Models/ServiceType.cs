namespace Reega.Shared.Models
{
    public class ServiceType
    {
        public static ServiceType ELECTRICITY => new("electricity", 0);
        public static ServiceType GAS => new("gas", 1);
        public static ServiceType WATER => new("water", 2);
        public static ServiceType GARBAGE => new("garbage", 3);

        public string Name { get; }
        public int Value { get; }

        private ServiceType() { }

        private ServiceType(string name,int value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
