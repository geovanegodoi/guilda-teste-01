using System.Threading.Tasks;

namespace ClassLibrary.Exemplo_2
{
    public interface IBlobStorage
    {
        void SaveAttachment(object blob);
    }

    public class BlobStorage : IBlobStorage
    {
        private IAzureBlob _azureBlob;

        public BlobStorage(IAzureBlob externalBlobStorage)
        {
            _azureBlob = externalBlobStorage;
        }

        public void SaveAttachment(object blob)
            => _azureBlob.SaveAsync(container: "container-v2", blob);

        public Task SaveAttachmentAsync(object blob)
        {
            SaveAttachment(blob);
            return Task.CompletedTask;
        }
    }

    public interface IAzureBlob
    {
        void Save(string container, object blob);
        Task SaveAsync(string container, object blob);
    }
}
