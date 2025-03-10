using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities;

namespace Sporadic.Abp.Users
{
    public interface IUser : IAggregateRoot<Guid>
    {
        [CanBeNull]
        string UserName { get; }

        string Name { get; }

        [CanBeNull]
        string Email { get; }

        bool EmailConfirmed { get; }

        [CanBeNull]
        string PhoneNumber { get; }

        bool PhoneNumberConfirmed { get; }

        bool IsActive { get; }
    }
}
