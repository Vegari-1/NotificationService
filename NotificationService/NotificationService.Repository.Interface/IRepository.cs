namespace NotificationService.Repository.Interface
{
	public interface IRepository<T> where T : class
	{
		T Save(T entity);
		Task<T> SaveAsync(T entity);
		int Delete(T entity);
		Task<int> DeleteAsync(T entity);
		int SaveChanges();
		Task<int> SaveChangesAsync();
	}
}
