using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public KeyCode openKey = KeyCode.Joystick1Button4;
    public int speedDegPerSecond = 90;
    public bool playerNearDoor = false;
    private int direction = 0; /* the direction the door is swinging */
    private GameObject door;
    public int keyType;

    // Start is called before the first frame update
    void Start()
    {
        door = transform.Find("door").gameObject; // using transform searches children only
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearDoor && Input.GetKeyDown(openKey))
        {
            if (direction == 0) // door isn't already moving
            {
                if (door.transform.rotation.eulerAngles.y == 270)
                    direction = speedDegPerSecond;
                else
                    direction = -1 * speedDegPerSecond;
            }
            else
            {
                direction = -direction; // change direction
            }
        }
        if (direction != 0)
        {
            // rotate a little
            door.transform.Rotate(Vector3.up, direction * Time.deltaTime);
            // see if we are at the closed or open points
            float doorAngle = door.transform.rotation.eulerAngles.y;
            if ((direction < 0 && doorAngle < 270)
                || ((direction > 0 && doorAngle >= 360) || (direction > 0 && doorAngle < 30))
               )
            {
                if (direction < 0)
                {
                    door.transform.rotation = Quaternion.Euler(door.transform.rotation.eulerAngles.x,
                        270, door.transform.rotation.eulerAngles.z);
                    direction = 0;
                }
                else if (direction > 0)
                {
                    door.transform.rotation = Quaternion.Euler(door.transform.rotation.eulerAngles.x,
                        360, door.transform.rotation.eulerAngles.z);
                    direction = 0;
                }
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        // todo: make sure the other is actually the player!
        playerNearDoor = false;
    }
}
