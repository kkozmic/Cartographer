namespace CartographerTests.Types
{
	public class AccountWithAdditionalParameterDto
	{
		public AccountWithAdditionalParameterDto(string number, string language)
		{
			Number = number;
			Language = language;
		}

		public string Number { get; set; }

		public int OwnerId { get; set; }

		public string Language { get; set; }
	}
}