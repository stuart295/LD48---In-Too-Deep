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
        }
    }

    private void MoveToPoint(Vector3 destination) {
        if (Vector3.Distance(transform.position, destination) < 0.01f) return;

        Vector3 dir = (destination - transform.position).normalized;

        transform.position = Vector3.Lerp(transform.position, transform.position + dir * moveSpeed, Time.deltaTime);
        LookAt(destination);

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
        }
        else {
            MoveToPoint(target.transform.position);
        }

    }

    private void OnTriggerStay(Collider other) {
        if (target) return;

        Building building = other.transform.root.GetComponent<Building>();
        if (building != null) target = building;
    }


}
