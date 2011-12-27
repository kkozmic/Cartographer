namespace CartographerTests
{
	using Cartographer;
	using CartographerTests.Types;
	using Xunit;

	public class MappingDescriptorTests
	{
		readonly IMapper mapper;

		public MappingDescriptorTests()
		{
			mapper = new MapperBuilder().BuildMapper();
		}

		[Fact]
		public void Can_describe_flattened_mapping()
		{
			mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });
		}
	}
}