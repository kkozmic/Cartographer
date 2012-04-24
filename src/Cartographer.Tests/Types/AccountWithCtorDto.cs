namespace CartographerTests.Types
{
	public class AccountWithCtorDto
	{
		public AccountWithCtorDto(int ownerId)
		{
			OwnerId = ownerId;
		}

		public string Number { get; set; }

		public int OwnerId { get; set; }
	}
}