using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // We check if the object we collided with has a tag called "Obstacle".
        if (collision.gameObject.name == "Ghost")
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}
