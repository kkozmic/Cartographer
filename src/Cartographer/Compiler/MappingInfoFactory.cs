namespace Cartographer.Compiler
{
	public class MappingInfoFactory: IMappingInfoFactory
	{
		readonly IMappingInfoSource[] sources;

		public MappingInfoFactory(params IMappingInfoSource[] sources)
		{
			this.sources = sources;
		}

		public MappingInfo GetMappingInfo(MappingRequest request)
		{
			foreach (var source in sources)
			{
				var info = source.GetMappingInfo(request);
				if (info != null)
				{
					return info;
				}
			}
			// we fallback to default behaviour
			return new MappingInfo(request.ActualSourceType, request.ActualTargetType ?? request.IndicatedTargetType, request.HasPreexistingTargetInstance);
		}
	}
}