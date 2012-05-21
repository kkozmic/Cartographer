namespace CartographerTests.Internal
{
	using System;
	using Cartographer;
	using Cartographer.Compiler;

	//internal so that it doesn't get selected automatically
	class DelegatingCatalog: IMappingCatalog
	{
		readonly Action<IIMappingBag> collect;

		public DelegatingCatalog(Action<IIMappingBag> collect)
		{
			this.collect = collect;
		}

		public void Collect(IIMappingBag mappings)
		{
			collect(mappings);
		}
	}
}