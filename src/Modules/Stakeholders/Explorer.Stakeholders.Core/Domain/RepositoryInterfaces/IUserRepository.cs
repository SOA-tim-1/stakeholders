using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    bool Exists(string username);
    PagedResult<User> GetPaged(int page, int pageSize);
    User? GetActiveByName(string username);
    User Create(User user);
    long GetPersonId(long userId);
    User GetUser(long id);
    User GetById(long id);
}