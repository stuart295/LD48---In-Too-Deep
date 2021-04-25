using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed = 1f;
    public float turnSpeed = 1f;


    [Header("Attack")]
    public float attackRange = 3f;
    public float attackDamage = 1f;
    public float attackCooldown = 1f;

    [Header("Other")]
    public float health = 1f;
    public Animator anim;
    public GameObject deathEffect;

    private Vector3 destination = Vector3.zero;

    private Building target;
    private float lastAttackTime = 0f;

    public Vector3 Destination { get => destination; set => destination = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target) {
            AttackTarget();
        }
        else {
            MoveToPoint(destination);

            //If reach destination, head to space port
            if (ReachedPoint(destination)) {
                Destination = Vector3.zero;
            }
        }

    }

    private bool ReachedPoint(Vector3 point) {
        return Vector3.Distance(transform.position, point) < 0.01f;
    }

    private void SetMovingAnim(bool moving = true) {
        anim.SetBool("Moving", moving);
    }

    private void MoveToPoint(Vector3 destination) {
        if (ReachedPoint(destination)) {
            SetMovingAnim(false);
            return;
        }

        Vector3 dir = (destination - transform.position).normalized;

        transform.position = Vector3.Lerp(transform.position, transform.position + dir * moveSpeed, Time.deltaTime);
        LookAt(destination);
        SetMovingAnim(true);

    }

    public void TakeDamage(float attackDamage) {
        health -= attackDamage;
        Debug.Log(gameObject + " took " + attackDamage + " damage", gameObject);
        if (health <= 0) {
            OnDeath();
            Destroy(gameObject);
        }

    }

    private void OnDeath() {
        Debug.Log(gameObject + " died");
        Instantiate(deathEffect, transform.position, Quaternion.identity);
    }

    private void LookAt(Vector3 position) {
        Vector3 dir = (position - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, dir * turnSpeed, Time.deltaTime);
        transform.forward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    }

    private void AttackTarget() {
        if (target == null || target.gameObject == null) {
            target = null;
            return;
        }
        
        if (Vector3.Distance(transform.position, target.transform.position) <= attackRange) {
            LookAt(target.transform.position);
            if (Time.time - lastAttackTime > attackCooldown) {
                lastAttackTime = Time.time;
                target.TakeDamage(attackDamage);
            }
            SetMovingAnim(false);
        }
        else {
            MoveToPoint(target.transform.position);
        }

    }

    private void OnTriggerStay(Collider other) {
        Building building = other.transform.root.GetComponent<Building>();
        if (building != null && !building.Placing) {
            if (target == null || Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, target.transform.position))
                target = building;
        }
    }


}
