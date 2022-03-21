

using System.Text;

namespace Pickpoint.MassTransitConsole.Publisher.Traffic
{
    internal class GenerateString
    {
        public static String generateStringSize(long trafficSize, string myAlphabet)
        {
            StringBuilder sb = new StringBuilder();
            Random rd = new Random();

            var alphabet = myAlphabet;
            int maxIndex = alphabet.Length - 1;

            for (int i = 0; i < trafficSize; i++)
            {
                int index = rd.Next(maxIndex);
                char c = alphabet[index];
                sb.Append(c);
            }

            return sb.ToString().ToLower();
        }
    }
}
