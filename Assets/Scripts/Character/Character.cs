using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public SpriteRenderer sprite;
	
	public int maxHealth = 100;
	public float movementSpeed = 20f;
	
	protected int currentHealth;
	
	public Vector2 moveDirection;
	protected Vector2 faceDirection;
	
	protected Rigidbody rb;
	protected Animator animator;
	protected Controller controller;
	
	protected float lastAnimatorDirection;
	
	protected float knockbackTime;
	protected bool destroyed;
	
	protected GameObject gameManagerObject;
	protected GameManager gameManager;
	
    protected virtual void Awake()
    {
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		controller = GetComponent<Controller>();
		
		currentHealth = maxHealth;
		
        moveDirection = new Vector2(0,0);
		faceDirection = new Vector2(0,-1); // Front by default
		lastAnimatorDirection = -1; // Front by default
		
		knockbackTime = 0;
		destroyed = false;
		
		gameManagerObject = GameObject.FindWithTag("GameManager");
		if (gameManagerObject != null) {
			gameManager = gameManagerObject.GetComponent<GameManager>();
		}
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
		
		if (!destroyed) {
			if (//(!destroyed && currentHealth <= 0) || 
			(gameManager != null && transform.position.y <= gameManager.GetLavaHeight())
			|| (transform.position.y <= -15f)) {
				DestroyCharacter();
			}
		}
		
		knockbackTime = (knockbackTime > 0) ? knockbackTime - Time.deltaTime : 0;
		
    }
	
	protected virtual void FixedUpdate()
	{
		if (rb != null && knockbackTime <= 0) {
			Vector3 newVelocity = new Vector3(
				moveDirection.x, 0, moveDirection.y).normalized*movementSpeed;
			rb.velocity = (!destroyed) ?
				new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z)
					: new Vector3(0,0,0);
		}
	}
	
	public virtual void DestroyCharacter()
	{
		if (!destroyed) {
			destroyed = true;
			
			if (controller != null) {
				controller.DestroyController();
			}
			if (animator != null) {
				animator.SetBool("Destroy", true);
			}
			
			Destroy(gameObject, 3f);
		}
	}
	
	public virtual void Damage(int damage)
	{
		if (!destroyed) {
			currentHealth = (currentHealth + damage > 0) ? currentHealth + damage
				: 0;
		}
	}
	
	public virtual void Damage(int damage, Transform hitbox, float knockback, 
	float knockbackTime)
	{
		if (!destroyed && knockback != 0 && rb != null && hitbox != null) {
			this.knockbackTime = knockbackTime;
			rb.AddForce(
				(transform.position - hitbox.position).normalized*knockback,
				ForceMode.Impulse);
		}
		
		Damage(damage);
	}
}
