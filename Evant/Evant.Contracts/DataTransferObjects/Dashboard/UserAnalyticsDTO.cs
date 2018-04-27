namespace Evant.Contracts.DataTransferObjects.Dashboard
{
    public sealed class UserAnalyticsDTO
    {
        public T Teenager { get; set; }

        public T Young { get; set; }

        public T Middle { get; set; }

        public T Old { get; set; }
    }

    public class T
    {
        public string Name { get; set; }

        public double Value { get; set; } = 0;

        public int Min { get; set; }

        public int Max { get; set; }

        public double Ratio { get; set; } = 0.0;
    }
}
