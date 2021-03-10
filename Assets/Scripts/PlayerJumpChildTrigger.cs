using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpChildTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<PlayerController>().OnChildTriggerEntered(other, transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.gameObject.GetComponent<PlayerController>().OnChildTriggerExited(other, transform.position);
    }
}
