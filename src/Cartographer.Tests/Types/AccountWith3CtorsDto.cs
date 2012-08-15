namespace CartographerTests.Types
{
	public class AccountWith3CtorsDto
	{
		public AccountWith3CtorsDto(string number)
		{
			Number = number;
		}

		public AccountWith3CtorsDto(int ownerId)
		{
			OwnerId = ownerId;
		}

		public AccountWith3CtorsDto()
		{
		}

		public string Number { get; set; }

		public int OwnerId { get; set; }
	}
}