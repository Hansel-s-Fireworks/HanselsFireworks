using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            RotatePlayer(Vector3.right);
        }

        if (Input.GetKey(KeyCode.A))
        {
            RotatePlayer(-Vector3.right);
        }

        if (Input.GetKey(KeyCode.S))
        {
            RotatePlayer(-Vector3.forward);
        }

        if (Input.GetKey(KeyCode.W))
        {
            RotatePlayer(Vector3.forward);
        }
    }

    private void RotatePlayer(Vector3 direction)
    {
        // 주어진 방향을 향해 플레이어를 회전합니다.
        Vector3 lookDirection = direction.normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

}
