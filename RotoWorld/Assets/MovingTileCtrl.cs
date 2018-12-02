using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTimeCtrl : MonoBehaviour
{

    // Use this for initialization
    float offset;
    public enum MoveInAxis { X, Y, Z };
    MoveInAxis axisMove;
    float targetPos;
    public float originalPos;
    public float currentPos;
    float delay;
    bool targetReached;
    float moveRate;
    GameObject target;
    Vector3 playerTileOffset;
    void Start()
    {
        offset = 8;
        axisMove = MoveInAxis.Z;//if not set by setAxisMove then it will default to Z.
        setOriginalPos();
        currentPos = originalPos;
        targetPos = originalPos + offset;
        delay = 2;
        targetReached = false;
        moveRate = 0.4f*Time.fixedDeltaTime;
        target = null;
    }

    private void setOriginalPos()
    {
        switch (axisMove)
        {
            case MoveInAxis.X:
                {
                    originalPos = transform.position.x;
                    break;
                }
            case MoveInAxis.Y:
                {
                    originalPos = transform.position.y;
                    break;
                }
            default:
                {
                    originalPos = transform.position.z;
                    break;
                }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Abs(currentPos - targetPos) > 0.05 && !targetReached)
        {
            StartCoroutine(MoveToTarget());
        }
        else
        {
            targetReached = true;
            StartCoroutine(MoveBackToOriginal());
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, currentPos);
    }

    public void setAxisMove(MoveInAxis _movAxis)
    {
        axisMove = _movAxis;
    }

    public IEnumerator MoveToTarget()
    {
        yield return new WaitForSeconds(delay);
        currentPos = Mathf.Lerp(currentPos, targetPos, moveRate);
    }

    public IEnumerator MoveBackToOriginal()
    {
        yield return new WaitForSeconds(delay);
        currentPos = Mathf.Lerp(currentPos, originalPos, moveRate);
        if(Mathf.Abs(currentPos - originalPos) < 0.05)//reached original position again
        {
            currentPos = originalPos;
            targetReached = false;
            currentPos = originalPos;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
            playerTileOffset = target.transform.position - transform.position;
        }
    }
    private void LateUpdate()
    {
        if(target != null)
        {
            target.transform.position = target.transform.position + playerTileOffset;
        }
    }



}
