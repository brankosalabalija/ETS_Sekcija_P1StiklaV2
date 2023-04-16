using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity=transform.right*20f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(this.gameObject);
        
    }
}
