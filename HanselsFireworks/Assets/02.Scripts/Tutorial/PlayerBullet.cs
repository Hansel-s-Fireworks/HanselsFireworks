using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Tutorial
{
    public class PlayerBullet : MonoBehaviour
    {
        public float speed;
        public float time;
        [SerializeField] private Mode mode;
        // Start is called before the first frame update
        void Start()
        {
            transform.SetParent(null);
            Destroy(gameObject, 3f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.Translate(0, -speed * Time.fixedDeltaTime, 0);
        }
    }
}