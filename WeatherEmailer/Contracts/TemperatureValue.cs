namespace WeatherEmailer.Contracts
{
    public class TemperatureValue
    {
        public decimal Value { get; set; }
        public TemperatureUnit Unit { get; set; }

        public enum TemperatureUnit
        {
            Fahrenheit = 1,
            Celsius = 2
        }

        public override string ToString()
        {
            string unit = Unit switch
            {
                TemperatureUnit.Fahrenheit => "F",
                TemperatureUnit.Celsius => "C",
                _ => throw new System.Exception($"Unsupported temperature unit {Unit}")
            };
            return Value + unit;
        }
    }
}
