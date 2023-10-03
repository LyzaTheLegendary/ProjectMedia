using Common.Gui.Server;
using System.Diagnostics;
using Terminal.Gui;

public sealed class Program
{
    private static Process process;
    private static void Main()
    {
        process = Process.GetCurrentProcess();
        Display.ConstructInstance();
        for(int i = 0; i < 1000; i++)
            Display.WriteNet("test");
        process.WaitForExit();
        
    }
    public static void Exit()
    {
        Application.RequestStop();
        process.Close();
    }
}