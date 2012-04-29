namespace Cartographer.Compiler
{
	using System;

	public class MappingRequest
	{
		public MappingRequest(Type actualSourceType, Type indicatedTargetType, Type actualTargetType)
		{
			ActualSourceType = actualSourceType;
			IndicatedTargetType = indicatedTargetType;
			ActualTargetType = actualTargetType;
		}

		public Type ActualSourceType { get; private set; }

		public Type ActualTargetType { get; private set; }

		public bool HasPreexistingTargetInstance
		{
			get { return ActualTargetType != null; }
		}

		/// <summary>
		///   TODO: This is not exactly the nest name. Instead of 'indocated' it should be
		/// </summary>
		public Type IndicatedTargetType { get; private set; }
	}
}