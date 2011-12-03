namespace Cartographer
{
	using System;

	public class MappingBuilder: IMappingBuilder
	{
		public Delegate BuildMapping(TypeModel source, TypeModel target)
		{
			return new Func<object, MappingContext, dynamic>(Map);
		}

		private dynamic Map(object source, MappingContext context)
		{
			var sourceType = source.GetType();
			var targetType = context.TargetType;

			var target = Activator.CreateInstance(targetType);

			foreach (var targetProperty in targetType.GetProperties())
			{
				var sourceProperty = sourceType.GetProperty(targetProperty.Name);
				if( sourceProperty == null) continue;

				var value = sourceProperty.GetValue(source, null);
				targetProperty.SetValue(target, value, null);
			}
			return target;
		}
	}
}