namespace CartographerTests
{
	using CartographerTests.Types;
	using Xunit;

	public class CollectionMappingTests: AbstractMappingTests
	{
		[Fact]
		public void Can_properly_map_collection_to_a_new_collection()
		{
			var dtos = mapper.Convert<OrderLineDto[]>(new[]
			                                          {
			                                          	new OrderLine { ItemId = 1, ItemName = "1" }, new OrderLine { ItemId = 2, ItemName = "2" },
			                                          });

			Assert.Equal(2, dtos.Length);
			Assert.Equal(1, dtos[0].ItemId);
			Assert.Equal("2", dtos[1].ItemName);
		}
	}
}