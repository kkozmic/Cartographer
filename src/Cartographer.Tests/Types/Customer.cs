namespace CartographerTests.Types
{
	using System;

	public class Customer: IHasId
	{
		public int ItemId { get; set; }

		public DateTime LastModified { get; set; }
	}
}