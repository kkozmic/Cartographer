namespace Cartographer
{
	using System;
	using System.Linq.Expressions;

	public class MappingContext
	{
		public IMapper Mapper { get; set; }
		public ParameterExpression MapperParameter { get; set; }
		public object SourceInstance { get; set; }
		public ParameterExpression SourceParameter { get; set; }

		public Type SourceType
		{
			get { return SourceInstance.GetType(); }
		}

		public object TargetInstance { get; set; }

		public ParameterExpression TargetParameter { get; set; }
		public Type TargetType { get; set; }

		public Expression ValueParameter { get; set; }
	}
}
