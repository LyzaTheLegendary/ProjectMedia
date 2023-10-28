using Gtk;
using System.Security.Cryptography;

namespace Client.Gui.Views
{
    internal class LoginView : IView
    {
        Widget[] _widgets;
        TextView usernameInput;
        TextView passwordInput;
        Button submitBtn;
        public LoginView()
        {
            Gui.Resize(400, 400);
            List<Widget> widgets = new List<Widget>();
            usernameInput = new TextView();
            passwordInput = new TextView();
            submitBtn = new Button("Login");

            submitBtn.Clicked += (_,_) => SendRequest(usernameInput.Buffer.Text, passwordInput.Buffer.Text);

            widgets.Add(usernameInput); 
            widgets.Add(passwordInput);
            widgets.Add(submitBtn);
            _widgets = widgets.ToArray();
        }
        private void SendRequest(string username, string password)
        {

        }
        public void Delete()
        {
            foreach (Widget widget in _widgets)
                Gui.RemoveWidget(widget);
        }

        public Widget[] GetAllWidgets()
        {
            return _widgets;
        }
    }
}
