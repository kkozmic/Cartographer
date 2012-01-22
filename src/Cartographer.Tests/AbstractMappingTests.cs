namespace CartographerTests
{
	using Cartographer;

	public class AbstractMappingTests
	{
		protected IMapper mapper;

		public AbstractMappingTests()
		{
			mapper = new MapperBuilder().BuildMapper();
		}
	}
}