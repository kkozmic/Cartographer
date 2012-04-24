namespace CartographerTests
{
	using CartographerTests.Types;
	using Xunit;

	public class CustomConstructorTests: AbstractMappingTests
	{
		[Fact]
		public void Can_map_using_custom_constructor()
		{
			var dto = mapper.Convert<AccountWithCtorDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });

			Assert.Equal(42, dto.OwnerId);
			Assert.Equal("abc123", dto.Number);
		}
	}
}