using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Sporadic.Abp.Users
{
    public interface IUserRepository<TUser> : IBasicRepository<TUser, Guid>
     where TUser : class, IUser, IAggregateRoot<Guid>
    {
        /// <summary>
        /// 通过email查找用户
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过手机号查找用户
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TUser> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通过用户名查找用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TUser> FindByUserNameAsync(string userName, bool includeDetails, CancellationToken cancellationToken);
    }
}
