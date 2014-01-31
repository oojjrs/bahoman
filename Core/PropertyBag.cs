using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    sealed public class PropertyBag
    {
        private Dictionary<Type, List<KeyValuePair<string, object>>> containers = new Dictionary<Type, List<KeyValuePair<string, object>>>();
        private List<KeyValuePair<string, Vector3f>> vectors = new List<KeyValuePair<string, Vector3f>>();

        public override string ToString()
        {
            return base.ToString();
        }

        public void AddPrimitive<T>(string key, T value)
        {
            List<KeyValuePair<string, object>> container;
            if (containers.TryGetValue(typeof(T), out container) == false)
            {
                container = new List<KeyValuePair<string, object>>();
                containers[typeof(T)] = container;
            }

            container.Add(new KeyValuePair<string, object>(key, value));
        }

        public void AddVector(string key, Vector3f value)
        {
            vectors.Add(new KeyValuePair<string, Vector3f>(key, value));
        }

        public bool GetPrimitive<T>(string key, ref T value, bool raiseException = true)
        {
            T[] values;
            if (this.GetPrimitives<T>(key, out values, raiseException) == false)
            {
                if (raiseException)
                    throw new Exception(key + " 팩터가 필요합니다");
                return false;
            }

            value = values.First();
            return true;
        }

        public bool GetPrimitives<T>(string key, out T[] values, bool raiseException = true)
        {
            List<KeyValuePair<string, object>> container;
            if (containers.TryGetValue(typeof(T), out container) == false)
            {
                if (raiseException)
                    throw new Exception(typeof(T) + " 타입 팩터가 입력되지 않았습니다");

                values = null;
                return false;
            }

            var ret = from entry in container where entry.Key == key select (T)entry.Value;
            if (ret.Count() == 0)
            {
                if (raiseException)
                    throw new Exception(String.Format("{0} 타입 {1} 팩터가 0개입니다", typeof(T), key));

                values = null;
                return false;
            }

            values = ret.ToArray();
            return true;
        }

        public bool GetVector(string key, out Vector3f value, bool raiseException = true)
        {
            Vector3f[] values;
            if (this.GetVectors(key, out values, raiseException) == false)
            {
                if (raiseException)
                    throw new Exception(key + " 팩터가 필요합니다");

                value = null;
                return false;
            }

            value = values.First();
            return true;
        }

        public bool GetVectors(string key, out Vector3f[] values, bool raiseException = true)
        {
            var ret = from entry in vectors where entry.Key == key select entry.Value;
            if (ret.Count() == 0)
            {
                if (raiseException)
                    throw new Exception(key + " 팩터가 필요합니다");

                values = null;
                return false;
            }

            values = ret.ToArray();
            return true;
        }

        public bool IsFlagOn(string key)
        {
            bool isOn = false;
            return this.GetPrimitive(key, ref isOn, false) && isOn;
        }
    }
}
