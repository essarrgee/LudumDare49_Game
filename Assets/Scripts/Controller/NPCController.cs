using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Controller
{
    public float followDelay = 0.2f;
	
    protected float currentFollowDelay;
	protected float currentRandomMovementCooldown;
	protected float currentRandomPauseCooldown;
	
	protected GameObject playerObject;
	protected PlayerController playerController;
	protected Character playerCharacter;
	
	protected float playerDistance;
	protected float previousPlayerDistance;
	
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
		
		currentRandomMovementCooldown = 0;
		currentRandomPauseCooldown = 0;
	}
	
	protected override void Update()
	{
		base.Update();
		
		currentFollowDelay = (currentFollowDelay > 0) ? 
			(currentFollowDelay - Time.deltaTime) : 0;
		currentRandomMovementCooldown = (currentRandomMovementCooldown > 0) ?
			(currentRandomMovementCooldown - Time.deltaTime) : 0;
		currentRandomPauseCooldown = (currentRandomMovementCooldown <= 0) ? (
			(currentRandomPauseCooldown > 0) ?
				(currentRandomPauseCooldown - Time.deltaTime) : 0) 
					: currentRandomPauseCooldown;
		
		if (character != null) {
			character.moveDirection = inputDirection;
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
