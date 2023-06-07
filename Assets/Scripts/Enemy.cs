using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { PATROL, FOLLOW }
    public LayerMask whatIsPlayer;
    public State currentState;
    public float visionRange;
    public float wallCheckDistance;
    public float speed;
    protected Transform target;
    protected bool IsInRange;
    public void ChangeState(State nextState)
    {
        currentState = nextState;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.PATROL:
                PatrolUpdate();
                break;
            case State.FOLLOW:
                FollowUpdate();
                break;

        }
    }
    protected virtual void FixedUpdate()
    {
        //Wall check
        //dos opciones o debug ray o por gizmos
        var dir = transform.forward;
        var hit = Physics.Raycast(transform.position, dir, wallCheckDistance);
        Debug.DrawRay(transform.position, dir * wallCheckDistance, hit ? Color.red : Color.green);
        if (hit)
        {
            transform.forward = UnityEngine.Random.value > 0.5f ? transform.right : transform.right * -1;
            //transform.forward = UnityEngine.Random. * Vector3.right;
        }

        switch (currentState)
        {
            case State.PATROL:
                var collider = Physics.OverlapSphere(transform.position, visionRange, whatIsPlayer);
                IsInRange = collider.Length > 0;
                if (IsInRange)
                    target = collider[0].transform;
                break;
            case State.FOLLOW:
                break;
        }



    }
    protected virtual void PatrolUpdate()
    {
        if (target != null) ChangeState(State.FOLLOW);

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void FollowUpdate()
    {
        if (target == null)
            ChangeState(State.PATROL);

        if (Vector2.Distance(transform.position, target.position) > visionRange)
            target = null;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
    }
 
    private void OnDrawGizmosSelected()
    {
        var color = IsInRange ? Color.green : Color.red;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, visionRange);

    
    }


}
