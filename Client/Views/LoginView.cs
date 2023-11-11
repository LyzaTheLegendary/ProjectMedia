using Classes;
using Client.Views;
using Common.Cryptography;
using Common.FileSystem;
using Common.FileSystem.Storage;
using Common.Network.Packets;
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
            Gui.Resize(200, 300);
            Label usernameLabel = new();
            Label passwordLabel = new();
            usernameLabel.Text = "Username";
            passwordLabel.Text = "Password";
            

            _container      = new Layout(null, null);
            usernameInput   = new Entry();
            passwordInput   = new Entry();
            submitBtn       = new Button("Login");
            response        = new Label();
            submitBtn.Clicked += (_, _) => SendRequest(usernameInput.Buffer.Text, passwordInput.Buffer.Text);

            _container.Put(usernameLabel, 75, 10);
            _container.Put(usernameInput, 75, 30);
            _container.Put(passwordLabel, 75, 80);
            _container.Put(passwordInput, 75, 100);
            _container.Put(submitBtn, 178, 150);
            _container.Put(response, 80, 135);
            //_container.ShowAll();
        }

        private void SendRequest(string username, string password)
        {
            submitBtn.Sensitive = false;
            usernameInput.Sensitive = false;
            passwordInput.Sensitive = false;
            //Rfc2898DeriveBytes hash = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), new byte[] {1,5,2,1,8}, iterations: 69);
            //Globals.NetworkModule.PendMessage((ushort)PacketIds.LOGIN, new MSG_LOGIN(username, Encoding.ASCII.GetString(hash.GetBytes(13)))); // todo introduce md5hash
            Globals.NetworkModule.PendMessage((ushort)PacketIds.LOGIN, new MSG_LOGIN(username, password), (code) =>
            {
                Application.Invoke(delegate
                {
                    switch (code)
                    {
                        case ResultCodes.Success:
                            Gui.SetView(new HomeView());
                            break;
                        case ResultCodes.NotFound:
                            response.Text = "Invalid login information!";
                            submitBtn.Sensitive = true;
                            usernameInput.Sensitive = true;
                            passwordInput.Sensitive = true;
                            break;
                    }
                });
            }); // todo introduce md5hash
        }

        public void HandleGuiRequest(byte[] data)
        {
            //MSG_LOGIN_RESULT result = data.Cast<MSG_LOGIN_RESULT>();
            //Application.Invoke(delegate
            //{
            //    response.Visible = true;
            //    response.Text = result.GetReason();
            //    if(!result.GetSuccess()) {
            //        submitBtn.Sensitive = true;
            //        usernameInput.Sensitive = true;
            //        passwordInput.Sensitive = true;
            //        return;
            //    }
                
            //    mFile file = new("token.t", Encoding.ASCII.GetBytes(result.GetToken()));
            //    Storage.SaveFile(file);
                
                
            //});
        }

        public Widget GetContainer()
        {
            return _container;
        }
    }
}
