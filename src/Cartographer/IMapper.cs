namespace Cartographer
{
	public interface IMapper
	{
		TResult Convert<TResult>(object source);
	}
}
