using System;
using System.Collections.Generic;
using System.Linq;
using Spellie.SpawnedObjects;

namespace Spellie.Config
{
    
    [Serializable]
    public class PrefabsConfig 
    {
        
        /// <summary>
        /// List of all contained prefabs
        /// </summary>
        private readonly List<PrefabObject> _prefabs = new List<PrefabObject>();

        public static PrefabsConfig New() => new PrefabsConfig();

        /// <summary>
        /// Gets prefab using name
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public PrefabObject Get(string n) => _prefabs.FirstOrDefault(p => p.name == n);
        
        /// <summary>
        /// Adds prefab
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public PrefabsConfig AddPrefab(string name, SpellieSpawnedObject prefab)
        {
            var obj = PrefabObject.New()
                .WithName(name)
                .WithObject(prefab);

            _prefabs.Add(obj);
            return this;
        }
        
        /// <summary>
        /// Removes all prefabs by name
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public PrefabsConfig RemovePrefab(string n)
        {
            _prefabs.RemoveAll(p => p.name == n);
            return this;
        }
        
        [Serializable]
        public class PrefabObject
        {
            /// <summary>
            /// Prefab name, used in 'object' directives
            /// </summary>
            public string name;
            
            /// <summary>
            /// Prefab object that will be spawned
            /// </summary>
            public SpellieSpawnedObject prefab;
            
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public static PrefabObject New() => new PrefabObject();

            /// <summary>
            /// Defines prefab name
            /// </summary>
            /// <param name="n"></param>
            /// <returns></returns>
            public PrefabObject WithName(string n)
            {
                name = n;
                return this;
            }

            /// <summary>
            /// Defines prefab object to spawn
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public PrefabObject WithObject(SpellieSpawnedObject obj)
            {
                prefab = obj;
                return this;
            }
        }
    }

    
}