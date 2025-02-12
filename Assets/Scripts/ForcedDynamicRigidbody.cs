using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedDynamicRigidbody : MonoBehaviour
{
    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.isKinematic) { rb.isKinematic = false; }
    }
}
