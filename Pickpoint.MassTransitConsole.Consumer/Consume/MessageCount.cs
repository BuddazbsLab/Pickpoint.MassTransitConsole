namespace Pickpoint.MassTransitConsole.Consumer.Consume
{
    internal class MessageCount
    {
        public static int messageNumber { get; set; }

        public static void ConsumeCount(int numberMessage)
        {
            messageNumber = numberMessage;
        }

    }
}
