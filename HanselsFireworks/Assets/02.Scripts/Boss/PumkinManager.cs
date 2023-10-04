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
            Debug.Log("instance 생성");
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

    public List<GameObject> GetPumkinList()
    {
        return pumkins;
    }

    public void PlayAttackAnimatation()
    {
        foreach (GameObject pumkin in pumkins)
        {
            pumkin.GetComponent<Animator>().SetTrigger("IsAttack");
        }
    }
}
