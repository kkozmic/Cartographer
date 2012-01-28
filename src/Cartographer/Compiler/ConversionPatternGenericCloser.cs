namespace Cartographer.Compiler
{
	using System;
	using Cartographer.Internal;

	public class ConversionPatternGenericCloser: IConversionPatternGenericCloser
	{
		// this has been through red and green phase, it has yet to see it's refactor phase
		public Type Close(Type conversionPatternType, Type sourceType, Type targetType)
		{
			var @interface = conversionPatternType.GetInterface(typeof (IConversionPattern<,>).Name);
			if (@interface == null)
			{
				throw new ArgumentException(string.Format("Type {0} doesn't implement {1} and therefore is invalid for this operation.", conversionPatternType, typeof (IConversionPattern<,>)));
			}
			var arguments = @interface.GetGenericArguments();
			var interfaceSourceType = arguments[0];
			var interfaceTargetType = arguments[1];
			if (conversionPatternType.IsGenericType == false)
			{
				if (sourceType.Is(interfaceSourceType) && targetType.Is(interfaceTargetType))
				{
					return conversionPatternType;
				}
				return null;
			}
			var openClassArguments = conversionPatternType.GetGenericArguments();
			var parameters = new Type[openClassArguments.Length];
			if (TryAddParameters(sourceType, interfaceSourceType, parameters, openClassArguments) == false)
			{
				return null;
			}
			if (TryAddParameters(targetType, interfaceTargetType, parameters, openClassArguments) == false)
			{
				return null;
			}
			if (Array.TrueForAll(parameters, p => p != null))
			{
				return conversionPatternType.MakeGenericType(parameters);
			}
			return null;
		}

		bool TryAddParameters(Type classType, Type interfaceType, Type[] parameters, Type[] openClassArguments)
		{
			if (interfaceType.ContainsGenericParameters == false)
			{
				return classType.Is(interfaceType);
			}

			// for now we only allow that if it's a generic argument of the sourceType
			var index = Array.IndexOf(openClassArguments, interfaceType);
			if (index == -1)
			{
				if (interfaceType.IsGenericType && classType.IsGenericType)
				{
					var openInterfaceType = interfaceType.GetGenericTypeDefinition();
					var openClassType = classType.GetGenericTypeDefinition();
					// we don't support assignable types for now
					if (openInterfaceType != openClassType)
					{
						return false;
					}
					var interfaceGenericArguments = interfaceType.GetGenericArguments();
					var classGenericArguments = classType.GetGenericArguments();
					for (var i = 0; i < interfaceGenericArguments.Length; i++)
					{
						if (interfaceGenericArguments[i].ContainsGenericParameters)
						{
							// for now we only allow that if it's a generic argument of the sourceType
							index = Array.IndexOf(openClassArguments, interfaceGenericArguments[i]);
							if (index == -1)
							{
								return false;
							}
							parameters[index] = classGenericArguments[i];
						}
					}
				}
				if (interfaceType.IsArray)
				{
					var classArrayItemType = classType.GetArrayItemType();
					if (classArrayItemType != null)
					{
						var interfaceArrayItemType = interfaceType.GetElementType();
						// for now we only allow that if it's a generic argument of the sourceType
						index = Array.IndexOf(openClassArguments, interfaceArrayItemType);
						if (index == -1)
						{
							return false;
						}
						parameters[index] = classArrayItemType;
					}
				}
			}
			else
			{
				parameters[index] = classType;
			}
			return true;
		}
	}
}