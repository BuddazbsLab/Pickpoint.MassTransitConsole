namespace Pickpoint.RabbitMQ.Publisher.SizeText
{
    sealed internal class GenerateString
    {
        private static Random random = new Random();

        public static string generateASCIIStringBySize(long trafficSize)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, (int)trafficSize)
              .Select(s => s[random.Next(s.Length)]).ToArray()).ToLower();
        }
    }
}
