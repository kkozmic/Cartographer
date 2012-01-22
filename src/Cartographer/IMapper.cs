namespace Cartographer
{
	public interface IMapper
	{
		TTarget Convert<TTarget>(object source);

		TTarget Convert<TTarget>(object source, TTarget target);
	}
}