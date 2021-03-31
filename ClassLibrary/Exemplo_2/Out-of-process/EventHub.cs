using System.Threading.Tasks;

namespace ClassLibrary.Exemplo_2
{
    public interface IEventHub
    {
        void SendMessage(MessageModel model);
        Task SendMessageAsync(MessageModel model);
    }

    public class EventHub : IEventHub
    {
        private readonly IAzureHub _azureHub;

        public EventHub(IAzureHub azureEventHub)
        {
            _azureHub = azureEventHub;
        }

        public void SendMessage(MessageModel model)
            => _azureHub.Send(hubName: "hub-teste-v1", @event: model);

        public Task SendMessageAsync(MessageModel model)
        {
            SendMessage(model);
            return Task.CompletedTask;
        }
    }

    public interface IAzureHub
    {
        void Send(string hubName, object @event);
    }
}
