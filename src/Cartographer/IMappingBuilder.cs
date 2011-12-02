using System;

namespace Cartographer
{
	public interface IMappingBuilder

	{
		Delegate BuildMapping(TypeModel source, TypeModel target);
	}
}