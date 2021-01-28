using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Spellie.Affiinities;
using Spellie.SpawnedObjects;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace Spellie.Config
{
    public class SpellieSingleton : MonoBehaviour
    {
        private static SpellieSingleton _instance;

        private Thread _movementThread;

        /// <summary>
        /// If true then will log singleton exceptions
        /// </summary>
        public static bool logging = false;
        
        /// <summary>
        /// If it's null move SpellieSingleton to top of your Script Execution Order
        /// </summary>
        public static SpellieSingleton Instance => _instance;

        /// <summary>
        /// List of all available prefabs
        /// </summary>
        public PrefabsConfig prefabs = PrefabsConfig.New();

        /// <summary>
        /// List of currently available spawned objects
        /// </summary>
        public List<SpellieSpawnedObject> spawnedObjects = new List<SpellieSpawnedObject>();
        
        /// <summary>
        /// Get prefab using name
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public SpellieSpawnedObject GetPrefab(string prefab)
        {
            return prefabs.Get(prefab).prefab;
        }

        private readonly List<SpellieAffinity> _affinities = new List<SpellieAffinity>();

        /// <summary>
        /// Get all affinities
        /// </summary>
        /// <returns></returns>
        public List<SpellieAffinity> GetAffinities()
        {
            if (_affinities.Count < 1)
                LoadAffinities();

            return _affinities;
        }
        
        /// <summary>
        /// Gets affinity using name
        /// </summary>
        /// <param name="affinityName"></param>
        /// <returns></returns>
        public SpellieAffinity Get(string affinityName)
        {
            if (_affinities.Count < 1)
                LoadAffinities();
     
            return _affinities.FirstOrDefault(a =>
            {
                var dat = a?.ToString().Split('.');
                var aff= dat?[dat.Length - 1].ToLower().Replace("affinity", "");
                return aff == affinityName;
            });
        }

        private void LoadAffinities()
        {
            // Get all affinities
            var aff = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(SpellieAffinity)));

            // Create instances of affinities
            foreach (var v in aff)
            {
                var a = Activator.CreateInstance(v, null) as SpellieAffinity;
                a?.Initialize();
                _affinities.Add(a);
            }
        }

        public void InitMovementThread()
        {
              // This thread is responsible for moving all SpellieSpawnedObject.
            // Thanks to this Spellie movement system is way more effective than it would be on main thread.
            // Calculations takes around 1ms each 1000 objects moving using this system.
            // Calculation accuracy is based on objects amount, so if you have large amount of objects the accuracy is slowly
            // decreasing.
            //
            // Using thread instead of tasks, because it's better to have one dedicated worker for this.
            _movementThread = new Thread(() =>
            {
                while (true)
                {
                    // Prevents thread crash when collection is being modified.
                    try
                    {


                        // Using for instead of foreach to make it throw less errors.
                        for (var index = 0; index < spawnedObjects.Count; index++)
                        {
                            try
                            {
                                // Process behaviour for spawned object
                                var o = spawnedObjects[index];
                                o.ProcessObjectBehaviour();
                            }
                            catch (Exception ex)
                            {
                                // Something went wrong (probably collection was modified, so skip to next iteration)
                                if (logging)
                                    Debug.LogWarning(ex.Message + "\n" + ex.StackTrace);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Something went wrong (probably collection was modified, so skip to next iteration)
                        if (logging)
                            Debug.LogWarning(ex.Message + "\n" + ex.StackTrace);
                    }


                }
                // ReSharper disable once FunctionNeverReturns
            });
            
            // Start calculation thread.
            _movementThread.Start();
        }

        public void Awake()
        {
            // Initialize this object
            _instance = this;
            InitMovementThread();
            LoadAffinities();
        }

        public void FixedUpdate()
        {
            // Check if movement thread exists
            if (_movementThread == null)
                InitMovementThread();
        }

        private void OnDestroy()
        {
            // End thread
            try
            {
                _movementThread.Abort();
            }
            catch
            {
                // Do nothing :)
            }
        }

        #if UNITY_EDITOR
        [MenuItem("GameObject/Spellie/Create Singleton")]
        public static void SpawnSingleton()
        {
            if (FindObjectOfType<SpellieSingleton>() == null)
            {
                var obj = new GameObject("Spellie Singleton");
                obj.AddComponent<SpellieSingleton>();
            }
            else
            {
                Debug.Log("[Spellie] Singleton already exists");
            }
        }
        #endif
    }
}