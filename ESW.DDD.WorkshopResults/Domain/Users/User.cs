using ESW.DDD.WorkshopResults.Domain.Common.Abstractions;

namespace ESW.DDD.WorkshopResults.Domain.Users;

internal class User : IAggregateRoot
{
    public static User Create(
        UserId id,
        string name,
        Email email,
        UserTypes type)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new(
            id: id,
            name: name,
            email: email,
            type: type,
            isActive: true);
    }

    public UserId Id { get; }
    public string Name { get; }
    public Email Email { get; }
    public UserTypes Type { get; }
    public bool IsActive { get; private set; }

    public void Disable() => IsActive = false;

    private User(
        UserId id,
        string name,
        Email email,
        UserTypes type,
        bool isActive)
    {
        Id = id;
        Name = name;
        Email = email;
        Type = type;
        IsActive = isActive;
    }
}
