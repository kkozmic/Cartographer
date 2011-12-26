namespace Cartographer
{
	using System;
	using System.Linq.Expressions;

	public interface IMappingDescriptor
	{
		void DescribeMapping(Type source, Type target);

		void DescribeStep(Expression step);
	}
}