using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    private float cooldown;
    private int damage;
    private int playerCI;
    private int coolnessCI; //excellent...
    private float actualCooldown;
    public bool isAttacking;
    private BoxCollider hitBox;

    void Start()
    {
        hitBox = GetComponent<BoxCollider>();
        hitBox.enabled = false;
        cooldown = 0.75f;
        damage = 1;
        isAttacking = false;
    }

    void FixedUpdate()
    {

        if (actualCooldown<=0)
        {
            transform.root.gameObject.GetComponent<PlayerController>().isAttacking = false;
            isAttacking = false;
            hitBox.enabled = false;
        }
        else
        {
            actualCooldown -= Time.fixedDeltaTime;
        }

    }

    public void Fire()
    {
        transform.root.gameObject.GetComponent<PlayerController>().isAttacking = true;
        isAttacking = true;
        actualCooldown = cooldown;
        hitBox.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player_hurtbox") && (transform.root.gameObject != other.transform.root.gameObject))
        {
            if (!other.transform.root.gameObject.GetComponent<PlayerController>().invunerable)
                other.transform.root.gameObject.GetComponent<PlayerController>().TakeDamage(damage,transform.root.gameObject.transform.rotation.y,5,true);
        }
    }
}
