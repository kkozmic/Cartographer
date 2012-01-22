namespace CartographerTests.Types
{
	using System;

	public class Identifier
	{
		public Identifier(int id, DateTime timestamp)
		{
			Timestamp = timestamp;
			Id = id;
		}

		public int Id { get; private set; }

		public DateTime Timestamp { get; private set; }
	}
}