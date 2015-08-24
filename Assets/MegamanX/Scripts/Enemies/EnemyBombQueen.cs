﻿using UnityEngine;
using System.Collections;

public class EnemyBombQueen : Enemy
{
	private Rigidbody2D myRigidbody2D = null;
	private Vector2 moveVector = Vector2.zero;
	public Animator myAnimator = null;
	public GameObject bombBullet = null;
	public float damage = 1f;
	public float speed = 1f;
	public bool moveLeft = true;
	public float detectionRadius = 1f;
	public bool canDropBomb = true;
	private bool canStartDroppingBombs = true;

	protected override void Awake()
	{
		base.Awake ();
		myRigidbody2D = GetComponent<Rigidbody2D> ();
	}
	
	void Start()
	{
	}

	IEnumerator StartDroppingBombs(GameObject localGameObject)
	{
		DropBomb (localGameObject, 150f);
		yield return new WaitForSeconds (0.5f);
		canDropBomb = true;
		DropBomb (localGameObject, 100f);
		yield return new WaitForSeconds (0.5f);
		canDropBomb = true;
		DropBomb (localGameObject, 50f);
	}

	void DropBomb(GameObject localGameObject, float force)
	{
		if(localGameObject != null)
		{
			if(canDropBomb)
			{
				canDropBomb = false;
				MegamanController megaman = localGameObject.GetComponent<MegamanController> ();

				if(megaman != null)
				{
					GameObject bombInstance = Instantiate(bombBullet, bombBullet.transform.position, bombBullet.transform.rotation) as GameObject;
					Vector3 moveVector = localGameObject.transform.position - bombInstance.transform.position;
					Rigidbody2D bombRigidbody = bombInstance.GetComponent<Rigidbody2D>();
					bombInstance.SetActive(true);
					bombRigidbody.AddForce(new Vector2(moveVector.x, moveVector.y) * force);
				}
			}
		}
	}

	void Update()
	{
		if(canStartDroppingBombs)
		{
			Collider2D[] allOverlaps = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), detectionRadius, collidableLayers);
			if(allOverlaps != null)
			{
				foreach(Collider2D c2D in allOverlaps)
				{
					canStartDroppingBombs = false;
					StartCoroutine(StartDroppingBombs(c2D.gameObject));
					break;
				}
			}
		}
	}

	void FixedUpdate()
	{
		if(moveLeft)
		{
			moveVector.x = transform.right.x * -1 * speed;
			moveVector.y = 0f;
		}
		else
		{
			moveVector.x = transform.right.x * speed;
			moveVector.y = 0f;
		}
		myRigidbody2D.velocity = moveVector;
//		myAnimator.SetFloat ("hSpeed", speed);
	}
	
	void OnTriggerEnter2D(Collider2D localCollision2D)
	{
		if(CanCollideWith(localCollision2D.gameObject))
		{
			Hitpoints hitpoints = localCollision2D.gameObject.GetComponent<Hitpoints>();
			if(hitpoints != null)
			{
				hitpoints.Damage(damage, gameObject, gameObject);
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere (transform.position, detectionRadius);
	}
}