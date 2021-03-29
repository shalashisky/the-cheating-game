using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    public CharacterController pController;


    //Analog inputs
    public float joypadInputXL;
    public float joypadInputYL;
    public float joypadInputYR;
    public float joypadInputXR;

    public float joypadInputLT;
    private bool joypadInputLTInUse;

    public float jumpForce;
    public float doubleJumpForce;
    public bool hasDoubleJumped = false;
    public float gravityModifier;
    public float speed = 10.0f;
    public float shit;
    public int jumpCount; //throwback to CORONA SDK a.k.a.  S O L A R  2 D
    public bool jumpCheck;
    public float rollSpeed;
    public int rollTimer;
    public bool isCrouching = false;
    public bool hasGun;

    public Vector3 movement;
    public Vector3 velocity;

    //WEAPONS - must keep track of:
    //What our current weapon is (true/false)
    //The reference to the gameObject in question
    //The animation number we need to use
    //The Head and spine rotation offsets
    //The aim height in neutral position

    //0: fists
    //1: flaregun
    //2: shotgun
    //3: sniper
    public bool[] weaponBools = new bool[10];
    public GameObject[] weaponObjects = new GameObject[10];
    public int[] weaponAnimationNumbers = new int[10];
    public Vector3[,] weaponRotationOffsets;

    public ParticleSystem doubleJumpParticle;

    public Transform Target;
    public Vector3 spineOffset;
    public Vector3 headOffset;

    private Transform spine;
    private Transform head;

    void Awake()
    {
        Physics.gravity = new Vector3(0, -60.1f, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        spine = playerAnim.GetBoneTransform(HumanBodyBones.Spine);
        head = playerAnim.GetBoneTransform(HumanBodyBones.Head);

        //Initialize our rotation offsets for spine/head based on current weapon
        weaponRotationOffsets = new Vector3[10, 2] {
        {new Vector3(10,0,-90), new Vector3(-7,0,-90) }, //fists
        {new Vector3(15,0,-90), new Vector3(-15,0,-90) }, //pistol
        {new Vector3(15,50,-90), new Vector3(20,-45,-90) }, //shotgun
        {new Vector3(20,52,-90), new Vector3(0,0,-90) }, //rifle
        {new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0) },
        };

        SelectWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        joypadInputXL = Input.GetAxis("MoveHorizontal");
        joypadInputYL = Input.GetAxis("MoveVertical");
        joypadInputYR = Input.GetAxis("MoveVerticalX");
        joypadInputXR = Input.GetAxis("MoveHorizontalX");
        joypadInputLT = Input.GetAxisRaw("LT");

        movement = new Vector3(joypadInputXL, 0.0f, 0.0f);

        playerAnim.SetFloat("Speed_f", Mathf.Abs(movement.x));
        //Crouching
        if (Input.GetKeyDown("joystick button 1") && jumpCount >= 1 && !isCrouching)
        {
            playerAnim.SetBool("Crouch_b", true);
            isCrouching = true;
        }
        else if (Input.GetKeyDown("joystick button 1") && jumpCount >= 1 && isCrouching)
        {
            playerAnim.SetBool("Crouch_b", false);
            //Store last 2 values of movement.x. If the same, change animation to idle, if not, keep running.
            isCrouching = false;
        }

        pController.Move(movement * speed * Time.deltaTime);

        velocity.y += gravityModifier * Time.deltaTime;


        jumpCheck = pController.isGrounded;

        if(jumpCheck)
        {
            velocity.y = 0;
            hasDoubleJumped = false;
            playerAnim.SetBool("Grounded", true);
            playerAnim.SetBool("Falling_b", false);
        } else
            pController.Move(velocity * Time.deltaTime);


        //Jump();


        //Play falling animation after jump or fall
        if (pController.velocity.y < -0.1 && !jumpCheck)
        {
            playerAnim.SetBool("Falling_b", true);
        }

        //Rotate character relative to left analog movement
        ChangeDirection();

        if (Mathf.Abs(joypadInputXL * speed) != 0)
            playerAnim.SetFloat("OverallSpeed_f", Mathf.Abs(joypadInputXL * speed));
        else
            playerAnim.SetFloat("OverallSpeed_f", 3);

    }







    //player methods

    //Gets the direction that the player should currently be facing and turns them
    private void ChangeDirection()
    {
        //No need to do anything if the player is idling their analog sticks
        if (movement.x != 0 || joypadInputXR != 0)
        {
            //First, check if the player is currently holding a ranged weapon
            if (!hasGun)
            {
                if (movement.x != 0) //Only change direction when left analog input is NOT zero.
                {
                    transform.rotation = Quaternion.LookRotation(movement); //Player's direction is based on:                               LEFT ANALOG HORIZONTAL
                    playerAnim.SetBool("RunningBack_b", false);
                }

            }
            else //They have a gun? Well, we need to check whether or not they're moving...
            {
                if (joypadInputXR != 0) //Change direction based on horizontal movement of right analog input, but only if it's NOT zero!
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(joypadInputXR, 0, 0)); //Player's direction is based on:       RIGHT ANALOG HORIZONTAL


                    if ((((Mathf.Ceil(movement.x) < 0) && joypadInputXR > movement.x) || ((Mathf.Ceil(joypadInputXR) < 0) && joypadInputXR < movement.x)) && (joypadInputXR != 0 && movement.x != 0)) //This if statement is brought to you by a 5 IQ moment
                        playerAnim.SetBool("RunningBack_b", true);
                    else
                        playerAnim.SetBool("RunningBack_b", false);

                }
                else if (movement.x != 0) //Otherwise, do the same as if we didn't have a gun
                {
                    transform.rotation = Quaternion.LookRotation(movement); //Player's direction is based on:                               LEFT ANALOG HORIZONTAL
                    playerAnim.SetBool("RunningBack_b", false);
                }
            }
        }
    }

    //Allows the player to increase their vertical velocity positively
    private void Jump()
    {
        //Jump Eins
        if (joypadInputLT != 0 && jumpCount >= 1)
        {
            //check if LT hasn't already been pressed...
            if (joypadInputLTInUse == false)
            {
                velocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityModifier);
                playerAnim.SetTrigger("Jump_trig");
                playerAnim.SetBool("Grounded", false);
                joypadInputLTInUse = true;
            }

        }


        //Jump Zwei
        if (joypadInputLT != 0 && !hasDoubleJumped)
        {
            //check if LT hasn't already been pressed...
            if (joypadInputLTInUse == false)
            {
                doubleJumpParticle.Play();
                hasDoubleJumped = true;

                //You must reset the y velocity back to zero before the second jump, otherwise jump heights will be entirely inconsistent
                velocity.y = 0;
                velocity.y += Mathf.Sqrt(doubleJumpForce * -3.0f * gravityModifier);
            }
        }

        //Reset LT to pressable?
        if (joypadInputLT == 0)
        {
            joypadInputLTInUse = false;
        }

    }

    //Enables weapon model and corresponding animation
    //param 1: Gameobject - AKA weapon in question
    //param 2: int        - the animaition number we want (USE 0 for no animation!)
    //param 3: int        - bool that corresponds to correct weapon in weapons[]
    private void SelectWeapon(int selectedWeapon)
    {
        //Set our new weapon to true
        weaponBools[selectedWeapon] = true;
        weaponObjects[selectedWeapon].SetActive(true);

        if (weaponObjects[selectedWeapon].tag == "Gun")
            hasGun = true;
        else
            hasGun = false;

        //Set our animations
        playerAnim.SetInteger("WeaponType_int", weaponAnimationNumbers[selectedWeapon]);

        //Change our offsets
        spineOffset = weaponRotationOffsets[selectedWeapon, 0];
        headOffset = weaponRotationOffsets[selectedWeapon, 1];

        //Then set literally everything else to false :)
        for (int i = 0; i < weaponBools.Length; i++)
        {
            if (i != selectedWeapon)
            {
                weaponBools[i] = false;
                weaponObjects[i].SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon_Flaregun"))
        {
            if (Input.GetButtonDown("Y"))
            {
                Destroy(other.gameObject);
                SelectWeapon(1);
            }
        }
        if (other.gameObject.CompareTag("Weapon_Shotgun"))
        {
            if (Input.GetButtonDown("Y"))
            {
                Destroy(other.gameObject);
                SelectWeapon(2);
            }
        }
        if (other.gameObject.CompareTag("Weapon_Rifle"))
        {
            if (Input.GetButtonDown("Y"))
            {
                Destroy(other.gameObject);
                SelectWeapon(3);
            }
        }

    }

    //Check jump hitbox to see if you can do the jump
    public void OnChildTriggerEntered(Collider other, Vector3 childPosition)
    {
        Debug.Log("bruh");
        if (other.gameObject.CompareTag("Ground"))
        {
            playerAnim.SetBool("Grounded", true);
            playerAnim.SetBool("Falling_b", false);
            jumpCount++;
            hasDoubleJumped = false;
        }
    }

    //Check jump hitbox to see if you cannot do the jump
    public void OnChildTriggerExited(Collider other, Vector3 childPosition)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount--;
        }
    }

    private void LateUpdate()
    {
        head.LookAt(Target.position);
        spine.LookAt(Target.position);
        spine.rotation = spine.rotation * Quaternion.Euler(spineOffset);
        head.rotation = head.rotation * Quaternion.Euler(headOffset);
    }

    private void FixedUpdate()
    {
        //if (!isCrouching)
        //{
        //    Debug.Log("shit");
        //    playerRb.AddForce(Vector3.right * joypadInputXL * speed * Time.deltaTime, ForceMode.Acceleration);
        //}

    }
}