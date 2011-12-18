namespace Cartographer
{
	public interface IMappingBuilder
	{
		MappingStrategy BuildMappingStrategy(TypeModel source, TypeModel target);
	}
}