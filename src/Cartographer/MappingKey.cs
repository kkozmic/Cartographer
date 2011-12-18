namespace Cartographer
{
	using System;

	public struct MappingKey
	{
		public MappingKey(Type @from, Type to): this()
		{
			Source = @from;
			Target = to;
		}

		public Type Source { get; private set; }

		public Type Target { get; private set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (obj.GetType() != typeof (MappingKey))
			{
				return false;
			}
			var other = (MappingKey)obj;
			return other.Source == Source && other.Target == Target;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Source.GetHashCode()*397) ^ Target.GetHashCode();
			}
		}

		public override string ToString()
		{
			return string.Format("{0} -> {1}", Source, Target);
		}
	}
}