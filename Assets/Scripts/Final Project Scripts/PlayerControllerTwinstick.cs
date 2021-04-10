using UnityEngine;

public class PlayerControllerTwinstick : MonoBehaviour
{
    private Animator playerAnim;
    public CharacterController pController;

    public float joypadInputXL;
    public float joypadInputYL;
    public float joypadInputYR;
    public float joypadInputXR;

    public Vector3 movement;
    public Vector3 lastMovement;
    public Vector3 aimMovement;


    public float speed;
    public bool isAiming;
    public bool hasGun;
    public int currentWeapon;

    public bool isDead;

    public GameObject aimPoint;
    public Transform Target;
    public Transform firingPoint;

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
    public GameObject[] weaponBulletObjects = new GameObject[10];
    public int[] weaponAnimationNumbers = new int[10];
    public Vector3[,] weaponRotationOffsets;

    public Vector3 spineOffset;
    public Vector3 headOffset;
    public Vector3 lShoulderOffset;
    public Vector3 rShoulderOffset;

    private Transform spine;
    private Transform head;
    private Transform leftShoulder;
    private Transform rightShoulder;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAnim.SetFloat("OverallSpeed_f", 3);
        isAiming = false;
        hasGun = false;

        spine = playerAnim.GetBoneTransform(HumanBodyBones.Spine);
        head = playerAnim.GetBoneTransform(HumanBodyBones.Head);
        leftShoulder = playerAnim.GetBoneTransform(HumanBodyBones.LeftShoulder);
        rightShoulder = playerAnim.GetBoneTransform(HumanBodyBones.RightShoulder);

        //Initialize our rotation offsets for spine/head based on current weapon
        weaponRotationOffsets = new Vector3[10, 4] {
        {new Vector3(20,0,-90), new Vector3(20,0,-90), new Vector3(0f,0f,0f), new Vector3(0f,0f,0f) }, //fists
        {new Vector3(25,0,-90), new Vector3(15,100,-90), new Vector3(-60f,0f,0f), new Vector3(-60f,0f,0f) }, //pistol
        {new Vector3(15,60,-75), new Vector3(12,60,-90), new Vector3(317f,12.96f,-16.6f), new Vector3(-25.4f,4.8f,-34.37f) }, //shotgun
        {new Vector3(10,60,-70), new Vector3(20,95,-90), new Vector3(-47.87f,50.5f,0f), new Vector3(-47.87f,33.51f,0f) }, //rifle
        {new Vector3(15,60,-75), new Vector3(12,60,-90), new Vector3(-27.7f,40.9f,-3f), new Vector3(-35f,29.38f,0f) }, //assualt rifle
        {new Vector3(15,60,-75), new Vector3(12,60,-90), new Vector3(0f,0f,0f), new Vector3(0f,0f,0f) }, //SMG
        {new Vector3(15,60,-75), new Vector3(12,60,-90), new Vector3(317f,12.96f,-16.6f), new Vector3(-25.4f,4.8f,-34.37f) }, //laser shotgun
        {new Vector3(25,0,-90), new Vector3(15,100,-90), new Vector3(-60f,0f,0f), new Vector3(-60f,0f,0f) }, //space gun
        {new Vector3(0,0,0), new Vector3(0,0,0), new Vector3(0f,0f,0f), new Vector3(0f,0f,0f) },
        {new Vector3(20,0,-90), new Vector3(20,90,-90), new Vector3(0f,0f,0f), new Vector3(0f,0f,0f) }, //idle position (when you're not aiming, only 0 & 1)
        };

        setRigidbodyState(true);
        setColliderState(false);
        setCharacterControllerState(true);

        isDead = false;

        SelectWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {

            joypadInputXL = (Input.GetAxis("MoveHorizontal1"));
            joypadInputYL = (Input.GetAxis("MoveVertical1"));
            joypadInputYR = (Input.GetAxis("AimVertical1"));
            joypadInputXR = (Input.GetAxis("AimHorizontal1"));

            movement = new Vector3(joypadInputXL, 0.0f, joypadInputYL);
            aimMovement = new Vector3(joypadInputXR, 0.0f, -joypadInputYR);

            if (movement.x != 0 || movement.z != 0)
            {
                playerAnim.SetFloat("Speed_f", 1);

            }
            else
            {
                playerAnim.SetFloat("Speed_f", 0);
            }



            pController.Move(movement * speed * Time.deltaTime);

            if (aimMovement.x != 0 || aimMovement.z != 0)
            {
                isAiming = true;
                lastMovement = aimMovement;
                transform.rotation = Quaternion.LookRotation(aimMovement);
            }
            else
            {
                isAiming = false;
                if (movement.x != 0 || movement.z != 0)
                {
                    lastMovement = movement;
                    transform.rotation = Quaternion.LookRotation(movement);
                }
                else
                    transform.rotation = Quaternion.LookRotation(lastMovement);

            }


            if (isAiming && hasGun)
            {
                fireWeapon();
                playerAnim.SetBool("Shoot_b", true);
                playerAnim.SetBool("FullAuto_b", true);
            }
            else
            {
                playerAnim.SetBool("Shoot_b", false);
                playerAnim.SetBool("FullAuto_b", false);
            }






            //DEBUGGING - REMOVE LATER!
            if (Input.GetKeyDown(KeyCode.Alpha0))
                SelectWeapon(0);
            else if (Input.GetKeyDown(KeyCode.Alpha1))
                SelectWeapon(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SelectWeapon(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SelectWeapon(3);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                SelectWeapon(4);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                SelectWeapon(5);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                SelectWeapon(6);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                SelectWeapon(7);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                SelectWeapon(8);
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                SelectWeapon(9);

        }

        if (transform.position.y<0)
        {
            isDead = true;
            KnockDown();
        }
    }
    private void LateUpdate()
    {
        if (!isDead)
        {
            head.LookAt(Target.position);
            spine.LookAt(Target.position);

            if (isAiming || !hasGun)
            {
                spine.rotation = spine.rotation * Quaternion.Euler(spineOffset);
                head.rotation = head.rotation * Quaternion.Euler(headOffset);
            }
            else
            {
                spine.rotation = spine.rotation * Quaternion.Euler(weaponRotationOffsets[9, 0]);
                head.rotation = head.rotation * Quaternion.Euler(weaponRotationOffsets[9, 1]);
                leftShoulder.rotation = leftShoulder.rotation * Quaternion.Euler(lShoulderOffset);
                rightShoulder.rotation = rightShoulder.rotation * Quaternion.Euler(rShoulderOffset);
            }

        }

    }

    //Enables weapon model and corresponding animation
    //param 1: int      - the weapon in question we want to select
    private void SelectWeapon(int selectedWeapon)
    {
        //Set our new weapon to true
        weaponBools[selectedWeapon] = true;
        weaponObjects[selectedWeapon].SetActive(true);

        //Get where the bullets shoot from
        firingPoint = weaponObjects[selectedWeapon].transform.Find("firingpoint");

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

    private void fireWeapon()
    {
        GameObject bullet = Instantiate(weaponBulletObjects[currentWeapon], firingPoint.position, Quaternion.identity);

        Vector3 shootDir = firingPoint.eulerAngles;

        bullet.GetComponent<bullet>().Setup(shootDir);

    }

    //Knocks player into a ragdoll state
    public void KnockDown()
    {
        //SelectWeapon(0);
        playerAnim.enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        setCharacterControllerState(false);

    }

    //Allows the player to get up from a ragdoll state
    public void GetUp()
    {
        playerAnim.enabled = true;
        playerAnim.SetTrigger("Getup_trig");
        setRigidbodyState(true);
        setColliderState(false);
        setCharacterControllerState(true);

    }

    public void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

    }

    public void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
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
}
