namespace CartographerTests
{
	using System;
	using Cartographer.Compiler;
	using Cartographer.Patterns;
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
			AssertDoesntClose(typeof (NullableConversionPattern<>), typeof (string), typeof (string));
		}

		[Fact]
		public void Bails_out_when_given_two_non_nullables_when_one_argument_is_nullable_value_type()
		{
			AssertDoesntClose(typeof (NullableConversionPattern<>), typeof (int), typeof (int));
		}

		void AssertDoesntClose(Type conversionPatternType, Type sourceType, Type targetType)
		{
			var closedType = closer.Close(conversionPatternType, sourceType, targetType);

			Assert.Null(closedType);
		}
	}
}