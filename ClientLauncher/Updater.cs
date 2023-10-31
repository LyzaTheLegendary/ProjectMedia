using Common.FileSystem;
using Common.Network.Clients;
using Common.Network.Packets.UpdateServerPackets;
using Common.Utilities;
using Network;

namespace ClientLauncher
{
    public class Updater
    {
        public int totalFiles = 0;
        public int filesLeft = 1;
        private int localVer;
        private int remoteVer;
        private bool completed = false;
        public Action<mFile>? onNewFile;
        public Action<Addr>? onCompleted;
        public Action? onFailure;
        public Updater(Addr host) {
            Client client = new(host);
            
            client.Receive(OnMessage);
            client.AddOnDisconnect(OnDisconnect);

            if (!File.Exists("ver"))
                localVer = 0;
            else
                localVer = File.ReadAllBytes("ver").Cast<int>();
            client.PendMessage((ushort)TS_CS.VERSION, new MSG_VERSION(localVer));
        }
        private void OnDisconnect(IClient client)
        {
            if (!completed)
                onFailure?.Invoke();
            
        }
        private void OnMessage(IClient client, Header header, byte[] buff)
        {
            switch ((TS_SC)header.GetId())
            {
                case TS_SC.UPDATE_FILE:
                    onNewFile?.Invoke((mFile)buff);
                    break;
                case TS_SC.UPDATE_FINISHED:
                    completed = true;
                    using (FileStream fs = File.Create("ver")) { fs.Write(MarshalUtil.StructToBytes<int>(remoteVer)); } ;
                    onCompleted?.Invoke((Addr)buff.Cast<MSG_UPDATE_FINISHED>());
                    client.Disconnect();
                    break;
                case TS_SC.UPDATE_INFO:
                    MSG_UPDATE_STATUS status = buff.Cast<MSG_UPDATE_STATUS>();
                    totalFiles = status.files;
                    remoteVer = status.remoteVer;
                    break;
            }
        }
    }
}
