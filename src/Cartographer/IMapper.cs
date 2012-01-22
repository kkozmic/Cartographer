namespace Cartographer
{
	public interface IMapper
	{
		TTarget Convert<TTarget>(object source);

		TTarget Convert<TTarget>(object source, TTarget target);

		TTarget ConvertWithArguments<TTarget>(object source, object inlineArgumentsAsAnonymousType);

		TTarget ConvertWithArguments<TTarget>(object source, TTarget target, object inlineArgumentsAsAnonymousType);
	}
}