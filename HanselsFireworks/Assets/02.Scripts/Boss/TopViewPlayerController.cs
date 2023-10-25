using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float moveSpeed = 5f; // 플레이어 이동 속도

    private void Update()
    {
        // WASD 키 입력
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 이동 방향 계산
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // 플레이어 이동
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0)) { Instantiate(bullet, firePoint); }
    }
}
