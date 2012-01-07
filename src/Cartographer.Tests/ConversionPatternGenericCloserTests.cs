namespace CartographerTests
{
	using Cartographer.Compiler;
	using CartographerTests.ConversionPatterns;
	using Xunit;

	public class ConversionPatternGenericCloserTests
	{
		readonly ConversionPatternGenericCloser closer;

		public ConversionPatternGenericCloserTests()
		{
			closer = new ConversionPatternGenericCloser();
		}

		[Fact]
		public void Doesnt_cloes_non_generic_types()
		{
			var conversionPatternType = typeof (NonGenericConversionPattern);
			var closedType = closer.Close(conversionPatternType, typeof (decimal), typeof (decimal));
			Assert.Equal(conversionPatternType, closedType);
		}

		[Fact]
		public void Returns_null_if_closed_types_doesnt_match_parameter_types()
		{
			var conversionPatternType = typeof(NonGenericConversionPattern);
			var closedType = closer.Close(conversionPatternType, typeof(string), typeof(decimal));
			Assert.Null(closedType);
		}
	}
}