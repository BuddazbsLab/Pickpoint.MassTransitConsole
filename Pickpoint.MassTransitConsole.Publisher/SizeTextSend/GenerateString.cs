using System.Text;

namespace Pickpoint.MassTransitConsole.Publisher.SizeTextSend
{
    internal class GenerateString
    {
        public static String generateASCIIStringBySize(long trafficSize)
        {
            StringBuilder sb = new StringBuilder();
            Random rd = new Random();

            for (int i = 0; i < trafficSize; i++)
            {
                sb.Append(rd.Next(255));
            }

            return sb.ToString().ToLower();
        }
    }
}
