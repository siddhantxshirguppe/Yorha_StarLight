using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headManager : MonoBehaviour
{
    public GameObject headObject;
    private float headZPosition;
    private float startingHeadPos;
    // Start is called before the first frame update
    void Start()
    {
        startingHeadPos = headObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (headObject != null)
        {

            headZPosition = headObject.transform.position.z;
            if (headZPosition > startingHeadPos)
            {
                SetStateForward();
            }
            else
            {
                SetStateBackward();
            }
        }

    }

    void SetStateForward()
    {
        // Implement code to set the state to forward
        Debug.Log("sidlog State set to forward"+ headZPosition);
    }

    void SetStateBackward()
    {
        // Implement code to set the state to backward
        Debug.Log("sidlog State set to backward"+ headZPosition);
    }
}
