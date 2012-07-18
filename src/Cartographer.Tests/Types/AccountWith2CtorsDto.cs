namespace CartographerTests.Types
{
	public class AccountWith2CtorsDto
	{
		public AccountWith2CtorsDto(string number)
		{
			Number = number;
		}

		public AccountWith2CtorsDto(int ownerId)
		{
			OwnerId = ownerId;
		}

		public string Number { get; set; }

		public int OwnerId { get; set; }
	}
}