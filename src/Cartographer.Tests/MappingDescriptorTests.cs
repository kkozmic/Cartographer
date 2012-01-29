namespace CartographerTests
{
	using System;
	using System.IO;
	using System.Text;
	using Cartographer;
	using CartographerTests.Types;
	using Xunit;

	public class MappingDescriptorTests: IDisposable
	{
		readonly IMapper mapper;

		readonly StringBuilder output;

		public MappingDescriptorTests()
		{
			output = new StringBuilder();
			var builder = new MapperBuilder();
			builder.Settings.MappingDescriptorWriter = new StringWriter(output);
			mapper = builder.BuildMapper();
		}

		[Fact]
		public void Can_describe_flattened_mapping()
		{
			mapper.Convert<AccountDto>(new Account { Number = "abc123", Owner = new Person { Id = 42 } });
		}

		public void Dispose()
		{
			//XUnit not yet supported in
			//Approvals.Approve(output.ToString());
		}
	}
}