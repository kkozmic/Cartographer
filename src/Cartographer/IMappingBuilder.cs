namespace Cartographer
{
	using System;

	public interface IMappingBuilder
	{
		Delegate BuildMapping(TypeModel source, TypeModel target);
	}
}
