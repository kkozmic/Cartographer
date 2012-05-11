namespace CartographerTests.Types
{
	public class Order4
	{
		public Identifier CustomerIdentifier { get; set; }

		public OrderLine[] OrderLines { get; set; }
	}
}