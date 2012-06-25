namespace Cartographer.Internal
{
	using System;
	using System.Collections.Generic;

	public class Arguments
	{
		readonly IDictionary<Type, object> argumentsByType = new Dictionary<Type, object>();

		public Arguments(object values)
		{
			if (values != null)
			{
				var properties = values.GetType().GetProperties();
				foreach (var property in properties)
				{
					if (property.CanRead == false)
					{
						continue;
					}

					if (argumentsByType.ContainsKey(property.PropertyType) == false)
					{
						argumentsByType[property.PropertyType] = property.GetValue(values, null);
					}
				}
			}
		}

		public object GetByType(Type type)
		{
			object value;
			if (argumentsByType.TryGetValue(type, out value))
			{
				return value;
			}
			throw new ArgumentException(string.Format("No argument of type {0} was found.", type));
		}


		public bool TryGetByType(Type type, out object value)
		{
			return argumentsByType.TryGetValue(type, out value);
		}
	}
}