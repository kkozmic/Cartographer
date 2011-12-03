namespace Cartographer
{
	using System;

	public class MappingContext
	{
		public object SourceInstance { get; set; }
		public object TargetInstance { get; set; }
		public Type TargetType { get; set; }
	}
}
