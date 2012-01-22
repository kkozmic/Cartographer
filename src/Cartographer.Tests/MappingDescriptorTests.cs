namespace CartographerTests
{
	using CartographerTests.Types;
	using Xunit;

	public class MappingDescriptorTests: AbstractMappingTests
	{
		[Fact]
		public void Can_describe_flattened_mapping()
		{
			mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });
		}
	}
}