using Common.Network.Clients;

internal sealed class Program
{
    public static void Main(string[] args)
    {
        Addr serverAddr = new(args[0]);
    }
}