using System;
namespace MyAzureAppService.DataAccess.Repositories
{
	public interface ICosmoRepository<T>
		where T: class
	{
		/// <summary>
		/// Obtiene lista de elementos
		/// </summary>
		/// <returns></returns>
		Task<ICollection<T>> ListData();

		/// <summary>
		/// Registra un elemento
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<T> Register(T item, string partitionKey);
	}
}

