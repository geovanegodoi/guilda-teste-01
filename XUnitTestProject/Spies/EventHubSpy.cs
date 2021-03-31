using ClassLibrary.Exemplo_2;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestProject.Spies
{
    public class EventHubSpy : IEventHub
    {
        private List<MessageModel> _events = new List<MessageModel>();

        public void SendMessage(MessageModel model) 
            => _events.Add(model);

        public Task SendMessageAsync(MessageModel model)
        {
            SendMessage(model);
            return Task.CompletedTask;
        }

        public void ShouldCountMessages(int count) 
            => _events.Count.Should().Be(count);

        public void WithMessage(MessageModel model) 
            => _events.Should().Contain(model);
    }
}
