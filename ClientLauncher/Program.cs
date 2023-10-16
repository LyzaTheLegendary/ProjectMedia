using ClientLauncher;
using Common.FileSystem;
using Common.FileSystem.Storage;
using Common.Network.Clients;
using System.Diagnostics;

public sealed class Program
{
    private static Updater updater;
    private static Process process = Process.GetCurrentProcess();
    private static void Main()
    {
        updater = new Updater("127.0.0.1:25565") // will rewrite for now as it directly just dumps what it gets, not a fan of it!
        {
            onCompleted = OnComplete,
            onFailure = OnFailure,
            onNewFile = OnNewFile,
        };

        process.WaitForExit();

    }
    public static void OnNewFile(mFile file)
    {
        Console.WriteLine($"File {updater.filesLeft++}/{updater.totalFiles}: {file.GetFileName()}");
        Storage.SaveFile(file);
    }
    public static void OnComplete(Addr serverAddr)
    {
        Console.WriteLine("Completed!");
        Thread.Sleep(1000); // making sure all files have time to save

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "Client.exe";
        startInfo.Arguments = serverAddr.ToString();
        //Process.Start(startInfo);

        Environment.Exit(0);
    }
    public static void OnFailure() 
    {
        Console.WriteLine("Failed to update");
    }
    public static void Exit()
    {
        process.Close();
    }
}