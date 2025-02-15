using System;
using System.Collections.Generic;

namespace LogicRoll.Application
{
    public class ViewContainer
    {
        Dictionary<Type, object> viewDict = new();

        // 取得
        public T Get<T>()
        {
            if(viewDict.TryGetValue(typeof(T), out object value))
            {
                return (T)value;
            }

            throw new Exception($"{typeof(T).Name} is not found");
        }

        public IEnumerable<T> GetAll<T>()
        {
            if(viewDict.TryGetValue(typeof(T), out object value))
            {
                return (IEnumerable<T>)value;
            }

            throw new Exception($"{typeof(T).Name} is not found");
        }

        // 登録
        public void Register<T>(T instance)
        {
            if(viewDict.ContainsKey(typeof(T)))
            {
                throw new Exception($"{typeof(T).Name} is already registered");
            }

            viewDict[typeof(T)] = instance;
        }

        public void RegisterAll<T>(IEnumerable<T> instances)
        {
            if(viewDict.ContainsKey(typeof(T)))
            {
                throw new Exception($"{typeof(T).Name} is already registered");
            }

            viewDict[typeof(T)] = instances;
        }
    }
}
