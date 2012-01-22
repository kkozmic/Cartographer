namespace CartographerTests.Types
{
	public class Order2
	{
		public Customer Customer { get; set; }

		public OrderLine[] OrderLines { get; set; }
	}
}