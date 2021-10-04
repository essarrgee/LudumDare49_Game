using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : NPCController
{
	public float aggroRange = 12f;
	public float chaseRange = 30f;
	
	protected bool aggro;
	
	protected override void Awake()
	{
		base.Awake();
		
		aggro = false;
	}
	
	protected override void Update()
	{
		base.Update();
	
	}
	
	protected override void GetInput()
	{
		// inputDirection = new Vector2(0,0);
		if (playerObject != null && playerCharacter != null) {
			Vector3 direction3D = playerObject.transform.position - transform.position;
			playerDistance = direction3D.magnitude;
			
			if (previousPlayerDistance > chaseRange && !aggro) {
				currentFollowDelay = followDelay;
				
				if (currentRandomMovementCooldown <= 0 && currentRandomPauseCooldown <= 0) {
					currentRandomMovementCooldown = Random.Range(0.2f, 0.4f);
					currentRandomPauseCooldown = Random.Range(0.5f, 2f);
					inputDirection.x = Random.Range(-1f,1f);
					inputDirection.y = Random.Range(-1f,1f);
				}
				else if (currentRandomMovementCooldown <= 0
				&& currentRandomPauseCooldown > 0) {
					inputDirection.x = 0;
					inputDirection.y = 0;
				}
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
				// inputDirection = new Vector2(0,0);
			}
			
			previousPlayerDistance = playerDistance;
		}
	}
}
