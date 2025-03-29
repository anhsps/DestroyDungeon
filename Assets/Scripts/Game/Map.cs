using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshFilter>())
            {
                /*if (!child.GetComponent<Collider>())
                {
                    child.gameObject.AddComponent<MeshCollider>();
                }*/

                if (child.GetComponent<Collider>())
                {
                    var rb = child.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
