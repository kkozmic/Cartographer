namespace Cartographer
{
	using System;
	using Cartographer.Internal;

	public class MappingContext
	{
		readonly Arguments arguments;

		public MappingContext(Arguments arguments)
		{
			this.arguments = arguments;
		}

		public IMapper Mapper { get; set; }

		public object SourceInstance { get; set; }

		public Type SourceType
		{
			get { return SourceInstance.GetType(); }
		}

		public object TargetInstance { get; set; }

		public T Argument<T>()
		{
			return (T)arguments.GetByType(typeof (T));
		}
	}
}