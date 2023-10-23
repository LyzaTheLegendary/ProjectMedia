using Client.Gui;
using Common.Network.Clients;
using Gtk;

internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Addr serverAddr = new(args[0]);
        Application.Init();
        new MainWIndow("test");
        Application.Run();
    }
}