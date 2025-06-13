using ESW.DDD.WorkshopResults.Domain.Common.Abstractions;

namespace ESW.DDD.WorkshopResults.Domain.Users;

internal record UserId : IValueObject
{
    public static UserId Create(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));
        }

        return new(userId);
    }
    public Guid Id { get; set; }

    private UserId(Guid id) => Id = id;
}
