using Spellie.Config;
using Spellie.Elements.Abstracts;
using Spellie.MovementTypes;
using Spellie.MovementTypes.Ignore;
using Spellie.Structure;
using Spellie.Tools;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Spellie.SpawnedObjects
{
    /// <summary>
    /// Generic for SpawnedObject - processes movement and duration - aka generic parameters
    /// </summary>
    public class SpellieSpawnedObject : MonoBehaviour, ICastSource
    {

        /// <summary>
        /// If true then the object will use global position instead of local
        /// </summary>
        public bool useGlobalPosition;

        private Transform _transform;
        
        private Quaternion _currentRotation;
        private Vector3 _currentPosition;
        private Vector3 _currentScale = Vector3.one;

        public IEntity caster;
        
        /// <summary>
        /// Represents object X axis
        /// </summary>
        public Vector3 right;
        
        /// <summary>
        /// Represents object Y axis
        /// </summary>
        public Vector3 up;
        
        /// <summary>
        /// Represents object Z axis
        /// </summary>
        public Vector3 forward;

        /// <summary>
        /// Current LifeTime of this object since spawn
        /// </summary>
        [FormerlySerializedAs("_lifeTime")] [SerializeField]
        private float lifeTime;
        
        /// <summary>
        /// Initial position of spawned object
        /// </summary>
        public Vector3 spawnPos;
        
        /// <summary>
        /// Source element that spawned this object
        /// </summary>
        public ISpellieElement sourceElement;

        /// <summary>
        /// Meta Object of current Source - contains all meta parameters of spawned object
        /// </summary>
        public MetaObject MetaObject => (MetaObject) sourceElement;

        private IMovementType _movement;

        /// <summary>
        /// Movement of this object
        /// </summary>
        public IMovementType Movement
        {
            get
            {
                if (_movement == null) 
                    _movement = MetaObject.Get<IMovementType>("movement");
                return _movement;
            }
        }

        private float _duration;

        /// <summary>
        /// Duration of this object, zero or lower => infinite
        /// </summary>
        public float Duration
        {
            get
            {
                if (_duration < 0)
                    _duration = MetaObject.Get<float>("duration");
                return _duration;
            }
        }

        private string _direction;

        /// <summary>
        /// Direction of this object if exists
        /// </summary>
        public string Direction
        {
            get
            {
                if(string.IsNullOrEmpty(_direction))
                    _direction = MetaObject.Get<string>("direction");
                return _direction;
            }
        }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _currentPosition = spawnPos;
            
            
        }

        private void OnEnable()
        {
            // Register this object
            SpellieSingleton.Instance.spawnedObjects.Add(this);
            var objTransform = transform;

            // Process object variables
            try
            {
                useGlobalPosition = MetaObject.Get<bool>("use_global");
            }
            catch
            {
                useGlobalPosition = false;
            }

            _currentRotation = objTransform.rotation;
            _currentScale = objTransform.localScale;
        }

        private void OnDisable()
        {
            // Unregister this object
            SpellieSingleton.Instance.spawnedObjects.Remove(this);
        }

        public void ProcessObjectBehaviour()
        {
            // Process movement
            if (Movement != null)
                _currentPosition = Movement.EvaluateMovement(this, lifeTime);
        }

        public void Update()
        {
            var objTransform = transform;

            // Process position location
            if(!(Movement is IIgnorePosition))
                if (!useGlobalPosition)
                {
                    objTransform.localPosition = _currentPosition;
                }
                else
                {
                    objTransform.position = _currentPosition;
                }

            // Process rotation
            if(!(Movement is IIgnoreRotation))
                objTransform.rotation = _currentRotation;
            
            // Process scale
            if(!(Movement is IIgnoreScale))
                objTransform.localScale = _currentScale;
            
            // Process lifetime
            lifeTime += Time.deltaTime;
            
            // Process duration
            if (Duration > 0 && Duration < lifeTime)
                Destroy(gameObject);
        }

        /// <summary>
        /// Sets directions of this object basing on transform
        /// </summary>
        /// <param name="objTransform"></param>
        public void SetDirections(Transform objTransform)
        {
            forward = objTransform.forward;
            right = objTransform.right;
            up = objTransform.up;
        }

        /// <inheritdoc />
        public Transform GetTransform()
        {
            return _transform;
        }

        public Vector3 GetPosition()
        {
            return _transform.position;
        }
    }
}