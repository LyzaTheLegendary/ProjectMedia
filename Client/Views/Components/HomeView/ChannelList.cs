using Gtk;
using static System.Net.Mime.MediaTypeNames;

namespace Client.Views.Components.HomeView
{
    public class ChannelList : IComponent
    {
        private readonly ScrolledWindow container;
        private readonly Grid channelGrid;
        private readonly List<Channel> channels = new();

        public ChannelList()
        {
            container = new ScrolledWindow();
            channelGrid = new Grid();
            container.Add(channelGrid);
            //for (int i = 0; i < 10; i++)
            //{
            //    Channel channel = new("test:" + i);
                
            //    AddChannel(channel);
            //}
            //RemoveChannel("test:2");
        }
        public void AddChannel(Channel channel)
        {
            if (channels.Count == 0)
                channelGrid.Attach(channel.GetChannelBtn(), 5, 0, 10, 10);
            else
                channelGrid.AttachNextTo(channel.GetChannelBtn(), channels.Last().GetChannelBtn(), PositionType.Bottom, 10, 10);
            channels.Add(channel);
        }
        public void RemoveChannel(string name)
        {
            Channel? channel = channels.Select(channel => channel).Where(channel => channel.GetName() == name).FirstOrDefault();
            if (channel == null)
                return;

            channelGrid.Remove(channel.GetChannelBtn());
            channels.Remove(channel);
        }
        public Widget GetContainer() => container;
    }
}
