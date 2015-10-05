using System.Xml.Linq;
using Graph.Core.Database.Generated;
using Graph.Services.Storage;
using Moq;
using NUnit.Framework;

namespace Tests.Graph.Services
{
	[TestFixture]
	public class StorageServiceTests
	{
		[Test]
		public void XmlParsedAndSaved()
		{
			var repository = new Mock<IStorageRepository>();
			var service = new StorageService(repository.Object);

			var node1 = new {Id = 123, Label = "qweqwe"};
			var node2 = new {Id = 456, Label = "asdasd"};
			
			var root = new XElement("nodes",
				new XElement("node", new XElement("id", node1.Id), new XElement("label", node1.Label)),
				new XElement("node", new XElement("id", node2.Id), new XElement("label", node2.Label))
			);
			
			service.SaveGraph(root);

			repository.Verify(x => x.Save(It.IsAny<Node>()), Times.Exactly(2), "Nodes should get saved to the database");
			repository.Verify(x => x.Save(It.Is<Node>(xx => xx.Id == node1.Id && xx.Label == node1.Label)), Times.Once, "Nodes should get saved to the database");
			repository.Verify(x => x.Save(It.Is<Node>(xx => xx.Id == node2.Id && xx.Label == node2.Label)), Times.Once, "Nodes should get saved to the database");
		}
	}
}
