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
		public void Can_close_type_with_inverse_order_of_generic_arguments_with_regards_to_the_interface()
		{
			var conversionPatternType = typeof (ConvertConversionPattern<,>);
			var closedType = closer.Close(conversionPatternType, typeof (int), typeof (string));
			Assert.Equal(typeof (ConvertConversionPattern<string, int>), closedType);
		}

		[Fact]
		public void Can_close_type_with_one_generic_parameter()
		{
			var conversionPatternType = typeof (ToStringConversionPattern<>);
			var closedType = closer.Close(conversionPatternType, typeof (decimal), typeof (string));
			Assert.Equal(typeof (ToStringConversionPattern<decimal>), closedType);
		}

		[Fact]
		public void Can_close_type_with_paramter_nested_in_generic()
		{
			var conversionPatternType = typeof (NullableConversionPattern<>);
			var closedType = closer.Close(conversionPatternType, typeof (int?), typeof (int));
			Assert.Equal(typeof (NullableConversionPattern<int>), closedType);
		}

		[Fact]
		public void Can_close_type_with_single_generic_parameter()
		{
			var conversionPatternType = typeof (NoOpConversionPattern<>);
			var closedType = closer.Close(conversionPatternType, typeof (int), typeof (int));
			Assert.Equal(typeof (NoOpConversionPattern<int>), closedType);
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
			var conversionPatternType = typeof (NonGenericConversionPattern);
			var closedType = closer.Close(conversionPatternType, typeof (string), typeof (decimal));
			Assert.Null(closedType);
		}
	}
}