using Sky.Auth.Command.Commands;
using Sky.Auth.Domain.Models;
using System.Collections.Generic;

namespace Sky.Auth.Command.Extensions
{
    public static class UserCommandExtensions
    {
        internal static IReadOnlyCollection<Phone> ToDomain(this IReadOnlyCollection<PhoneNumberCommand> phone)
        {
            var numbers = new List<Phone>();

            foreach (var item in phone)
            {
                numbers.Add(new Phone(item.DDD, item.Number));
            }

            return numbers;
        }
    }
}
