namespace Cartographer
{
	using System.IO;
	using System.Linq.Expressions;

	public class MappingDescriptor: IMappingDescriptor
	{
		readonly TextWriter writer;

		public MappingDescriptor(TextWriter writer)
		{
			this.writer = writer;
		}

		public void DescribeMapping(TypeModel source, TypeModel target)
		{
			writer.WriteLine("Mapping for {0} => {1}", source.Type, target.Type);
		}

		public void DescribeStep(Expression step)
		{
			writer.Write(step.ToString());
			writer.WriteLine();
		}
	}
}