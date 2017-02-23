using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using Nancy;

using Intro.Application.Validation;
using Intro.Domain.PersistentModels;

using ServiceStack;

namespace Intro.Web.Serialization
{
    public class ValidationResult<T>
        where T : IEntity
    {
        public IntroValidationResult Validation { get; set; } 
        public EntityResult<T> Data { get; set; }
    }

    public class EntityResult<T> : DynamicObject,
                                   IEquatable<EntityResult<T>>,
                                   IHideObjectMembers,
                                   IEnumerable<string>,
                                   IDictionary<string, object>
        where T : IEntity
    {
        private readonly IDictionary<string, dynamic> dictionary =
            new Dictionary<string, dynamic>(
                StaticConfiguration.CaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);

        private readonly string typeName;

        public EntityResult()
        {
            this["entities"] = new object[0];
        }

        public EntityResult(T entity)
        {
            this.typeName = entity == null ? GetGenericTypGetTypeNameAsCamelCase() : GetEntityTypeAsCamelCase(entity);

            if (entity != null)
            {
                this[typeName] = new[] { entity };
            }
            else
            {
                this[typeName] = new object[0];
            }
        }

        public EntityResult(IEnumerable<T> entities)
        {
            this.typeName = GetGenericTypGetTypeNameAsCamelCase();

            this[typeName] = entities.Any() ? (dynamic) entities : new List<Entity>();
        }

        private static string GetEntityTypeAsCamelCase(T entity)
        {
            return entity.GetType().Name.ToCamelCase();
        }

        private static string GetGenericTypGetTypeNameAsCamelCase()
        {
            return typeof(T).Name.ToCamelCase();
        }

        //        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        //            => new[] { new KeyValuePair<string, object>(this.typeName, this.entities) }.AsEnumerable().GetEnumerator();


        public static EntityResult<T> Empty => new EntityResult<T>();

        public static EntityResult<T> Create(IDictionary<string, object> values)
        {
            var instance = new EntityResult<T>();

            foreach (var key in values.Keys)
            {
                instance[key] = values[key];
            }

            return instance;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!dictionary.TryGetValue(binder.Name, out result))
            {
                result = null;
            }

            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames() => this.dictionary.Keys;

        public IEnumerator<string> GetEnumerator() => this.dictionary.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.dictionary.Keys.GetEnumerator();

        public dynamic this[string name]
        {
            get
            {
                name = GetNeutralKey(name);

                dynamic member;
                if (!dictionary.TryGetValue(name, out member))
                {
                    member = null;
                }

                return member;
            }
            set
            {
                name = GetNeutralKey(name);

                dictionary[name] = value;
            }
        }

        public bool Equals(EntityResult<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || Equals(other.dictionary, this.dictionary);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == typeof(EntityResult<T>) && this.Equals((EntityResult<T>)obj);
        }

        IEnumerator<KeyValuePair<string, dynamic>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return dictionary?.GetHashCode() ?? 0;
        }

        public void Add(string key, dynamic value)
        {
            this[key] = value;
        }

        public void Add(KeyValuePair<string, dynamic> item)
        {
            this[item.Key] = item.Value;
        }

        public bool ContainsKey(string key)
        {
            key = GetNeutralKey(key);
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        public bool TryGetValue(string key, out dynamic value)
        {
            key = GetNeutralKey(key);
            return this.dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        public bool Contains(KeyValuePair<string, dynamic> item)
        {
            var dynamicValueKeyValuePair = GetDynamicKeyValuePair(item);

            return this.dictionary.Contains(dynamicValueKeyValuePair);
        }

        public void CopyTo(KeyValuePair<string, dynamic>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(string key)
        {
            key = GetNeutralKey(key);
            return this.dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<string, dynamic> item)
        {
            var dynamicValueKeyValuePair = GetDynamicKeyValuePair(item);

            return this.dictionary.Remove(dynamicValueKeyValuePair);
        }

        public ICollection<dynamic> Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }

        private static KeyValuePair<string, dynamic> GetDynamicKeyValuePair(KeyValuePair<string, dynamic> item)
        {
            var dynamicValueKeyValuePair = new KeyValuePair<string, dynamic>(item.Key, item.Value);
            return dynamicValueKeyValuePair;
        }

        private static string GetNeutralKey(string key)
        {
            return key.Replace("-", string.Empty);
        }

        public Dictionary<string, object> ToDictionary()
        {
            var data = new Dictionary<string, object>();

            foreach (var item in dictionary)
            {
                var newKey = item.Key;
                var newValue = item.Value;

                data.Add(newKey, newValue);
            }

            return data;
        }
    }
}
