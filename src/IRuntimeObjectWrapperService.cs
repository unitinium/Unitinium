namespace Unitinium
{
    public interface IRuntimeObjectWrapperService
    {
        object Wrap(object value, bool isDeep = true, bool needChild = true);
    }
}