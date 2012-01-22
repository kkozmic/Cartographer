namespace CartographerTests.Types
{
	public class Order2Dto
	{
		public Identifier CustomerIdentifier { get; set; }

		public OrderLine[] OrderLines { get; set; }
	}
}