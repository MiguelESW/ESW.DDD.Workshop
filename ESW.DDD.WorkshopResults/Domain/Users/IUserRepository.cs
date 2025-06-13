namespace ESW.DDD.WorkshopResults.Domain.Users;

internal interface IUserRepository
{
    public User Get(UserId id);
    public void Upsert(User user);
}
