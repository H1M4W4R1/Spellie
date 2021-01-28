using System;
using Spellie.Tools;
using UnityEngine;

namespace Spellie.Samples
{
    public class SamplePlayerMovement : MonoBehaviour
    {

        public bool useKeyDownInsteadOfKeyForSpell = true;
        
        public float speed = 5f;

        [TextArea(50, Int32.MaxValue)]
        public string spell;
        
        void Update()
        {
            var e = GetComponent<IEntity>();
            var transform1 = transform;
            
            if (Input.GetKey(KeyCode.W))
                transform1.position += speed * Time.deltaTime * transform1.right;
            if (Input.GetKey(KeyCode.S))
                transform1.position -= speed * Time.deltaTime * transform1.right;
            if (Input.GetKey(KeyCode.A))
                transform1.position += speed * Time.deltaTime * transform1.forward;
            if (Input.GetKey(KeyCode.D))
                transform1.position -= speed * Time.deltaTime * transform1.forward;

            if (useKeyDownInsteadOfKeyForSpell)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    SpellieParser.ParseSpell(spell).Cast(e, e, e);
            }
            else
            {
                if (Input.GetKey(KeyCode.Space))
                    SpellieParser.ParseSpell(spell).Cast(e, e, e);
            }
        }
    }
}
