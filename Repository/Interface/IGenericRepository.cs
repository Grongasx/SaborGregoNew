namespace saborGregoNew.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T? ReadById(int id);
        IEnumerable<T> ReadAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}