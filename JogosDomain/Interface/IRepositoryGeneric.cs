namespace Jogos.Service.Domain.Interface
{
    public interface IRepositoryGeneric<TEntity> where TEntity : class
    {
        Task<List<TEntity>> Listar();
        Task<bool> Adicionar(TEntity jogo);
        Task<bool> Atualizar(TEntity jogo);

    }
}
