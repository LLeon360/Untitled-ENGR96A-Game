using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UnitInfoScript))]

public class TowerScript : MonoBehaviour
{
    private Animator animator;
    private UnitInfoScript unitInfo;

    [SerializeField]
    private LineRenderer lightning;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float nextAttackTime;

    [SerializeField] //for debug
    private string state;
    [SerializeField]
    private int damage; //if the unit does area damage

    void Start()
    {
        animator = GetComponent<Animator>();
        unitInfo = GetComponent<UnitInfoScript>();
        nextAttackTime = 0f;
    }

    void Update()
    {
        //default idle
        state = "Idle";
        //if can fire, check for units and update state if there are enemies in range
        if (Time.time >= nextAttackTime)
        {
            CheckInFront();
        }

        //update animator
        animator.SetBool("Attack", state == "Attack");
    }

    void CheckInFront()
    {
        GameObject thisLane = unitInfo.GetLane();
        Transform unitsInLane = thisLane.transform.Find("Units");

        //iterate through children of unitsInLane
        foreach (Transform unit in unitsInLane.transform)
        {
            //check that it is in front based on player number
            if (unitInfo.player == 0)
            {
                if (unit.position.x < transform.position.x)
                {
                    continue;
                }
            }
            else if (unitInfo.player == 1)
            {
                if (unit.position.x > transform.position.x)
                {
                    continue;
                }
            }

            //check that it is an enemy
            if (unit.GetComponent<UnitInfoScript>().player == unitInfo.player)
            {
                continue;
            }

            float distance = Mathf.Abs(transform.position.x - unit.transform.position.x);

            if (distance < attackRange)
            {
                state = "Attack";
                nextAttackTime = Time.time + attackCooldown;
                //only needs to find one unit to attack
                break;
            }
        }
    }

    public void FireProjectile()
    {
        LineRenderer newProjectile = Instantiate(lightning);
        newProjectile.GetComponent<ProjectileScript>().player = unitInfo.player;
        foreach (Transform unit in unitInfo.GetLane().transform.Find("Units").transform)
        {
            newProjectile.SetPosition(0, unit.position);
            newProjectile.SetPosition(1, transform.position);
        }

    }

    public void AttackInArea()
    {
        GameObject thisLane = unitInfo.GetLane();
        Transform unitsInLane = thisLane.transform.Find("Units");

        //get all units within range
        foreach (Transform unit in unitsInLane.transform)
        {
            //check that it is in front based on player number
            if (unitInfo.player == 0)
            {
                if (unit.position.x < transform.position.x)
                {
                    continue;
                }
            }
            else if (unitInfo.player == 1)
            {
                if (unit.position.x > transform.position.x)
                {
                    continue;
                }
            }

            //check that it is an enemy
            if (unit.GetComponent<UnitInfoScript>().player == unitInfo.player)
            {
                continue;
            }

            float distance = Mathf.Abs(transform.position.x - unit.transform.position.x);

            if (distance < attackRange)
            {
                unit.GetComponent<HealthScript>().Damage(damage, gameObject);
            }
        }
        state = "Idle";
    }

    //instakills a unit and itself
    public void Mousetrap()
    {
        GameObject thisLane = unitInfo.GetLane();
        Transform unitsInLane = thisLane.transform.Find("Units");
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = null;
        //iterate through children of unitsInLane
        foreach (Transform unit in unitsInLane.transform)
        {
            //check that it is an enemy
            if (unit.GetComponent<UnitInfoScript>().player == unitInfo.player)
            {
                continue;
            }

            float distance = Mathf.Abs(transform.position.x - unit.transform.position.x);

            if (distance < attackRange)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = unit.gameObject;
                }
            }
        }

        if (closestUnit != null)
        {
            closestUnit.GetComponent<HealthScript>().Damage(1000, gameObject);
        }
        Destroy(gameObject);
    }
}
