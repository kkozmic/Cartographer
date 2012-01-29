namespace CartographerTests.Internal
{
	using ApprovalTests.Core;
	using Xunit;

	public class StringAssertEqualReporter: IApprovalFailureReporter
	{
		public void Report(string approved, string received)
		{
			Assert.Equal(approved, received);
		}
	}
}