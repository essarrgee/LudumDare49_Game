using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public SpriteRenderer sprite;
	
	public float movementSpeed = 20f;
	
	public Vector2 moveDirection;
	protected Vector2 faceDirection;
	
	protected Rigidbody rb;
	protected Animator animator;
	protected Controller controller;
	
	protected float lastAnimatorDirection;
	
    protected virtual void Awake()
    {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		controller = GetComponent<Controller>();
		
        moveDirection = new Vector2(0,0);
		faceDirection = new Vector2(0,-1); // Front by default
		lastAnimatorDirection = -1; // Front by default
    }

    protected virtual void Update()
    {	
		if (animator != null) {
			int animatorDirection = 0;
			
			if (moveDirection.sqrMagnitude > 0) {
				faceDirection = moveDirection;
				if (!animator.GetBool("Walking")) {
					animator.SetBool("Walking", true);
				}
			}
			else {
				if (animator.GetBool("Walking")) {
					animator.SetBool("Walking", false);
				}
			}
			
			if (faceDirection.y != 0) {
				animatorDirection = (int)Mathf.Round(faceDirection.y);
			}
			if (lastAnimatorDirection != animatorDirection) {
				animator.SetInteger("Direction", animatorDirection);
				lastAnimatorDirection = animatorDirection;
			}
		}
		
		if (sprite != null) {
			if (faceDirection.x != 0) { // faceDirection.y == 0 && 
				sprite.flipX = (faceDirection.x > 0) ? true : false;
			}
		}
		
    }
	
	protected virtual void FixedUpdate()
	{
		if (rb != null) {
			rb.velocity = 
				new Vector3(moveDirection.x, 0, moveDirection.y).normalized*movementSpeed;
		}
	}
	
	public virtual void DestroyCharacter()
	{
		if (controller != null) {
			controller.DestroyController();
		}
		Destroy(gameObject, 0.1f);
	}
}
