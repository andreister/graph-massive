using System.Collections.Generic;
using Graph.Services.Traversal;
using Moq;
using NUnit.Framework;

namespace Tests.Graph.Services
{
	[TestFixture]
	public class TraversalServiceTests
	{
		[Test]
		[TestCaseSource("Scenarios")]
		public void FindsShortestPath(global::Graph.Services.Traversal.Graph graph, int from, int to, int[] expectedPath, string explanation)
		{
			var repository = new Mock<ILoadGraphRepository>();
			repository.Setup(x => x.LoadGraph()).Returns(graph);

			var service = new TraversalService(repository.Object);
			var path = service.FindShortestPath(from, to);

			Assert.That(path, Is.EqualTo(expectedPath), explanation);
		}

		public IEnumerable<TestCaseData> Scenarios
		{
			get
			{
				var result = new List<TestCaseData>();

				// 1 - 2 - 3 - 4
				// |___________|
				var graph1 = new global::Graph.Services.Traversal.Graph { {1, 2}, {2, 3}, {3, 4}, {1, 4} };
				result.Add(new TestCaseData(graph1, 1, 4, new[] {1,4}, "Nodes 1 and 4 are connected"));

				// 1 - 2 - 3 - 4 - 5
				// |___________|
				var graph2 = new global::Graph.Services.Traversal.Graph { {1, 2}, {2, 3}, {3, 4}, {4, 5}, {1, 4} };
				result.Add(new TestCaseData(graph2, 1, 5, new[] {1,4,5}, "Nodes 1 and 5 are connected via 4"));

				// 1 - 2   3 - 4   5
				//     |___|___|   |
				//         |_______|
				var graph3 = new global::Graph.Services.Traversal.Graph { { 1, 2 }, { 2, 4 }, { 4, 3 }, { 3, 5 } };
				result.Add(new TestCaseData(graph3, 1, 5, new[] { 1,2,4,3,5 }, "The only way is through all nodes"));
				
				return result;
			}
		}
	}
}

