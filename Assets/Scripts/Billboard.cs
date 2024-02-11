using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    Vector3 camDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camDir = Camera.main.transform.forward;
        camDir.y = 0;

        transform.rotation = Quaternion.LookRotation(camDir);
    }
}
