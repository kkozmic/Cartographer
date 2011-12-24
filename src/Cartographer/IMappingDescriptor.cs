namespace Cartographer
{
	using System.Linq.Expressions;

	public interface IMappingDescriptor
	{
		void DescribeMapping(TypeModel source, TypeModel target);

		void DescribeStep(Expression step);
	}
}