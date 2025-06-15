using ESW.DDD.WorkshopResults.Domain.Common.Abstractions;

namespace ESW.DDD.WorkshopResults.Domain.Common;

internal record Email : IValueObject
{
    public static Email Create(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        if (!IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }

        return new(email);
    }

    public string Value { get; }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private Email(string value)
    {
        Value = value;
    }
}
