using System.Collections.Generic;
using Spellie.Config;
using Spellie.SpawnedObjects;
using UnityEngine;

namespace Spellie.Samples
{
    public class PrefabLoader : MonoBehaviour
    {

        /// <summary>
        /// List of prefabs to load into Spellie Prefab System
        /// </summary>
        public List<SpellieSpawnedObject> prefabs = new List<SpellieSpawnedObject>();

        public void Awake()
        {
            foreach (var p in prefabs)
            {
                SpellieSingleton.Instance.prefabs.AddPrefab(p.name, p);
            }

        }

    }
}