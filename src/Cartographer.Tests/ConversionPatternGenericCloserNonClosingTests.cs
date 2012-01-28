namespace CartographerTests
{
	using Cartographer.Compiler;
	using CartographerTests.ConversionPatterns;
	using Xunit;

	public class ConversionPatternGenericCloserNonClosingTests
	{
		readonly ConversionPatternGenericCloser closer;

		public ConversionPatternGenericCloserNonClosingTests()
		{
			closer = new ConversionPatternGenericCloser();
		}


		[Fact]
		public void Bails_out_when_given_class_when_has_constraint_of_struct_on_generic_argument()
		{
			var conversionPatternType = typeof (NullableConversionPattern<>);
			var closedType = closer.Close(conversionPatternType, typeof (string), typeof (string));

			Assert.Null(closedType);

		}
	}
}