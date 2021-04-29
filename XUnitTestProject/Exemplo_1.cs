using ClassLibrary.Exemplo_1;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnitTestProject
{
    public class Exemplo_1
    {       
        /*
        * -----------------------------------------
        * Protection against regression    | Good |
        * -----------------------------------------
        * Resistance to refactoring        | Good |
        * -----------------------------------------
        * Fast feedback                    | Bad  |
        * -----------------------------------------
        * Maintainability                  | Bad  |
        * ------------------------------------------
        */
        [Fact(Skip = "Nao executar")]
        public void Teste_1_nao_utilizando_test_doubles()
        {            
            // Arrange
            var directoryName = @"C:\Dextra\Guilda\GuildaDeTestes\Logs";
            var sut = new AuditManager_1(maxEntriesPerFile: 3, directoryName);
            
            // Act
            sut.AddRecord(visitorName: "Alice",  timeOfVisit: DateTime.Parse("2019-04-06T18:00:00"));

            // Assert ?
        }

        /*
        * -----------------------------------------
        * Protection against regression    | Good |
        * -----------------------------------------
        * Resistance to refactoring        | Good |
        * -----------------------------------------
        * Fast feedback                    | Good |
        * -----------------------------------------
        * Maintainability                  | Bad  |
        * ------------------------------------------
        */
        [Fact]
        public void Teste_2_utilizando_mocks_para_depencias_externas()
        {
            // Arrange
            var fileSystemMock = new Mock<IFileSystem>();
            fileSystemMock.Setup(x => x.GetFiles("audits"))
                          .Returns(new string[] 
                          { 
                              @"audits\audit_1.txt", 
                              @"audits\audit_2.txt" 
                          });
            fileSystemMock.Setup(x => x.ReadAllLines(@"audits\audit_2.txt"))
                          .Returns(new List<string>
                          {
                                "Peter; 2019-04-06T16:30:00",
                                "Jane; 2019-04-06T16:40:00",
                                "Jack; 2019-04-06T17:00:00"
                          });
            var sut = new AuditManager_2(maxEntriesPerFile: 3, directoryName: "audits", fileSystemMock.Object);
            var timeOfVisit = DateTime.Parse("2019-04-06T18:00:00");

            // Act
            sut.AddRecord(visitorName: "Alice", timeOfVisit);
            
            // Assert
            fileSystemMock.Verify(x => x.WriteAllText(@"audits\audit_3.txt", $"Alice;{timeOfVisit}"));
        }

        /*
        * -----------------------------------------
        * Protection against regression    | Good |
        * -----------------------------------------
        * Resistance to refactoring        | Good |
        * -----------------------------------------
        * Fast feedback                    | Good |
        * -----------------------------------------
        * Maintainability                  | Good |
        * ------------------------------------------
        */
        [Fact]
        public void Teste_3_utilizando_classe_de_dominio_sem_mocks()
        {
            // Arrange
            var sut = new AuditManager_3(maxEntriesPerFile: 3);
            var timeOfVisit = DateTime.Parse("2019-04-06T18:00:00");
            var files = new FileContent[]  
            {
                new FileContent("audit_1.txt", new[]  
                { 
                    "Ana; 27/03/2021 09:05:47", 
                    "Jose; 27/03/2021 09:06:14", 
                    "Joao; 27/03/2021 09:06:37" 
                }) 
            };

            // Act
            var result = sut.AddRecord(files, visitorName: "Alice", timeOfVisit);

            // Assert
            result.FileName.Should().Be("audit_2.txt");
            result.NewContent.Should().Be($"Alice;{timeOfVisit}");
        }
    }
}
