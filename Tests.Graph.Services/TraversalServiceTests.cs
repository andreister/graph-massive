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
		public void FindsShortestPath(global::Graph.Services.Graph graph, int from, int to, int expectedLengh, string explanation)
		{
			var repository = new Mock<ILoadGraphRepository>();
			repository.Setup(x => x.LoadGraph()).Returns(graph);

			var service = new TraversalService(repository.Object);
			int pathLength = service.FindShortestPath(from, to);

			Assert.That(pathLength, Is.EqualTo(expectedLengh), explanation);
		}

		public IEnumerable<TestCaseData> Scenarios
		{
			get
			{
				var result = new List<TestCaseData>();

				// 1 - 2 - 3 - 4
				// |___________|
				var graph1 = new global::Graph.Services.Graph { {1, 2}, {2, 3}, {3, 4}, {1, 4} };
				var length1 = 1;
				result.Add(new TestCaseData(graph1, 1, 4, length1, "Nodes 1 and 4 are connected, so shortest path should be 1"));

				// 1 - 2 - 3 - 4 - 5
				// |___________|
				var graph2 = new global::Graph.Services.Graph { {1, 2}, {2, 3}, {3, 4}, {4, 5}, {1, 4} };
				var length2 = 2;
				result.Add(new TestCaseData(graph2, 1, 5, length2, "Nodes 1 and 5 are connected via 4, so shortest path should be 2"));

				return result;
			}
		}
	}
}

