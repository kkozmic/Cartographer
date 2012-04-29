namespace Cartographer.Compiler
{
	using System;

	public sealed class MappingInfo
	{
		readonly bool mapIntoExistingTargetInstance;

		public MappingInfo(Type sourceInstanceType, Type targetConstrtaintType, Type targetInstanceType)
		{
			MappingSourceType = sourceInstanceType;
			TargetConstrtaintType = targetConstrtaintType;
			MappingTargetType = targetInstanceType ?? targetConstrtaintType;
			mapIntoExistingTargetInstance = targetInstanceType != null;
		}

		public bool MapIntoExistingTargetInstance
		{
			get { return mapIntoExistingTargetInstance; }
		}

		public Type MappingSourceType { get; set; }

		public Type MappingTargetType { get; set; }

		public Type TargetConstrtaintType { get; private set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != typeof (MappingInfo))
			{
				return false;
			}
			var other = (MappingInfo)obj;
			return other.MappingSourceType == MappingSourceType &&
			       other.MappingTargetType == MappingTargetType &&
			       other.MapIntoExistingTargetInstance == MapIntoExistingTargetInstance;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = (MappingSourceType != null ? MappingSourceType.GetHashCode() : 0);
				result = (result*397) ^ (MappingTargetType != null ? MappingTargetType.GetHashCode() : 0);
				result = (result*397) ^ (MapIntoExistingTargetInstance.GetHashCode());
				return result;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} -> {1}{2}", MappingSourceType, MappingTargetType, MapIntoExistingTargetInstance ? "*" : string.Empty);
		}
	}
}