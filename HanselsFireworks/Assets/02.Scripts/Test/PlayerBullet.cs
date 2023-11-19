using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Test
{
    public class PlayerBullet : MonoBehaviour
    {
        public float speed;
        public float time;
        private MemoryPool memoryPool;
        private ImpactMemoryPool impactMemoryPool;
        [SerializeField] private Mode mode;
        // Start is called before the first frame update
        void Start()
        {
            impactMemoryPool = FindObjectOfType<ImpactMemoryPool>();
            transform.SetParent(null);
        }
        public void Setup(MemoryPool pool)
        {
            memoryPool = pool;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            transform.Translate(0, -speed * Time.fixedDeltaTime, 0);
            // 3초 후에도 파괴되지 않았다면 적을 맞추지 못한 것이므로 콤보 초기화
            if (time > 3)
            {
                time = 0;       // 시간을 다시 0으로 만들어줘야 다시 생성된다.
                memoryPool.DeactivatePoolItem(gameObject);      // 비활성화
            }
        }
    }

}
