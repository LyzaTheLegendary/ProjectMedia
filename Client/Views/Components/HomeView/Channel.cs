using Gtk;

namespace Client.Views.Components.HomeView // TODO make me
{
    public class Channel : IComponent
    {
        private readonly Grid container;
        private readonly Button ListButton;
        private readonly Lazy<ChannelView> channelView;
        private readonly string _name;
        public Channel(string channelName)
        {
            _name = channelName;
            ListButton = new Button(channelName);
            container = new Grid();
        }
        public string GetName() => _name;
        public Button GetChannelBtn() => ListButton;
        public Widget GetContainer() => container;
    }
}
