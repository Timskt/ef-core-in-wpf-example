using System;

namespace Base.Contracts.Domain
{
    /// <summary>
    ///     Default int based Domain Entity interface
    /// </summary>
    public interface IDomainEntityId : IDomainEntityId<int>
    {
    }

    /// <summary>
    ///     Universal Domain Entity interface based on generic PK type
    /// </summary>
    /// <typeparam name="TKey">Type for primary key</typeparam>
    public interface IDomainEntityId<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}