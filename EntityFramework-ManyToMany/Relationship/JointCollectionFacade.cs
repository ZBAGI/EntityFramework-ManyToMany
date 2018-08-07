using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityFramework_ManyToMany.Relationship
{
    public class JointCollectionFacade<TOwnerEntity, TEntity, TJoinEntity> : ICollection<TEntity>
        where TJoinEntity : IJointEntity<TEntity>, IJointEntity<TOwnerEntity>, new()
    {
        private readonly TOwnerEntity OwnerEntity;
        private ICollection<TJoinEntity> Collection;

        public ICollection<TJoinEntity> GetCollection()
        {
            if (Collection == null)
                Collection = typeof(TOwnerEntity).GetProperties()
                    .SingleOrDefault(p => p.PropertyType == typeof(ICollection<TJoinEntity>))?
                    .GetValue(OwnerEntity, null) as ICollection<TJoinEntity>;

            if (Collection == null)
                throw new NullReferenceException($"Missing ICollection<{typeof(TJoinEntity).FullName}> " +
                    $"in entity type {typeof(TOwnerEntity).FullName} or collection haven't been initalized.");

            return Collection;
        }

        public JointCollectionFacade(TOwnerEntity ownerEntity)
        {
            OwnerEntity = ownerEntity;
        }

        public IEnumerator<TEntity> GetEnumerator()
            => GetCollection().Select(e => ((IJointEntity<TEntity>)e).Navigation).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void Add(TEntity item)
        {
            var entity = new TJoinEntity();
            ((IJointEntity<TEntity>)entity).Navigation = item;
            ((IJointEntity<TOwnerEntity>)entity).Navigation = OwnerEntity;
            GetCollection().Add(entity);
        }

        public void Clear()
            => GetCollection().Clear();

        public bool Contains(TEntity item)
            => GetCollection().Any(e => Equals(item, e));

        public void CopyTo(TEntity[] array, int arrayIndex)
            => this.ToList().CopyTo(array, arrayIndex);

        public bool Remove(TEntity item)
            => GetCollection().Remove(
                GetCollection().FirstOrDefault(e => Equals(item, e)));

        public int Count
            => GetCollection().Count;

        public bool IsReadOnly
            => GetCollection().IsReadOnly;

        private static bool Equals(TEntity item, TJoinEntity e)
            => Equals(((IJointEntity<TEntity>)e).Navigation, item);
    }
}
