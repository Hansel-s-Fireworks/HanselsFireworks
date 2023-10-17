using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    private MemoryPool impactMemoryPool;

    private void Awake()
    {
        impactMemoryPool = new MemoryPool(effect);
    }

    private void OnApplicationQuit()
    {
        impactMemoryPool.DestroyObjects();
    }

    // 얘를 총알에서 호출해야 한다. 
    public void OnSpawnImpact(Collider other,Vector3 position, Quaternion rotation)
    {
        // 맞힌 위치로 이동
        GameObject item = impactMemoryPool.ActivatePoolItem();

        item.transform.position = position;
        // item.transform.rotation = rotation;       
        item.transform.SetParent(other.transform);      // 쿠키에 붙어 다니기

        item.GetComponent<Impact>().Setup(impactMemoryPool);
    }
}
