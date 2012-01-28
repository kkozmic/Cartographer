namespace Cartographer.Compiler
{
	using System;
	using System.IO;
	using System.Linq.Expressions;

	public class MappingDescriptor: IMappingDescriptor
	{
		readonly DescriptionVisitor description;

		readonly TextWriter writer;

		public MappingDescriptor(TextWriter writer)
		{
			description = new DescriptionVisitor(writer);
			this.writer = writer;
		}

		public void DescribeMapping(Type source, Type target)
		{
			writer.WriteLine("Mapping for {0} => {1}", source, target);
		}

		public void DescribeStep(Expression step)
		{
			description.Visit(step);
			writer.WriteLine();
		}
	}
}