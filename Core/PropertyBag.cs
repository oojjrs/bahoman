using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    sealed public class PropertyBag
    {
        // Note : 값의 순서를 보존하기 위해 사전 대신 리스트 사용
        private List<KeyValuePair<Type, List<KeyValuePair<string, object>>>> containers = new List<KeyValuePair<Type, List<KeyValuePair<string, object>>>>();

        public override string ToString()
        {
            var ret = new StringBuilder("\r\n");
            foreach (var c in containers)
            {
                ret.AppendFormat("Type : {0}\r\n", c.Key);
                foreach (var pair in c.Value)
                    ret.AppendFormat("    {0} : {1}\r\n", pair.Key, pair.Value);
            }
            return ret.ToString();
        }

        public void CopyTo(PropertyBag props)
        {
            foreach (var container in containers)
            {
                var propsContainer = props.GetContainer(container.Key);
                if (propsContainer == null)
                {
                    propsContainer = new List<KeyValuePair<string, object>>();
                    props.containers.Add(new KeyValuePair<Type, List<KeyValuePair<string, object>>>(container.Key, propsContainer));
                }

                foreach (var c in container.Value)
                    propsContainer.Add(new KeyValuePair<string, object>(c.Key, c.Value));
            }
        }

        public void AddValue<T>(string key, T value)
        {
            var container = this.GetContainer(typeof(T));
            if (container == null)
            {
                container = new List<KeyValuePair<string, object>>();
                containers.Add(new KeyValuePair<Type, List<KeyValuePair<string, object>>>(typeof(T), container));
            }
            container.Add(new KeyValuePair<string, object>(key, value));
        }

        private List<KeyValuePair<string, object>> GetContainer(Type t)
        {
            foreach (var c in containers)
            {
                if (c.Key == t)
                    return c.Value;
            }
            return null;
        }

        public float GetFactor(string key, bool raiseException = true)
        {
            float value = 0.0f;
            this.GetValue(key, ref value, raiseException);
            return value;
        }

        public bool GetValue<T>(string key, ref T value, bool raiseException = true)
        {
            T[] values;
            if (this.GetValues<T>(key, out values, raiseException) == false)
            {
                if (raiseException)
                    throw new Exception(key + " 팩터가 필요합니다");
                return false;
            }

            value = values.First();
            return true;
        }

        public bool GetValues<T>(string key, out T[] values, bool raiseException = true)
        {
            var container = this.GetContainer(typeof(T));
            if (container == null)
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

        public bool HasKey<T>(string key)
        {
            var container = this.GetContainer(typeof(T));
            if (container != null)
            {
                var ret = from entry in container where entry.Key == key select (T)entry.Value;
                if (ret.Count() > 0)
                    return true;
            }
            return false;
        }

        public bool IsFlagOn(string key)
        {
            bool isOn = false;
            return this.GetValue(key, ref isOn, false) && isOn;
        }
    }
}
