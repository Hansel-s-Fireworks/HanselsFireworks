using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : MonoBehaviour
{
    // �޸� Ǯ�� �����Ǵ� ������Ʈ ����
    private class PoolItem
    {
        public bool isActive;           // "gameObject"�� Ȱ��ȭ/��Ȱ��ȭ ����
        public GameObject gameObject;   // ȭ�鿡 ���̴� ���� ���ӿ�����Ʈ
    }

    private int increaseCount = 5;      // ������Ʈ�� �����Ҷ�, �߰��� �����Ǵ� ������Ʈ ��
    private int maxCount;               // ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ����
    private int activeCount;            // ���� ���ӿ� ���ǰ� �ִ� ������Ʈ ����

    private GameObject poolObject;      // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������
    private List<PoolItem> poolItemList;// �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ 

    public int MaxCount => maxCount;        // �ܺο��� Ȯ���� ���� ������Ƽ 
    public int ActiveCount => activeCount;  // �ܺο��� Ȯ���� ���� ������Ƽ

    // private Vector3 tempPosition = new Vector3(1000, 1000, 1000);

    // �ش� ���ӿ�����Ʈ�� �޾Ƽ� ������ ȣ��
    public MemoryPool(GameObject poolObject)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();   // ���� 5���� ������ �̸� ����
    }

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
    /// ���� �������� ��� ������Ʈ�� ����
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

        // ���� �����ؼ� �����ϴ� ��� ������Ʈ ������ ���� Ȱ��ȭ ������ ������Ʈ ���� ��
        // ��� ������Ʈ�� Ȱ��ȭ �����̸� ���ο� ������Ʈ �ʿ�
        if (maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            // i������ ��Ȱ��ȭ�̸�
            if (poolItem.isActive == false)
            {
                // ���� �� Ȱ��ȭ 
                activeCount++;
                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    // ���� ����� �Ϸ�� ������Ʈ�� ��Ȱ��ȭ ���·� ����
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

    // ���ӿ� ������� ��� ������Ʈ�� ��Ȱ��ȭ ���·� ����
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
