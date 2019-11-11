using MongoDB.Bson;
using Sky.Auth.Data.Dtos;
using Sky.Auth.Domain.Models;
using System.Collections.Generic;

namespace Sky.Auth.Data.Extensions
{
    public static class UserExtensions
    {
        internal static User ToDomain(this UserDto userDto) => new User(userDto.ObjectId.ToString(), userDto.Name, userDto.Email, userDto.Password, userDto.PhoneNumbers.ToDomain());

        internal static IReadOnlyCollection<Phone> ToDomain(this IReadOnlyCollection<PhoneDto> phoneDto)
        {
            var numbers = new List<Phone>();

            foreach(var item in phoneDto)
            {
                numbers.Add(new Phone(item.DDD, item.Number));
            }

            return numbers;
        }

        internal static UserDto ToDto(this User user) => !string.IsNullOrEmpty(user.Id)  ? 
            new UserDto { ObjectId = new ObjectId(user.Id), Name = user.Name, Email = user.Email, Password = user.Password, PhoneNumbers = user.Phones.ToDto() } : 
            new UserDto { Name = user.Name, Email = user.Email, Password = user.Password, PhoneNumbers = user.Phones.ToDto() };

        internal static IReadOnlyCollection<PhoneDto> ToDto (this IReadOnlyCollection<Phone> phone)
        {
            var numbers = new List<PhoneDto>();

            foreach (var item in phone)
            {
                numbers.Add(new PhoneDto { DDD = item.DDD, Number = item.Number });
            }

            return numbers;
        }
    }
}
