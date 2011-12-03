namespace Cartographer
{
	using System;

	public class MappingContext
	{
		public Type TargetType { get; set; }
		public object TargetInstance { get; set; }
		public object SourceInstance { get; set; }
	}
}
