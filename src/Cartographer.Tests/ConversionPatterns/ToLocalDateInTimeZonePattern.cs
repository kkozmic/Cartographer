namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;

	public class ToLocalDateInTimeZonePattern: IConversionPattern<DateTime, DateTime>
	{
		public Expression<Func<DateTime, IMapper, MappingContext, DateTime>> BuildConversionExpression(MappingStep mapping)
		{
			return (s, m, c) => TimeZoneInfo.ConvertTime(s, c.Argument<TimeZoneInfo>()).Date;
		}
	}
}