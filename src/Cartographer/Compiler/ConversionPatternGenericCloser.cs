namespace Cartographer.Compiler
{
	using System;
	using Cartographer.Internal;

	public class ConversionPatternGenericCloser: IConversionPatternGenericCloser
	{
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
			if (interfaceSourceType.ContainsGenericParameters)
			{
				// for now we only allow that if it's a generic argument of the sourceType
				var index = Array.IndexOf(openClassArguments, interfaceSourceType);
				if (index == -1)
				{
					return null;
				}
				parameters[index] = sourceType;
			}
			else
			{
				if (sourceType.Is(interfaceSourceType) == false)
				{
					return null;
				}
			}
			if (interfaceTargetType.ContainsGenericParameters)
			{
				// for now we only allow that if it's a generic argument of the sourceType
				var index = Array.IndexOf(openClassArguments, interfaceTargetType);
				if (index == -1)
				{
					return null;
				}
				parameters[index] = targetType;
			}
			else
			{
				if (targetType.Is(interfaceTargetType) == false)
				{
					return null;
				}
			}
			if (Array.TrueForAll(parameters, p => p != null))
			{
				return conversionPatternType.MakeGenericType(parameters);
			}
			return null;
		}
	}
}