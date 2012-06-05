namespace Cartographer
{
	public interface IMapper
	{
		TTarget Convert<TTarget>(object sourceInstance);

		TTarget Convert<TTarget>(object sourceInstance, TTarget targetInstance);

		TTarget ConvertWithArguments<TTarget>(object sourceInstance, object inlineArgumentsAsAnonymousType);

		TTarget ConvertWithArguments<TTarget>(object sourceInstance, TTarget targetInstance, object inlineArgumentsAsAnonymousType);
	}
}