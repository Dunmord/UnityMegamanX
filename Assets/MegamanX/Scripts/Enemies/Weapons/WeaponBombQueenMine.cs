﻿using UnityEngine;
using System.Collections;

public class WeaponBombQueenMine : MonoBehaviour
{
	private Rigidbody2D myRigidbody2D = null;
	public Animator myAnimator = null;
	public float explosionDelay = 2f;

	void Awake()
	{
		myRigidbody2D = GetComponent<Rigidbody2D> ();
	}

	void OnCollisionEnter2D(Collision2D localCollider2D)
	{
		if(myRigidbody2D.velocity.x != 0)
		{
			myAnimator.SetBool("armed", true);
			myRigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
			Invoke ("Explode", explosionDelay);
		}
	}

	void Explode()
	{
		Destroy (gameObject);
	}
}
