namespace Cartographer.Compiler
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using Cartographer.Internal.Extensions;

	public class ConversionPatternGenericCloser: IConversionPatternGenericCloser
	{
		// this has been through red and green phase, it has yet to see it's refactor phase
		public Type Close(Type conversionPatternType, Type sourceType, Type targetType)
		{
			var @interface = conversionPatternType.GetInterface(typeof (IConversionPattern<,>));
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
			if (parameters.Any(p => p == null))
			{
				return null;
			}
			return conversionPatternType.MakeGenericType(parameters);
		}

		Type EnsureMeetsGenericConstraints(Type type, Type genericParameter)
		{
			Debug.Assert(genericParameter.IsGenericParameter, "genericParameter must really be a generic parameter.");
			var genericConstraints = genericParameter.GenericParameterAttributes;
			if (genericConstraints == GenericParameterAttributes.None)
			{
				return type;
			}
			if (genericConstraints.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
			{
				if (IsNotNullableValueType(type) == false)
				{
					return null;
				}
			}
			else
			{
				// no need to check for default .ctor when we have value type, since we know it'll have one anyway
				if (genericConstraints.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
				{
					if (HasDefaultConstructor(type) == false)
					{
						return null;
					}
				}
				if (genericConstraints.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
				{
					if (IsReferenceType(type) == false)
					{
						return null;
					}
				}
			}
			return type;
		}

		bool HasDefaultConstructor(Type type)
		{
			if (type.IsValueType)
			{
				return true;
			}

			var defaultConstructor = type.GetConstructor(TypeExtensions.EmptyTypes);
			return defaultConstructor != null;
		}

		bool IsNotNullableValueType(Type type)
		{
			if (type.IsValueType == false)
			{
				return false;
			}
			if (type.IsGenericType == false)
			{
				return true;
			}
			var definition = type.GetGenericTypeDefinition();
			return definition != typeof (Nullable<>);
		}

		bool IsReferenceType(Type type)
		{
			return type.IsValueType == false;
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
				if (interfaceType.IsGenericType)
				{
					if (classType.IsGenericType == false)
					{
						return false;
					}
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
							parameters[index] = EnsureMeetsGenericConstraints(classGenericArguments[i], interfaceGenericArguments[i]);
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
						parameters[index] = EnsureMeetsGenericConstraints(classArrayItemType, interfaceArrayItemType);
					}
				}
			}
			else
			{
				parameters[index] = EnsureMeetsGenericConstraints(classType, interfaceType);
			}
			return true;
		}
	}
}