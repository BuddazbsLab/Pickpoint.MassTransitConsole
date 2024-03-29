﻿using System.Text;

namespace Pickpoint.MassTransitConsole.Publisher.SizeTextSend
{
    sealed internal class GenerateString
    {
        public static string generateASCIIStringBySize(long trafficSize)
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
