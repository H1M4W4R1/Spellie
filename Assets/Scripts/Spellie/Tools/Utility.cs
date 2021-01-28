using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Spellie.SpawnedObjects;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Spellie.Tools
{
    public class Utility
    {
        public class Src
        {
            public const string NONE = "none";
            public const string CASTER = "caster";
            public const string SOURCE = "source";
            public const string TARGET = "target";
        }
        public class Direction
        {
            /// <summary>
            /// Projectile will fly from caster to his look direction
            /// </summary>
            public const string FORWARD = "forward";
            
            /// <summary>
            /// Projectile will fly to caster from its spawn pos
            /// </summary>
            public const string REVERSE = "reverse";
        }

        public class TargetType
        {
            /// <summary>
            /// Applies if entity is evil to player
            /// </summary>
            public const string ENEMY = "evil";
            
            /// <summary>
            /// Applies if entity is friendly to player
            /// </summary>
            public const string FRIENDLY = "friendly";
            
            /// <summary>
            /// Applies if entity is player
            /// </summary>
            public const string PLAYER = "player";
            
            /// <summary>
            /// Applies if entity is an npc
            /// </summary>
            public const string NPC = "npc";
            
            /// <summary>
            /// Applies if entity is world object like barrel, chest etc.
            /// </summary>
            public const string WORLD = "world";
            
            /// <summary>
            /// Caster is related to entity that casted spell -
            /// applies if entity that is being targeted is same as entity casted spell
            /// </summary>
            public const string CASTER = "caster";

            public const string ANY = "any";
        }
        
        /// <summary>
        /// Checks if target can be hit by possible targets
        /// </summary>
        /// <param name="scannedTarget"></param>
        /// <param name="possibleTargets"></param>
        /// <returns></returns>
        public static bool CanHitTarget(string scannedTarget, string possibleTargets)
        {
            var t = possibleTargets.Replace(" ", "");
            return t.Split(',').Any(val => val == scannedTarget);
        }


        /// <summary>
        /// Spawns object in world
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static SpellieSpawnedObject Instantiate(SpellieSpawnedObject prefab, Transform parent = null)
        {
            return Instantiate(prefab, Vector3.zero, Vector3.zero, parent);
        }
        
        /// <summary>
        /// Spawns object in world
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static SpellieSpawnedObject Instantiate(SpellieSpawnedObject prefab, Vector3 position, Transform parent = null)
        {
            return Instantiate(prefab, position, Vector3.zero, parent);
        }
        
        /// <summary>
        /// Gets castSource for attached property
        /// </summary>
        /// <param name="attached"></param>
        /// <param name="source"></param>
        /// <param name="caster"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ICastSource GetSource(string attached, ICastSource source, IEntity caster, IEntity target)
        {
            var parent = null as ICastSource;
            switch (attached)
            {
                case Src.SOURCE:
                    parent = source;
                    break;
                case Src.CASTER:
                    parent = caster;
                    break;
                case Src.TARGET:
                    parent = target;
                    break;
            }

            return parent;
        }
        
        /// <summary>
        /// Spawns object in world
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <param name="eulerAngles"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static SpellieSpawnedObject Instantiate(SpellieSpawnedObject prefab, Vector3 position, Vector3 eulerAngles, Transform parent = null)
        {
            var o =  Object.Instantiate(prefab, position, Quaternion.Euler(eulerAngles), parent);
            if (!o.useGlobalPosition)
                o.spawnPos = o.transform.localPosition;
            else
                o.spawnPos = o.transform.position;
            return o;
        }

        /// <summary>
        /// Randomizes single circle position
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="upAxis"></param>
        /// <returns></returns>
        public static Vector3 RandomCirclePosition(float distance, Vector3 upAxis)
        {
            return GetCirclePosition(Random.Range(0, 360f), distance, upAxis);
        }

        /// <summary>
        /// Randomizes position on sphere
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector3 RandomSpherePosition(float distance)
        {
            // Get random angles
            var rho = Random.Range(0f, 360f);
            var theta = Random.Range(0f, 360f);
            
            // Get vector from angles
            var x = distance * Mathf.Cos(rho * Mathf.Deg2Rad) * Mathf.Cos(theta * Mathf.Deg2Rad);
            var y = distance * Mathf.Cos(rho * Mathf.Deg2Rad) * Mathf.Sin(theta * Mathf.Deg2Rad);
            var z = distance * Mathf.Sin(rho * Mathf.Deg2Rad);

            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Randomizes position on circle
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="distance"></param>
        /// <param name="upAxis">circle normal vector</param>
        /// <returns></returns>
        public static Vector3 GetCirclePosition(float angle, float distance, Vector3 upAxis)
        {
            // Calculate position place
            var sin = Mathf.Sin(Mathf.Deg2Rad * angle);
            var cos = Mathf.Cos(Mathf.Deg2Rad * angle);

            // Recalculate position
            var pos = new Vector3(cos, 0, sin);
            pos = Quaternion.Euler(upAxis) * pos;
                
            // Move position to correct distance
            pos /= pos.magnitude;
            pos *= distance;

            return pos;
        }

        /// <summary>
        /// Gets positions on circle cone
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="angle"></param>
        /// <param name="startAngle"></param>
        /// <param name="distance"></param>
        /// <param name="upAxis"></param>
        /// <returns></returns>
        public static List<Vector3> GetCircleConePositions(int amount, float angle, float startAngle, float distance,
            Vector3 upAxis)
        {
            var ret = new List<Vector3>();
            
            // Get angle difference
            var dAngle = angle / amount;
            for (var a = 0; a < amount; a++)
            {
                // Calculate current angle
                var cAngle = a * dAngle;
                
                // Calculate position
                var pos = GetCirclePosition(startAngle + cAngle, distance, upAxis);
                ret.Add(pos);
            }

            return ret;
        }

        /// <summary>
        /// Gets positions at circle
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="distance"></param>
        /// <param name="upAxis"></param>
        /// <returns></returns>
        public static List<Vector3> GetCirclePositions(int amount, float distance, Vector3 upAxis)
        {
            return GetCircleConePositions(amount, 360f, 0f, distance, upAxis);
        }

        /// <summary>
        /// Converts direction to direction multiplier
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static float ParseDirection(string direction)
        {
            int mult;
            switch (direction)
            {
                case Direction.FORWARD:
                    mult = 1;
                    break;
                case Direction.REVERSE:
                    mult = -1;
                    break;
                default:
                    throw new ArgumentException(
                        "[Spellie] Unknown direction found. Please use one of the allowed directions.");
            }

            return mult;
        }

        /// <summary>
        /// Parses string to object, used in <see cref="SpellieParser"/>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object FromString(string data, Type t)
        {
            if (t == typeof(int))
                return int.Parse(data);
            if (t == typeof(long))
                return long.Parse(data);
            if (t == typeof(ulong))
                return ulong.Parse(data);
            if (t == typeof(short))
                return short.Parse(data);
            if (t == typeof(ushort))
                return ushort.Parse(data);
            if (t == typeof(byte))
                return byte.Parse(data);
            if (t == typeof(uint))
                return uint.Parse(data);
            if (t == typeof(bool))
                return bool.Parse(data);
            if (t == typeof(float))
                return float.Parse(data, CultureInfo.InvariantCulture);
            if (t == typeof(double))
                return double.Parse(data, CultureInfo.InvariantCulture);
            if (t == typeof(Vector3))
            {
                var v3 = data.Replace(")", "").Replace("(", "").Replace(" ", "");
                var p = v3.Split(',');
                var vector = new Vector3();
                vector.x = float.Parse(p[0], CultureInfo.InvariantCulture);
                vector.y = float.Parse(p[1], CultureInfo.InvariantCulture);
                vector.z = float.Parse(p[2], CultureInfo.InvariantCulture);
                return vector;
            }
            if (t == typeof(Vector2))
            {
                var v3 = data.Replace(")", "").Replace("(", "").Replace(" ", "");
                var p = v3.Split(',');
                var vector = new Vector2();
                vector.x = float.Parse(p[0], CultureInfo.InvariantCulture);
                vector.y = float.Parse(p[1], CultureInfo.InvariantCulture);
                return vector;
            }
            if (t == typeof(Vector4))
            {
                var v3 = data.Replace(")", "").Replace("(", "").Replace(" ", "");
                var p = v3.Split(',');
                var vector = new Vector4();
                vector.x = float.Parse(p[0], CultureInfo.InvariantCulture);
                vector.y = float.Parse(p[1], CultureInfo.InvariantCulture);
                vector.z = float.Parse(p[2], CultureInfo.InvariantCulture);
                vector.w = float.Parse(p[3], CultureInfo.InvariantCulture);
                return vector;
            }
            if (t == typeof(Quaternion))
            {
                var v3 = data.Replace(")", "").Replace("(", "").Replace(" ", "");
                var p = v3.Split(',');
                var vector = new Quaternion();
                vector.x = float.Parse(p[0], CultureInfo.InvariantCulture);
                vector.y = float.Parse(p[1], CultureInfo.InvariantCulture);
                vector.z = float.Parse(p[2], CultureInfo.InvariantCulture);
                vector.w = float.Parse(p[3], CultureInfo.InvariantCulture);
                return vector;
            }
            
            
            return data;

        }
    }
}