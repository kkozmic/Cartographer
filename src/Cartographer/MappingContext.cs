namespace Cartographer
{
	using System;

	public class MappingContext
	{
		public IMapper Mapper { get; set; }
		public object SourceInstance { get; set; }

		public Type SourceType
		{
			get { return SourceInstance.GetType(); }
		}

		public object TargetInstance { get; set; }

		public Type TargetType { get; set; }
	}
}
