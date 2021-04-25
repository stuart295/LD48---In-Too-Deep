using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{

    [Header("Attack settings")]
    public float attackRange = 10f;
    public float attackDamage = 1f;
    public float attackDelay = 0.5f;

    [Header("Turret settings")]
    public GameObject turret;
    public ParticleSystem muzzleFlash;
    public float aimSpeed = 1f;
    public float aimThresh = 0.05f;



    private Alien target;
    private float lastAttackTime = 0f;


    public override void FinishPlacing() {
        base.FinishPlacing();
        GetComponent<SphereCollider>().radius = attackRange;
    }

    private void OnTriggerStay(Collider other) {
        Alien alien = other.transform.root.GetComponent<Alien>();
        if (alien != null && (target == null || Vector3.Distance(transform.position, alien.transform.position) < Vector3.Distance(transform.position, target.transform.position))) target = alien;
    }

    public override void Update() {
        if (!gm || gm.GameOver || placing) return;
        base.Update();
        UpdateAttack();
    }

    private void UpdateAttack() {
        if (target == null || target.gameObject == null) return;
        if (Vector3.Distance(transform.position, target.transform.position) > attackRange) {
            target = null;
            return;
        }

        //Follow target
        bool aiming = FollowTarget();

        if (!aiming || Time.time - lastAttackTime < attackDelay) return;

        muzzleFlash.Play(true);
        target.TakeDamage(attackDamage);
        lastAttackTime = Time.time;

    }

    private bool FollowTarget() {
        Vector3 aimDir = (target.transform.position - turret.transform.position).normalized;
        turret.transform.forward = Vector3.MoveTowards(turret.transform.forward, aimDir, Time.deltaTime * aimSpeed);

        return (turret.transform.forward - aimDir).magnitude < aimThresh;
    }


}
