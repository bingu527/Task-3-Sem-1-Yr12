using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[ExecuteInEditMode]

public class Pour : MonoBehaviour
{
    public int pourThreshold = 45;
    public Transform PourPoint = null;
    public GameObject streamPrefab = null;

    private bool isPouring = false;
    private Stream currentStream = null;

    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if(isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }
    private void StartPour()
    {
        Debug.Log("Start");
        currentStream = createStream();
        currentStream.Begin();
    }

    private void EndPour()
    {
        Debug.Log("End");
        currentStream.End();
        currentStream = null;
    }
    private float CalculatePourAngle()
    {
        return transform.forward.y * Mathf.Rad2Deg;
    }
    private Stream createStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, PourPoint.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }
}
