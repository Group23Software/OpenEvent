using OpenEvent.Web.Contexts;

namespace OpenEvent.Test.Factories
{
    public interface IServiceFactory<out T>
    {
        T Create(ApplicationContext context);
    }
}