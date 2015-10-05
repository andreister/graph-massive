using System.Collections.Generic;
using Graph.Core.Database.Repositories;
using Graph.Services.Common.Definitions;

namespace Graph.Services.Presentation
{
	internal class LoadEdgesRepository : Repository, ILoadEdgesRepository
	{
		public IEnumerable<Edge> LoadGraph()
		{
			return SelectAdHoc<Edge>(@"
				SELECT 
					source.Id    AS SourceId,
					source.Label AS Source,
					target.Id    AS TargetId,
					target.Label AS Target
				FROM Edges e
				INNER JOIN Nodes source ON source.Id = e.[From]
				INNER JOIN Nodes target ON target.Id = e.[To]
			");
		}
	}
}