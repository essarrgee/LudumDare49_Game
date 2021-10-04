using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinController : NPCController
{
	public float escapeRange = 5f;
	
	protected override void Awake()
	{
		base.Awake();
	}
	
	protected override void GetInput()
	{
		if (leaderObject != null) {
			Vector3 direction3D = leaderObject.transform.position - transform.position;
			leaderDistance = direction3D.magnitude;
			inputDirection = new Vector2(0,0);
			if (previousLeaderDistance <= 0.7f) {
				currentFollowDelay = followDelay;
			}
			if (leaderDistance > 0.7f && currentFollowDelay <= 0f) {
				inputDirection = new Vector2(direction3D.x, direction3D.z);
			}
			else {
				inputDirection = new Vector2(0,0);
			}
			previousLeaderDistance = leaderDistance;
		}
		else {
			Vector3 direction3D = new Vector3(0,0,0);
			if (playerObject != null) {
				direction3D = 
					transform.position - playerObject.transform.position;
				playerDistance = direction3D.magnitude;
			}
			
			if (playerDistance > escapeRange) {
				if (currentRandomMovementCooldown <= 0 && currentRandomPauseCooldown <= 0) {
					currentRandomMovementCooldown = Random.Range(0.5f, 1f);
					currentRandomPauseCooldown = Random.Range(0.5f, 1f);
					inputDirection.x = Random.Range(-1f,1f);
					inputDirection.y = Random.Range(-1f,1f);
				}
				else if (currentRandomMovementCooldown <= 0
				&& currentRandomPauseCooldown > 0) {
					inputDirection.x = 0;
					inputDirection.y = 0;
				}
			}
			else {
				inputDirection = new Vector2(direction3D.x, direction3D.z);
			}
			previousPlayerDistance = playerDistance;
			
		}
	}
}
