using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinManager : MonoBehaviour
{
    public static PumkinManager Instance;

    [SerializeField]
    private List<GameObject> pumkins;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("instance ?ùÏÑ±");
            Instance = this;
        }
    }

    public void addPumkin(GameObject pumkin)
    {
        pumkins.Add(pumkin);
    }

    public void DeletePumkin(GameObject pumkin)
    {
        pumkins.Remove(pumkin);
    }
    
    public void DeleteAllPumkin()
    {
        pumkins.Clear();
    }

    public List<GameObject> GetPumkinList()
    {
        return pumkins;
    }

    public void Attack()
    {
        foreach (GameObject pumkin in pumkins)
        {
            pumkin.GetComponent<PumkinEnemy>().Attack();
        }
    }
}