using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.Exemplo_2
{
    public class SomeServiceClass
    {
        private readonly IEventHub    _eventHub;
        private readonly IBlobStorage _blobStorage;

        public SomeServiceClass(IEventHub eventHub, IBlobStorage blobStorage)
        {
            _eventHub = eventHub;
            _blobStorage = blobStorage;
        }

        public bool ExecuteSomeServiceOperation(RequestModel model)
        {
            if (model == null) return false;

            try
            {
                var messages = MapRequestToMessageById(model);
                
                messages.ForEach(message =>
                {
                    _eventHub.SendMessageAsync(message);
                });

                _blobStorage.SaveAttachment(blob: model.Attachment);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private List<MessageModel> MapRequestToMessageById(RequestModel request)
        {
            var ids = request.Ids.Split(',').ToList();
            var messages = new List<MessageModel>();

            ids.ForEach(id => 
            {
                messages.Add(new MessageModel(id, request.Name)); 
            });
            return messages;
        }
    }
}
