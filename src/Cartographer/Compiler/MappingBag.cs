namespace Cartographer.Compiler
{
	using System.Collections.Generic;

	public class MappingBag: IIMappingBag
	{
		readonly ICollection<MappingInfo> mappings;

		public MappingBag(ICollection<MappingInfo> mappings)
		{
			this.mappings = mappings;
		}

		public IIMappingBag Add(MappingInfo mappingInfo)
		{
			mappings.Add(mappingInfo);
			return this;
		}
	}
}