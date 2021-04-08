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
        damage = 2;
        isAttacking = false;
    }

    void FixedUpdate()
    {

        if (actualCooldown<=0)
        {
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
        isAttacking = true;
        actualCooldown = cooldown;
        hitBox.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && (transform.root.gameObject != other.gameObject))
        {
            if (!other.gameObject.GetComponent<PlayerController>().invunerable)
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
