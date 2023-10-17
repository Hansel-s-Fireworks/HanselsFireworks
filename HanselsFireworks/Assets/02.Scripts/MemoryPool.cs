using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    // 占쌨몌옙 풀占쏙옙 占쏙옙占쏙옙占실댐옙 占쏙옙占쏙옙占쏙옙트 占쏙옙占쏙옙
    private class PoolItem
    {
        public bool isActive;           // "gameObject"占쏙옙 활占쏙옙화/占쏙옙활占쏙옙화 占쏙옙占쏙옙
        public GameObject gameObject;   // 화占썽에 占쏙옙占싱댐옙 占쏙옙占쏙옙 占쏙옙占쌈울옙占쏙옙占쏙옙트
    }

    private int increaseCount = 5;      // 占쏙옙占쏙옙占쏙옙트占쏙옙 占쏙옙占쏙옙占쌀띰옙, 占쌩곤옙占쏙옙 占쏙옙占쏙옙占실댐옙 占쏙옙占쏙옙占쏙옙트 占쏙옙
    private int maxCount;               // 占쏙옙占쏙옙 占쏙옙占쏙옙트占쏙옙 占쏙옙溝퓸占?占쌍댐옙 占쏙옙占쏙옙占쏙옙트 占쏙옙占쏙옙
    private int activeCount;            // 占쏙옙占쏙옙 占쏙옙占쌈울옙 占쏙옙占실곤옙 占쌍댐옙 占쏙옙占쏙옙占쏙옙트 占쏙옙占쏙옙

    private GameObject poolObject;      // 占쏙옙占쏙옙占쏙옙트 풀占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占싹댐옙 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙트 占쏙옙占쏙옙占쏙옙
    private List<PoolItem> poolItemList;// 占쏙옙占쏙옙占실댐옙 占쏙옙占?占쏙옙占쏙옙占쏙옙트占쏙옙 占쏙옙占쏙옙占싹댐옙 占쏙옙占쏙옙트 

    public int MaxCount => maxCount;        // 占쌤부울옙占쏙옙 확占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙티 
    public int ActiveCount => activeCount;  // 占쌤부울옙占쏙옙 확占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙티

    // private Vector3 tempPosition = new Vector3(1000, 1000, 1000);

    // 占쌔댐옙 占쏙옙占쌈울옙占쏙옙占쏙옙트占쏙옙 占쌨아쇽옙 占쏙옙占쏙옙占쏙옙 호占쏙옙
    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();   // 占쏙옙占쏙옙 5占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 占싱몌옙 占쏙옙占쏙옙
    }
    // 한국어주석

    public void InstantiateObjects()
    {
        maxCount += increaseCount;
        for (int i = 0; i < increaseCount; i++)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            // poolItem.gameObject.transform.position = tempPosition;
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }

    /// <summary>
    /// 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙占쏙옙 占쏙옙占?占쏙옙占쏙옙占쏙옙트占쏙옙 占쏙옙占쏙옙
    /// </summary>
    public void DestroyObjects()
    {
        if (poolItemList == null) return;
        int count = poolItemList.Count; 
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        // 占쏙옙占쏙옙 占쏙옙占쏙옙占쌔쇽옙 占쏙옙占쏙옙占싹댐옙 占쏙옙占?占쏙옙占쏙옙占쏙옙트 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙 활占쏙옙화 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙트 占쏙옙占쏙옙 占쏙옙
        // 占쏙옙占?占쏙옙占쏙옙占쏙옙트占쏙옙 활占쏙옙화 占쏙옙占쏙옙占싱몌옙 占쏙옙占싸울옙 占쏙옙占쏙옙占쏙옙트 占십울옙
        if (maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            // i占쏙옙占쏙옙占쏙옙 占쏙옙활占쏙옙화占싱몌옙
            if (poolItem.isActive == false)
            {
                // 占쏙옙占쏙옙 占쏙옙 활占쏙옙화 
                activeCount++;
                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    // 占쏙옙占쏙옙 占쏙옙占쏙옙占?占싹뤄옙占?占쏙옙占쏙옙占쏙옙트占쏙옙 占쏙옙활占쏙옙화 占쏙옙占승뤄옙 占쏙옙占쏙옙
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            if (poolItem.gameObject == removeObject)
            {
                activeCount--;

                // poolItem.gameObject.transform.position = tempPosition;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }
    }

    // 占쏙옙占쌈울옙 占쏙옙占쏙옙占쏙옙占?占쏙옙占?占쏙옙占쏙옙占쏙옙트占쏙옙 占쏙옙활占쏙옙화 占쏙옙占승뤄옙 占쏙옙占쏙옙
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                // poolItem.gameObject.transform.position = tempPosition;
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }



}
