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
			if (interfaceSourceType.ContainsGenericParameters)
			{
				// for now we only allow that if it's a generic argument of the sourceType
				var index = Array.IndexOf(openClassArguments, interfaceSourceType);
				if (index == -1)
				{
					if (interfaceSourceType.IsGenericType && sourceType.IsGenericType)
					{
						var openInterfaceSource = interfaceSourceType.GetGenericTypeDefinition();
						var openSource = sourceType.GetGenericTypeDefinition();
						// we don't support assignable types for now
						if (openInterfaceSource != openSource)
						{
							return null;
						}
						var interfaceSourceArguments = interfaceSourceType.GetGenericArguments();
						var sourceArguments = sourceType.GetGenericArguments();
						var sourceParameters = new Type[sourceArguments.Length];
						for (var i = 0; i < sourceParameters.Length; i++)
						{
							if (interfaceSourceArguments[i].ContainsGenericParameters)
							{
								// for now we only allow that if it's a generic argument of the sourceType
								var sourceIndex = Array.IndexOf(openClassArguments, interfaceSourceArguments[i]);
								if (sourceIndex == -1)
								{
									return null;
								}
								sourceParameters[sourceIndex] = sourceArguments[i];
							}
						}
					}
					if (interfaceSourceType.IsArray)
					{
						var sourceArrayItemType = sourceType.GetArrayItemType();
						if (sourceArrayItemType != null)
						{
							var interfaceArrayItemType = interfaceSourceType.GetElementType();
							// for now we only allow that if it's a generic argument of the sourceType
							var sourceIndex = Array.IndexOf(openClassArguments, interfaceArrayItemType);
							if (sourceIndex == -1)
							{
								return null;
							}
							parameters[sourceIndex] = sourceArrayItemType;
						}
					}
				}
				else
				{
					parameters[index] = sourceType;
				}
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
					if (interfaceTargetType.IsGenericType && targetType.IsGenericType)
					{
						var openInterfaceTarget = interfaceTargetType.GetGenericTypeDefinition();
						var openTarget = targetType.GetGenericTypeDefinition();
						// we don't support assignable types for now
						if (openInterfaceTarget != openTarget)
						{
							return null;
						}
						var interfaceTargetArguments = interfaceTargetType.GetGenericArguments();
						var targetArguments = targetType.GetGenericArguments();
						var targetParameters = new Type[targetArguments.Length];
						for (var i = 0; i < targetParameters.Length; i++)
						{
							if (interfaceTargetArguments[i].ContainsGenericParameters)
							{
								// for now we only allow that if it's a generic argument of the sourceType
								var targetIndex = Array.IndexOf(openClassArguments, interfaceTargetArguments[i]);
								if (targetIndex == -1)
								{
									return null;
								}
								targetParameters[targetIndex] = targetArguments[i];
							}
						}
					}
					if (interfaceTargetType.IsArray)
					{
						var targetArrayItemType = targetType.GetArrayItemType();
						if (targetArrayItemType != null)
						{
							var interfaceArrayItemType = interfaceTargetType.GetElementType();
							// for now we only allow that if it's a generic argument of the sourceType
							var targetIndex = Array.IndexOf(openClassArguments, interfaceArrayItemType);
							if (targetIndex == -1)
							{
								return null;
							}
							parameters[targetIndex] = targetArrayItemType;
						}
					}
				}
				else
				{
					parameters[index] = targetType;
				}
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