using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNormal : MonoBehaviour
{
    public float destoryTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        Destroy(gameObject, destoryTime);
        
    }
}
