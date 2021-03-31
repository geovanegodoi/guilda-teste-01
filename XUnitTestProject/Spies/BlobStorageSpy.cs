using ClassLibrary.Exemplo_2;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestProject.Spies
{
    public class BlobStorageSpy : IBlobStorage
    {
        private List<object> _blobs = new List<object>();

        public void SaveAttachment(object blob) 
            => _blobs.Add(blob);

        public Task SaveAttachmentAsync(string container, object blob)
        {
            SaveAttachment(blob);
            return Task.CompletedTask;
        }

        public void ShouldCountBlobs(int count)
            => _blobs.Count.Should().Be(count);
    }
}
