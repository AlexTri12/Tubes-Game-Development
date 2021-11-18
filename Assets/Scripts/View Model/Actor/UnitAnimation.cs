using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private const string ATTACK = "Attack";
    private const string SPECIAL_ATTACK = "Special Attack";
    private const string DEFEND = "Defend";
    private const string GET_HIT = "Get Hit";
    private const string DIZZY = "Dizzy";
    private const string KNOCK_OUT = "Knock Out";
    private const string WALK = "Walk";
    private const string RUN = "Run";
    private const string VICTORY = "Victory";

    Animator animator;
    bool isDefend = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk(Movement m)
    {
        switch (GetLocomotion(m))
        {
            case Locomotions.Teleport:
                break;
            case Locomotions.Fly:
                break;
            default:
                animator.SetBool(WALK, true);
                break;
        }
    }

    public void StopWalk()
    {
    }

    public void Run(Movement m)
    {
        switch (GetLocomotion(m))
        {
            case Locomotions.Teleport:
                break;
            case Locomotions.Fly:
                break;
            default:
                animator.SetBool(RUN, true);
                break;
        }
    }

    public void IdleState()
    {
        animator.SetBool(RUN, false);
        animator.SetBool(WALK, false);
        animator.SetBool(DIZZY, false);
        animator.SetBool(DEFEND, false);
        isDefend = false;
    }

    public void ResetTriggers()
    {
        animator.ResetTrigger(ATTACK);
        animator.ResetTrigger(SPECIAL_ATTACK);
        animator.ResetTrigger(GET_HIT);
    }

    public void Die()
    {
        animator.SetBool(KNOCK_OUT, true);
    }

    public void Revive()
    {
        animator.SetBool(KNOCK_OUT, false);
    }

    public void GetDizzy()
    {
        animator.SetBool(DIZZY, true);
    }

    public void GetHit()
    {
        if (!isDefend)
            animator.SetTrigger(GET_HIT);
    }

    public void Attack()
    {
        animator.SetTrigger(ATTACK);
    }

    public void SpecialAttack()
    {
        animator.SetTrigger(SPECIAL_ATTACK);
    }

    public void Defend()
    {
        animator.SetBool(DEFEND, true);
        isDefend = true;
    }

    public void Victory()
    {
        animator.SetBool(VICTORY, true);
    }

    Locomotions GetLocomotion(Movement m)
    {
        if (m is TeleportMovement)
            return Locomotions.Teleport;
        else if (m is FlyMovement)
            return Locomotions.Fly;
        return Locomotions.Walk;
    }
}
