using System;
using System.Collections.Generic;
using Characters.AbilitiesSystem.States;
using MapSystem;

namespace Characters.LibrarySystem
{
    public abstract class Library<TEntity> : MapperBase<Type, Type>
    {
        protected static Library<TEntity> _entityLibraryInstance;
        protected Dictionary<Type, TEntity> _entities;
        protected Dictionary<Type, AbilityBase> _baseStates;

        public Library() : base()
        {
            _entities = new Dictionary<Type, TEntity>();
            _baseStates = new Dictionary<Type, AbilityBase>();
        }
        
        public static Type StaticToEntityType(TEntity entity) => _entityLibraryInstance.ToEntityType(entity.GetType());
        private Type ToEntityType(Type entityType) => _dictionary[entityType];

        private void AddEffectData(Type key, TEntity value)
        {
            if (!_entities.ContainsKey(key))
            {
                _entities.Add(key, value);
            }
        }

        private void AddState(Type key, Type value)
        {
            if (!_baseStates.ContainsKey(key))
            {
                _dictionary.Add(key, value);
            }
        }

        public static void StaticAddEntity(Type key, TEntity value)
        {
            _entityLibraryInstance.AddEffectData(key, value);
        }

        public static void StaticAddState(Type key, Type value)
        {
            _entityLibraryInstance.AddState(key, value);
        }

        
    }
}