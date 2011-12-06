namespace Cartographer
{
	using System;
	using System.Linq.Expressions;

	public class MappingContext
	{
		public IMapper Mapper { get; set; }
		public Expression MapperExpression { get; set; }
		public object SourceInstance { get; set; }
		public ParameterExpression SourceExpression { get; set; }

		public Type SourceType
		{
			get { return SourceInstance.GetType(); }
		}

		public object TargetInstance { get; set; }

		public ParameterExpression TargetExpression { get; set; }
		public Type TargetType { get; set; }

		public Expression ValueExpression { get; set; }

		public ParameterExpression ContextExpression { get; set; }
	}
}
