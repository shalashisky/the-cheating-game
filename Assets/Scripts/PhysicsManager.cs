using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(9, 10);
        Physics.IgnoreLayerCollision(9, 11);
        Physics.IgnoreLayerCollision(9, 12);

        Physics.IgnoreLayerCollision(10, 9);
        Physics.IgnoreLayerCollision(10, 11);
        Physics.IgnoreLayerCollision(10, 12);

        Physics.IgnoreLayerCollision(11, 9);
        Physics.IgnoreLayerCollision(11, 10);
        Physics.IgnoreLayerCollision(11, 12);

        Physics.IgnoreLayerCollision(12, 9);
        Physics.IgnoreLayerCollision(12, 10);
        Physics.IgnoreLayerCollision(12, 11);
    }

}
