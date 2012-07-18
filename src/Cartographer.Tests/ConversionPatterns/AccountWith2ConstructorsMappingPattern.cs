namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;
	using CartographerTests.Types;

	public class AccountWith2ConstructorsMappingPattern: IConversionPattern<Account, AccountWith2CtorsDto>
	{
		public Expression<Func<Account, IMapper, MappingContext, AccountWith2CtorsDto>> BuildConversionExpression(MappingStep mapping)
		{
			return (s, m, c) => new AccountWith2CtorsDto(s.Number) { OwnerId = s.Owner.Id };
		}
	}
}