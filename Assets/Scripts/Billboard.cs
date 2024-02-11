using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float gizmoSize = 0.5f;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}
