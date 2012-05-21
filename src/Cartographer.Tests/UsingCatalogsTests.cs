namespace CartographerTests
{
	using Cartographer;
	using Cartographer.Compiler;
	using CartographerTests.Internal;
	using CartographerTests.Types;
	using Xunit;

	public class UsingCatalogsTests
	{
		[Fact]
		public void Can_register_mappings_via_catalog()
		{
			var builder = new MapperBuilder();
			builder.AddCatalogs(new DelegatingCatalog(b => b.Add(new MappingInfo(typeof (Account), typeof (AccountDto), null))));
			var mapper = builder.BuildMapper();

			mapper.Convert<AccountDto>(new Account { Number = "number", Owner = new Person { Id = 3 } });
		}
	}
}