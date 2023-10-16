﻿using Common.FileSystem;
using Common.Gui.Server;
using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.UpdateServerPackets;
using Common.Threading;
using Common.Utilities;
using Network;
using UpdateServer.Core.FileSystem;

namespace UpdateServer.Core.Network
{
    public class UpServer
    {
        readonly TcpListener listener;
        readonly TaskPool pool;
        readonly List<Client> clientList = new(100);
        readonly Storage storage;
        public UpServer(Addr addr)
        {
            storage = new Storage("FileSystem");
            var test = storage.GetLatestUpdateVer();
            pool = new(10);
            listener = new TcpListener(addr,10);
            listener.Listen(OnConnect);
        }
        public void OnConnect(Client client)
        {
            lock (clientList) { clientList.Add(client); }
            client.AddOnDisconnect(OnDisconnect);
            client.Receive(OnMessage);
            Display.AddConn(client.GetAddr());

        }
        public void OnDisconnect(Client client)
        {
            lock (clientList) { clientList.Remove(client); }
            Display.DelConn(client.GetAddr());
        }
        public void OnMessage(IClient client, Header header, byte[] buff) { // should turn with byte[] into Memory
            // imemory that block buffer copies on a cast
            switch((TS_CS)header.GetId())
            {
                case TS_CS.VERSION:
                    // Version check?
                    int remoteVer = buff.Cast<MSG_VERSION>().ver;
                    int localVer = storage.GetLatestUpdateVer();
                    if (remoteVer >= localVer)
                    {
                        client.PendMessage((ushort)TS_SC.UPDATE_FINISHED, new MSG_UPDATE_FINISHED(new Addr("127.0.0.1:25566")));
                        return;
                    }

                    List<mFile> files = new(100);
                    

                    while(remoteVer < (localVer - 1)) { 
                        mFile[]? tempFiles = storage.GetUpdate(remoteVer);
                        if(tempFiles != null)
                            files.AddRange(tempFiles);
                        remoteVer++;
                    }

                    client.PendMessage((ushort)TS_SC.UPDATE_INFO, new MSG_UPDATE_STATUS(false, files.Count, localVer - 1));

                    pool.EnqueueTask(() =>
                    {
                        foreach (mFile file in files) {
                            //Thread.Sleep(20);
                            client.PendMessage((ushort)TS_SC.UPDATE_FILE, (byte[])file);
                            Display.WriteNet($"Sent file: {file.GetFileName()} to: {client.GetAddr()}");
                            }
                        files.Clear();

                        //Thread.Sleep(200);
                        client.PendMessage((ushort)TS_SC.UPDATE_FINISHED, new MSG_UPDATE_FINISHED(new Addr("127.0.0.1:25566")));
                        Display.WriteNet($"Updated client from ver: {remoteVer - files.Count} to ver: {localVer - 1}");

                        //GC.Collect();
                        
                        client.Disconnect();
                    });

                    break;
            }
        }
    }
}
