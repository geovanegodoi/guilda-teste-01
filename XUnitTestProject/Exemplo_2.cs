using ClassLibrary.Exemplo_2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using XUnitTestProject.Spies;

namespace XUnitTestProject
{
    public class Exemplo_2
    {
        [Theory]
        [InlineData("1")]
        [InlineData("1,2,3")]
        [InlineData("1,2,3,4,5,6")]
        public void Teste_1_utilizando_mocks_nas_libs_externas(string ids)
        {
            // Arrange
            var azureHubMock  = new Mock<IAzureHub>();
            var azureBlobMock = new Mock<IAzureBlob>();
            var eventHub      = new EventHub(azureHubMock.Object);
            var blobStorage   = new BlobStorage(azureBlobMock.Object);
            var requestModel  = new RequestModel { Ids = ids, Name = "example-request", Attachment = new object() };
            var splitedIds    = requestModel.Ids.Split(',').ToList();
            var sut           = new SomeServiceClass(eventHub, blobStorage);

            // Act
            var result = sut.ExecuteSomeServiceOperation(requestModel);

            // Assert
            result.Should().BeTrue();
            azureHubMock.Verify(m => m.Send("hub-teste-v1", It.IsAny<object>()), Times.Exactly(splitedIds.Count));           
            azureBlobMock.Verify(m => m.SaveAsync("container-v2", requestModel.Attachment), Times.Once);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1,2,3")]
        [InlineData("1,2,3,4,5,6")]
        public void Teste_2_utilizando_mocks_nas_classes_wrappers(string ids)
        {
            // Arrange
            var eventHubMock    = new Mock<IEventHub>();
            var blobStorageMock = new Mock<IBlobStorage>();
            var requestModel    = new RequestModel { Ids = ids, Name = "example-request", Attachment = new object() };
            var splitedIds      = requestModel.Ids.Split(',').ToList();
            var sut             = new SomeServiceClass(eventHubMock.Object, blobStorageMock.Object);

            // Act
            var result = sut.ExecuteSomeServiceOperation(requestModel);

            // Assert
            result.Should().BeTrue();
            eventHubMock.Verify(m => m.SendMessageAsync(It.Is<MessageModel>(p => splitedIds.Contains(p.Id) && p.Name == requestModel.Name)), Times.Exactly(splitedIds.Count));
            blobStorageMock.Verify(m => m.SaveAttachment(requestModel.Attachment), Times.Once);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1,2,3")]
        [InlineData("1,2,3,4,5,6")]
        public void Teste_3_utilizando_spies_nas_classes_wrappers(string ids)
        {
            // Arrange
            var eventHubSpy     = new EventHubSpy();
            var blobStorageSpy  = new BlobStorageSpy();
            var requestModel    = new RequestModel { Ids = ids, Name = "example-request", Attachment = new object() };
            var splitedIds      = requestModel.Ids.Split(',').ToList();
            var sut             = new SomeServiceClass(eventHubSpy, blobStorageSpy);

            // Act
            var result = sut.ExecuteSomeServiceOperation(requestModel);

            // Assert
            result.Should().BeTrue();
            eventHubSpy.ShouldCountMessages(splitedIds.Count);
            blobStorageSpy.ShouldCountBlobs(count: 1);
        }
    }
}
