namespace Cartographer.Compiler
{
	using System;

	public class MappingInfo
	{
		public MappingInfo(Type source, Type target, bool preexistingTargetInstance)
		{
			Source = source;
			Target = target;
			PreexistingTargetInstance = preexistingTargetInstance;
		}

		public bool PreexistingTargetInstance { get; private set; }

		public Type Source { get; private set; }

		public Type Target { get; private set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (obj.GetType() != typeof (MappingInfo))
			{
				return false;
			}
			var other = (MappingInfo)obj;
			return other.Source == Source && other.Target == Target && other.PreexistingTargetInstance == PreexistingTargetInstance;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = Source.GetHashCode();
				result = (result*397) ^ Target.GetHashCode();
				result = (result*397) ^ PreexistingTargetInstance.GetHashCode();
				return result;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} -> {1}{2}", Source, Target, PreexistingTargetInstance ? "*" : string.Empty);
		}
	}
}