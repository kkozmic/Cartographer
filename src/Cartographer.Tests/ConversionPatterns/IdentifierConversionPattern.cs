namespace CartographerTests.ConversionPatterns
{
	using System;
	using System.Linq.Expressions;
	using Cartographer;
	using Cartographer.Compiler;
	using Cartographer.Steps;
	using CartographerTests.Types;

	public class IdentifierConversionPattern: IConversionPattern<IHasId, Identifier>
	{
		public Expression<Func<IHasId, IMapper, MappingContext, Identifier>> BuildConversionExpression(MappingStep mapping)
		{
			return (s, m, c) => new Identifier(s.ItemId, s.LastModified);
		}
	}
}