namespace LoginAppServer.util
{
    internal class Util
    {
        public static void OutgoingMessage(params string[] messages)
        {
            Console.Clear();
            Console.WriteLine("\n--------------------------- Saída ----------------------------");

            foreach (string message in messages)
                Console.WriteLine(message);

            Console.WriteLine("--------------------------------------------------------------\n");
        }
    }
}
