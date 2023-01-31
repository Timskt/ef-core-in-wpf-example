using System;
using Base.Contracts.Domain;

namespace Base.Domain
{
    public abstract class DomainEntityId : DomainEntityId<int>, IDomainEntityId
    {
    }

    public abstract class DomainEntityId<TKey> : IDomainEntityId<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}