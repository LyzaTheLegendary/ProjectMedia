using Classes;
using Common.Cryptography;
using Common.Network.Packets.GuiPackets;
using Common.Network.Packets.MediaServerPackets;
using Common.Utilities;
using Gtk;
using System.Security.Cryptography;
using System.Text;

namespace Client.Gui.Views
{
    internal class LoginView : IView
    {
        Layout _container;
        Entry usernameInput;
        Entry passwordInput;
        Label response;
        Button submitBtn;
        public LoginView()
        {
            Gui.Resize(400, 300);

            Application.Invoke(delegate
            {
                _container      = new Layout(null, null);
                usernameInput   = new Entry();
                passwordInput   = new Entry();
                submitBtn       = new Button("Login");
                response        = new Label();
                submitBtn.Clicked += (_, _) => SendRequest(usernameInput.Buffer.Text, passwordInput.Buffer.Text);

                _container.Put(usernameInput, 75, 0);
                _container.Put(passwordInput, 75, 50);
                _container.Put(submitBtn, 178, 100);
                _container.Put(response, 200, 100);
                _container.ShowAll();
            });
        }

        private void SendRequest(string username, string password)
        {
            //Rfc2898DeriveBytes hash = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), new byte[] {1,5,2,1,8}, iterations: 69);
            //Globals.NetworkModule.PendMessage((ushort)PacketIds.LOGIN, new MSG_LOGIN(username, Encoding.ASCII.GetString(hash.GetBytes(13)))); // todo introduce md5hash
            Globals.NetworkModule.PendMessage((ushort)PacketIds.LOGIN, new MSG_LOGIN(username, password)); // todo introduce md5hash
        }
        
        public void Delete()
        {

        }

        public Widget GetContainer()
        {
            return _container;
        }

        public void ReceiveMessage(byte[] data)
        {
            MSG_LOGIN_RESULT msg = data.Cast<MSG_LOGIN_RESULT>();

            if (msg.GetSuccess())
                response.Text = msg.GetReason();
            else
                response.Text = msg.GetReason();
        }
    }
}
