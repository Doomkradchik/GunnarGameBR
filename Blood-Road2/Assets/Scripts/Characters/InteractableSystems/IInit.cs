using System;

namespace Characters.InteractableSystems
{
    public interface IInit<in T> where T: Delegate
    {
        public void Subscribe(T subscriber);
        public void Unsubscribe(T unsubscriber);
    }
}