namespace CartographerTests.Types
{
	using System;

	public interface IHasId
	{
		int ItemId { get; }

		DateTime LastModified { get; }
	}
}