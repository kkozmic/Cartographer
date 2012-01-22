namespace CartographerTests
{
	using System;
	using System.Collections.Generic;
	using Cartographer.Compiler;
	using Cartographer.Patterns;
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
		public void Can_close_type_with_array_of_T()
		{
			var conversionPatternType = typeof (CollectionConversionPattern<>);
			var closedType = closer.Close(conversionPatternType, typeof (List<int>), typeof (IEnumerable<string>));
			Assert.Equal(typeof (CollectionConversionPattern<string>), closedType);
		}

		[Fact]
		public void Can_close_type_with_both_paramters_nested_in_generic()
		{
			var conversionPatternType = typeof (DoubleNullableConversionPattern<,>);
			var closedType = closer.Close(conversionPatternType, typeof (Int64?), typeof (DateTime?));
			Assert.Equal(typeof (DoubleNullableConversionPattern<Int64, DateTime>), closedType);
		}

		[Fact]
		public void Can_close_type_with_inverse_order_of_generic_arguments_with_regards_to_the_interface()
		{
			var conversionPatternType = typeof (ConvertConversionPattern<,>);
			var closedType = closer.Close(conversionPatternType, typeof (int), typeof (string));
			Assert.Equal(typeof (ConvertConversionPattern<string, int>), closedType);
		}

		[Fact]
		public void Can_close_type_with_just_target_parameter()
		{
			var conversionPatternType = typeof (MapConversionPattern<>);
			var closedType = closer.Close(conversionPatternType, typeof (DateTime), typeof (string));
			Assert.Equal(typeof (MapConversionPattern<string>), closedType);
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