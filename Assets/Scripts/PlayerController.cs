using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    public CharacterController pController;
    private PlayerInput inputManager;
    private HudManager hudManager;
    private SkinnedMeshRenderer playerRenderer;

    //Analog inputs
    public float joypadInputXL;
    public float joypadInputYL;
    public float joypadInputYR;
    public float joypadInputXR;

    public float joypadInputLT;
    public float joypadInputRT;
    private bool joypadInputLTInUse;
    private bool joypadInputRTInUse;

    //Button inputs
    private bool jumpAlt;
    private bool fireAlt;
    private bool interact;
    private bool taunt;

    public float jumpForce;
    public float doubleJumpForce;
    public bool hasDoubleJumped = false;
    public float gravityModifier;
    public float speed = 10.0f;
    public float shit;
    public int jumpCount; //throwback to CORONA SDK a.k.a.  S O L A R  2 D
    public bool jumpCheck;
    public bool ceilingCheck;
    public float rollSpeed;
    public int rollTimer;
    public bool isCrouching = false;
    public bool hasGun;
    public bool invunerable;
    public int playerNumber;

    public Vector3 movement;
    public Vector3 velocity;
    public Vector3 impact = Vector3.zero;
    private float fallTimer;
    public float jumpTimeCounter;
    private bool isJumping;
    private bool isKnockedOver;
    public bool isAttacking;

    public int health;
    private float healthModifier;
    public int money;
    public int crowdInfluence;

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
    //4: baseball bat
    public bool[] weaponBools = new bool[10];
    public GameObject[] weaponObjects = new GameObject[10];
    public int[] weaponAnimationNumbers = new int[10];
    public int currentWeapon;
    public Vector3[,] weaponRotationOffsets;

    public ParticleSystem doubleJumpParticle;

    public Transform Target;
    public Vector3 spineOffset;
    public Vector3 headOffset;
    public Vector3 lShoulderOffset;
    public Vector3 rShoulderOffset;

    private Transform spine;
    private Transform head;
    private Transform leftShoulder;
    private Transform rightShoulder;
    private Transform hips;

    void Awake()
    {
        Physics.gravity = new Vector3(0, -60.1f, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerAnim = GetComponent<Animator>();
        spine = playerAnim.GetBoneTransform(HumanBodyBones.Spine);
        head = playerAnim.GetBoneTransform(HumanBodyBones.Head);
        leftShoulder = playerAnim.GetBoneTransform(HumanBodyBones.LeftShoulder);
        rightShoulder = playerAnim.GetBoneTransform(HumanBodyBones.RightShoulder);
        hips = transform.Find("Hips_jnt");


        inputManager = GetComponent<PlayerInput>();
        InitializePlayer();

        health = 4;
        invunerable = false;
        isAttacking = false;
        //Initialize our rotation offsets for spine/head based on current weapon
        weaponRotationOffsets = new Vector3[10, 4] {
        {new Vector3(10,0,-90), new Vector3(-7,0,-90), new Vector3(0,0,0), new Vector3(0,0,0) }, //fists
        {new Vector3(15,0,-90), new Vector3(-5,90,-90), new Vector3(0,0,0), new Vector3(0,0,0) }, //pistol
        {new Vector3(20,55,-90), new Vector3(20,45,-90), new Vector3(0,0,0), new Vector3(0,0,0) }, //shotgun
        {new Vector3(20,55,-90), new Vector3(15,85,-90), new Vector3(0,0,0), new Vector3(0,0,0) }, //rifle
        {new Vector3(0,0,0), new Vector3(0,10,0), new Vector3(-15,25,-9), new Vector3(0,0,0) }, //baseball bat
        {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0) },
        {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0,0,0) },
        };



        setRigidbodyState(true);
        setColliderState(true);
        setCharacterControllerState(true);

        SelectWeapon(4);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (health > 0 && !isKnockedOver)
        {
            //No going out of bounds. Sorry :/
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            joypadInputXL = Input.GetAxis(inputManager.controller.lAnalogX);
            joypadInputYL = Input.GetAxis(inputManager.controller.lAnalogY);
            joypadInputYR = Input.GetAxis(inputManager.controller.rAnalogY);
            joypadInputXR = Input.GetAxis(inputManager.controller.rAnalogX);
            joypadInputLT = Input.GetAxisRaw(inputManager.controller.jump);
            joypadInputRT = Input.GetAxisRaw(inputManager.controller.fire);
            jumpAlt = Input.GetButtonDown(inputManager.controller.jumpAlt);
            fireAlt = Input.GetButtonDown(inputManager.controller.fireAlt);
            interact = Input.GetButtonDown(inputManager.controller.interact);
            taunt = Input.GetButtonDown(inputManager.controller.taunt);

            movement = new Vector3(joypadInputXL, 0.0f, 0.0f);

            playerAnim.SetFloat("Speed_f", Mathf.Abs(movement.x));

            pController.Move(movement * speed * Time.deltaTime);

            Jump();
            Attack();

            //Rotate character relative to left analog movement
            ChangeDirection();

            if (Mathf.Abs(joypadInputXL * speed) != 0)
                playerAnim.SetFloat("OverallSpeed_f", Mathf.Abs(joypadInputXL * (speed / 2.3f)));
            else
                playerAnim.SetFloat("OverallSpeed_f", 3);

            if (impact.magnitude > 0.2) 
                pController.Move(impact * Time.deltaTime);

            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        }
        else
        {
            hips.position = new Vector3(hips.transform.position.x, hips.transform.position.y, 0);
        }
    }

    private void LateUpdate()
    {
        if (health > 0 && !isKnockedOver)
        {
            if (hasGun)
            {
                head.LookAt(Target.position);
                spine.LookAt(Target.position);
                spine.rotation = spine.rotation * Quaternion.Euler(spineOffset);
                head.rotation = head.rotation * Quaternion.Euler(headOffset);
            }
            else
            {
                spine.rotation = spine.rotation * Quaternion.Euler(spineOffset);
                head.rotation = head.rotation * Quaternion.Euler(headOffset);
                leftShoulder.rotation = leftShoulder.rotation * Quaternion.Euler(lShoulderOffset);
                rightShoulder.rotation = rightShoulder.rotation * Quaternion.Euler(rShoulderOffset);
            }
        }
    }







    //player methods

    //Checks if the player is on the ground or not
    private bool isGrounded()
    {
        RaycastHit hit;
        Boolean onGround;
        Physics.Raycast(pController.bounds.center, Vector3.down, out hit, pController.bounds.extents.y+0.1f);
        Color rayColor;
        if (hit.collider !=null && (hit.collider.tag.Equals("Ground") || hit.collider.tag.Equals("Player_hurtbox")))
        {
            rayColor = Color.green;
            onGround = true;
        }  
        else
        {
            rayColor = Color.red;
            onGround = false;
        }
        Debug.DrawRay(pController.bounds.center, Vector3.down * (pController.bounds.extents.y+0.1f), rayColor);

        return onGround;
    }

    private bool hittingHead()
    {
        RaycastHit hit;
        Boolean onCeiling;
        Physics.Raycast(pController.bounds.center, Vector3.up, out hit, pController.bounds.extents.y + 0.5f);
        Color rayColor;
        if(hit.collider!=null && hit.collider.tag.Equals("Ground"))
        {
            rayColor = Color.green;
            onCeiling = true;
        }
        else
        {
            rayColor = Color.red;
            onCeiling = false;
        }
        Debug.DrawRay(pController.bounds.center, Vector3.up * (pController.bounds.extents.y + 0.5f), rayColor);

        return onCeiling;
    }

    //Gets the direction that the player should currently be facing and turns them
    private void ChangeDirection()
    {
        //No need to do anything if the player is idling their analog sticks
        if (movement.x != 0 || joypadInputXR != 0)
        {
            //First, check if the player is currently holding a ranged weapon
            if (!hasGun)
            {
                if (movement.x != 0 && !isAttacking) //Only change direction when left analog input is NOT zero.
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
        jumpCheck = isGrounded();
        ceilingCheck = hittingHead();

        //Jump
        if ((joypadInputLT != 0 || jumpAlt) && jumpCheck)
        {
            //check if LT hasn't already been pressed...
            if (joypadInputLTInUse == false)
            {
                isJumping = true;
                jumpCheck = false;
                velocity.y += jumpForce;
                playerAnim.SetTrigger("Jump_trig");
                playerAnim.SetBool("Grounded", false);
                joypadInputLTInUse = true;
            }

        }

        //Jump Zwei
        if ((joypadInputLT != 0 || jumpAlt) && !hasDoubleJumped)
        {
            //check if LT hasn't already been pressed...
            if (joypadInputLTInUse == false)
            {
                doubleJumpParticle.Play();
                hasDoubleJumped = true;

                //You must reset the y velocity back to zero before the second jump, otherwise jump heights will be entirely inconsistent
                velocity.y += doubleJumpForce;
                Debug.Log(velocity.y);
                joypadInputLTInUse = true;
            }
        }

        //Reset LT to pressable?
        if (joypadInputLT == 0)
        {
            joypadInputLTInUse = false;
            isJumping = false;
        }
        else
            joypadInputLTInUse = true;


        if (jumpCheck)
        {
            velocity.y = 0;
            hasDoubleJumped = false;
            playerAnim.SetBool("Grounded", true);
            playerAnim.SetBool("Falling_b", false);
            fallTimer = 0.5f;
            jumpTimeCounter = 0.37f;
        }
        else
        {
            //"You can't fight gravity" -Dutch van der Linde (1911)
            velocity.y += gravityModifier * Time.deltaTime;

            if (ceilingCheck)
            {
                velocity.y = -1f;
                fallTimer = 0;
            }
            else
                fallTimer -= Time.deltaTime;

            pController.Move(velocity * Time.deltaTime);

            if(fallTimer<=0 || velocity.y<0)
                playerAnim.SetBool("Falling_b", true);
        }

    }

    private void Attack()
    {
        if ((joypadInputRT != 0 || fireAlt))
        {
            if (currentWeapon==4)
            {
                if (!isAttacking)
                {
                    GetComponentInChildren<BaseballBat>().Fire();
                    playerAnim.SetTrigger("Shoot_trig");
                }
                    
            }
        }
    }

    public void TakeDamage(int damage, float direction, float force, bool knockOver)
    {
        invunerable = true;
        health -= damage;
        hudManager.updateHealth(health);

        impact += new Vector3(direction, 0f, 0f).normalized * force;

        if (knockOver)
        {
            KnockDown();
            GetComponentInChildren<Rigidbody>().AddForce(impact*50);

            if(health>=1)
                Invoke("GetUp", 1.5f);
        }
          


        if (health >= 4)
            playerRenderer.material.SetColor("_Color", new Color32(251, 176, 59, 255));
        else if (health == 3)
            playerRenderer.material.SetColor("_Color", new Color32(3, 113, 150, 255));
        else if (health == 2)
            playerRenderer.material.SetColor("_Color", new Color32(208, 98, 23, 255));
        else if (health == 1 || health <=0)
            playerRenderer.material.SetColor("_Color", new Color32(111, 12, 16, 255));

        if (health<=0 && !knockOver)
            KnockDown();
       
        Invoke("EndDamage", 2f);
    }

    public void EndDamage()
    {
        invunerable = false;
        playerRenderer.material.SetColor("_Color", Color.white);
    }

    //Knocks player into a ragdoll state
    public void KnockDown()
    {
        isKnockedOver = true;
        //SelectWeapon(0);
        GetComponent<Outline>().enabled = false;
        playerAnim.enabled = false;
        setRigidbodyState(false);
        setColliderState(false);
        setCharacterControllerState(false);

    }

    //Allows the player to get up from a ragdoll state
    public void GetUp()
    {
        GetComponent<Outline>().enabled = true;
        playerAnim.enabled = true;
        playerAnim.SetTrigger("Getup_trig");
        transform.position = hips.position;
        setRigidbodyState(true);
        setColliderState(true);
        setCharacterControllerState(true);

        isKnockedOver = false;

    }


    //See what hud element belongs to us based on playerNumber, then activate it
    public void InitializePlayer()
    {
        //And no.... ("PlayerContainer"+playerNumber) does NOT work.
        if(playerNumber==1)
        {
            hudManager = GameObject.Find("PlayerContainer1").GetComponent<HudManager>();
            GetComponent<Outline>().OutlineColor = new Color32(3, 113, 150, 255);
            this.gameObject.layer = 9;
        }
        else if(playerNumber==2)
        {
            hudManager = GameObject.Find("PlayerContainer2").GetComponent<HudManager>();
            GetComponent<Outline>().OutlineColor = new Color32(111, 12, 16, 255);
            this.gameObject.layer = 10;
        }
        else if (playerNumber==3)
        {
            hudManager = GameObject.Find("PlayerContainer3").GetComponent<HudManager>();
            GetComponent<Outline>().OutlineColor = new Color32(251, 176, 59, 255);
            this.gameObject.layer = 11;
        }
        else
        {
            hudManager = GameObject.Find("PlayerContainer4").GetComponent<HudManager>();
            GetComponent<Outline>().OutlineColor = new Color32(208, 98, 23, 255);
            this.gameObject.layer = 12;
        }

        hudManager.Show();
        hudManager.updateMoney(money);
        hudManager.updateCI(crowdInfluence);
    }


    public void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

    }

    public void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider.GetType() != typeof(CharacterController))
                collider.isTrigger = state;
        }

    }

    public void setCharacterControllerState(bool state)
    {
        CharacterController[] cControllers = GetComponentsInChildren<CharacterController>();

        foreach (CharacterController cController in cControllers)
        {
            cController.enabled = state;
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

        currentWeapon = selectedWeapon;

        if (weaponObjects[selectedWeapon].tag == "Gun")
            hasGun = true;
        else
            hasGun = false;

        //Set our animations
        playerAnim.SetInteger("WeaponType_int", weaponAnimationNumbers[selectedWeapon]);

        //Change our offsets
        spineOffset = weaponRotationOffsets[selectedWeapon, 0];
        headOffset = weaponRotationOffsets[selectedWeapon, 1];
        lShoulderOffset = weaponRotationOffsets[selectedWeapon, 2];
        rShoulderOffset = weaponRotationOffsets[selectedWeapon, 3];

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
            if (interact)
            {
                Destroy(other.gameObject);
                SelectWeapon(1);
            }
        }
        if (other.gameObject.CompareTag("Weapon_Shotgun"))
        {
            if (interact)
            {
                Destroy(other.gameObject);
                SelectWeapon(2);
            }
        }
        if (other.gameObject.CompareTag("Weapon_Rifle"))
        {
            if (interact)
            {
                Destroy(other.gameObject);
                SelectWeapon(3);
            }
        }

    }

}