using UnityEngine;
namespace JoyWay.Core.Resources
{
    public class LazyResource<T> where T : Object
    {
        private string _path;
        private T _value;

        public LazyResource(string path)
        {
            _path = path;
        }

        public T Value
        {
            get
            {
                if (_value == null)
                {
                    lock (this)
                    {
                        if (_value == null)
                        {
                            _value = UnityEngine.Resources.Load<T>(_path);
                        }
                    }
                }

                return _value;
            }
        }
    }
}