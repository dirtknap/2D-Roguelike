﻿using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start ()
	{
	    boxCollider = GetComponent<BoxCollider2D>();
	    rb2d = GetComponent<Rigidbody2D>();
	    inverseMoveTime = 1f/moveTime;
	}

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        var start = (Vector2) transform.position;
        var end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        var canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null) return;

        var hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        var sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            var newPostion = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime*Time.deltaTime);
            rb2d.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }


    protected abstract void OnCantMove<T>(T component) where T : Component;

}