using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2Bubble : MonoBehaviour
{

    [SerializeField]
    private GameObject target;

    private float approachTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ApproachTarget());
    }

    IEnumerator ApproachTarget()
    {
        yield return new WaitForSeconds(1.0f);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < approachTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / approachTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

}
