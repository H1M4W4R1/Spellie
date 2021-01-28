using System.Collections.Generic;

namespace Spellie.Structure
{
    public class MetaObject
    {
        /// <summary>
        /// Contains object MetaData
        /// </summary>
        private Dictionary<string, object> _metaData = new Dictionary<string, object>();

        /// <summary>
        /// Set metadata parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        public void Set<T>(string id, T obj)
        {
            _metaData[id] = obj;
        }

        /// <summary>
        /// Get metadata parameter
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string id)
        {
            if (_metaData.ContainsKey(id))
                return (T) _metaData[id];

            return default;
        }

        /// <summary>
        /// Gets metadata parameter as generic object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(string id)
        {
            if (_metaData.ContainsKey(id))
                return _metaData[id];

            return null;
        }
    
    }
}
