namespace CartographerTests
{
	using System;
	using Cartographer;
	using CartographerTests.Types;
	using Xunit;

	public class MappingDescriptorTests
	{
		readonly Mapper mapper;

		public MappingDescriptorTests()
		{
			mapper = new Mapper(
				new TypeMapper(),
				new MappingBuilder(new MappingDescriptor(Console.Out),
				                   new IConversionPattern[] { new MapConversionPattern(), new CollectionConversionPattern(), },
				                   new MatchByNameMappingPattern(), new MatchByNameFlattenMappingPattern()),
				new MappingCompiler());
		}

		[Fact]
		public void Can_describe_flattened_mapping()
		{
			mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });
		}
	}
}