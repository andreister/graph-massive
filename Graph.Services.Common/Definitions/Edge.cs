using System.Runtime.Serialization;

namespace Graph.Services.Common.Definitions
{
	[DataContract(IsReference = true)]
	public class Edge
	{
		[DataMember] 
		public int SourceId { get; set; }

		[DataMember] 
		public string Source { get; set; }

		[DataMember] 
		public int TargetId { get; set; }

		[DataMember] 
		public string Target { get; set; }

		public override string ToString()
		{
			return Source + " - " + Target;
		}
	}
}
