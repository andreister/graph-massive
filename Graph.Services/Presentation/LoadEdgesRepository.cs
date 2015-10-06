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
					source.Id				AS SourceId,
					source.Label			AS Source,
					CASE 
						WHEN target.Id IS NULL THEN source.Id
						ELSE target.Id
					END						AS TargetId,
					CASE 
						WHEN target.Label IS NULL THEN source.Label
						ELSE target.Label
					END						AS Target
				FROM Nodes source
				LEFT OUTER JOIN Edges e ON e.[From] = source.Id
				LEFT OUTER JOIN Nodes target ON target.Id = e.[To]
			");
		}
	}
}