using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller
{
	public float aggroRange = 12f;
	public float chaseRange = 30f;
	public float followDelay = 0.3f;
	
    protected float currentFollowDelay;
	
	protected GameObject playerObject;
	protected PlayerController playerController;
	protected Character playerCharacter;
	
	protected float playerDistance;
	protected float previousPlayerDistance;
	
	protected bool aggro;
	
	protected override void Awake()
	{
		base.Awake();
		
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null) {
			playerCharacter = playerObject.GetComponent<Character>();
			playerController = playerObject.GetComponent<PlayerController>();
		}
		
		playerDistance = 20;
		previousPlayerDistance = 20;
		aggro = false;
	}
	
	protected override void Update()
	{
		base.Update();
		
		if (character != null) {
			character.moveDirection = inputDirection;
		}
		
		currentFollowDelay = (currentFollowDelay > 0) ? (currentFollowDelay - Time.deltaTime) : 0;
	}
	
	protected override void GetInput()
	{
		inputDirection = new Vector2(0,0);
		if (playerObject != null) {
			Vector3 direction3D = playerObject.transform.position - transform.position;
			playerDistance = direction3D.magnitude;
			if (previousPlayerDistance >= chaseRange && !aggro) {
				currentFollowDelay = followDelay;
			}
			if (playerDistance <= aggroRange) {
				aggro = true;
			}
			if (playerDistance > chaseRange) {
				aggro = false;
			}
			if (aggro && currentFollowDelay <= 0f) {
				inputDirection = new Vector2(direction3D.x, direction3D.z);
			}
			else {
				inputDirection = new Vector2(0,0);
			}
			previousPlayerDistance = playerDistance;
		}
	}
	
	protected virtual void GetPlayerDistance()
	{
		if (playerObject != null) {
			previousPlayerDistance = playerDistance;
			playerDistance = 
				(transform.position-playerObject.transform.position).magnitude;
		}
	}
}
