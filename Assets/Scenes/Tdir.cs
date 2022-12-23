using System;
using UnityEngine;

public class Tdir : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    Transform tr;
  
    void Start () {  tr = transform;  }
  
    void Update ()
    {
        //Debug.Log("TF Forward = " + transform.forward);
        Debug.Log("TD forward" + transform.TransformDirection(direction));
        //Debug.Log("Local=" + Vector3.right + " | Global=" + tr.TransformDirection(Vector3.right));
    }

    private void OnDrawGizmos()
    {
        DrawSphere(direction);
    }

    private void DrawSphere(Vector3 pos)
    {
        //Debug.Log("DR");
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pos, 0.5f);
        
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformDirection(direction), 0.5f);
    }
}