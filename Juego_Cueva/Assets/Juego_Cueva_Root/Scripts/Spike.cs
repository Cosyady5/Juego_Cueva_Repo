using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float upHeight = 1.5f;
    public float speed = 2f;
    public float stayUpTime = 0.5f;

    private Vector3 downPos;
    private Vector3 upPos;
    private bool isMoving = false;

    void Start()
    {
        downPos = transform.position;
        upPos = downPos + Vector3.up * upHeight;
    }

    public void Activate()
    {
        if (!isMoving) StartCoroutine(MoveSpike());
    }

    private System.Collections.IEnumerator MoveSpike()
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, upPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, upPos, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(stayUpTime);

        while (Vector3.Distance(transform.position, downPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, downPos, speed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}
