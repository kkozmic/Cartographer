namespace Cartographer
{
	using System;
	using System.IO;
	using System.Linq.Expressions;

	public class MappingDescriptor: IMappingDescriptor
	{
		readonly TextWriter writer;

		public MappingDescriptor(TextWriter writer)
		{
			this.writer = writer;
		}

		public void DescribeMapping(Type source, Type target)
		{
			writer.WriteLine("Mapping for {0} => {1}", source, target);
		}

		public void DescribeStep(Expression step)
		{
			writer.Write(step.ToString());
			writer.WriteLine();
		}
	}
}