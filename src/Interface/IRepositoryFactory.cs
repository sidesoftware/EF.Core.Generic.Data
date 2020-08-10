
namespace EF.Core.Generic.Data.Interface
{
    public interface IRepositoryFactory
    {
        IRepository<T> Repository<T>() where T : class;
    }
}