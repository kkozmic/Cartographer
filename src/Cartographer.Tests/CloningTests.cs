namespace CartographerTests
{
	using System;
	using CartographerTests.Types;
	using Xunit;

	public class CloningTests: AbstractMappingTests
	{
		[Fact]
		public void Can_map_type_with_property_of_type_shared_between_left_and_right()
		{
			var source = new Order4
			             {
			             	CustomerIdentifier = new Identifier(1, DateTime.Now),
			             	OrderLines = new[]
			             	             {
			             	             	new OrderLine { ItemId = 1, ItemName = "1" },
			             	             	new OrderLine { ItemId = 2, ItemName = "2" },
			             	             	new OrderLine { ItemId = 3, ItemName = "3" },
			             	             }
			             };
			var dto = mapper.Convert<Order2Dto>(source);

			Assert.Same(source.CustomerIdentifier, dto.CustomerIdentifier);
		}
	}
}