namespace ClassLibrary.Exemplo_2
{
    public class MessageModel
    {
        public MessageModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
