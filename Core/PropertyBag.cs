using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class PropertyBag
    {
        private List<KeyValuePair<string, float>> floats = new List<KeyValuePair<string, float>>();
        private List<KeyValuePair<string, Vector3f>> vectors = new List<KeyValuePair<string, Vector3f>>();

        public void AddFloat(string key, float value)
        {
            floats.Add(new KeyValuePair<string, float>(key, value));
        }

        public void AddVector(string key, Vector3f value)
        {
            vectors.Add(new KeyValuePair<string, Vector3f>(key, value));
        }

        public bool GetFloat(string key, ref float value)
        {
            var ret = this.GetFloats(key);
            if (ret == null)
                return false;

            value = ret.First();
            return true;
        }

        public float[] GetFloats(string key)
        {
            var ret = from entry in floats where entry.Key == key select entry.Value;
            if (ret.Count() == 0)
                return null;
            return ret.ToArray();
        }

        public bool GetVector(string key, out Vector3f value)
        {
            var ret = this.GetVectors(key);
            if (ret == null)
            {
                value = null;
                return false;
            }

            value = ret.First();
            return true;
        }

        public Vector3f[] GetVectors(string key)
        {
            var ret = from entry in vectors where entry.Key == key select entry.Value;
            if (ret.Count() == 0)
                return null;
            return ret.ToArray();
        }
    }
}
