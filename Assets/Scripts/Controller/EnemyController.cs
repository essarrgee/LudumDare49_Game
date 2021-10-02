using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller
{
	protected float followDelay;
	
	protected override void Update()
	{
		base.Update();
		
		if (character != null) {
			character.moveDirection = inputDirection;
		}
		
		followDelay = (followDelay > 0) ? (followDelay - Time.deltaTime) : 0;
	}
	
	protected override void GetInput()
	{
		inputDirection = new Vector2(0,0);
		if (leaderObject != null) {
			Vector3 direction3D = leaderObject.transform.position - transform.position;
			leaderDistance = direction3D.magnitude;
			if (previousLeaderDistance <= 1f) {
				followDelay = 0.3f;
			}
			if (leaderDistance > 1f && followDelay <= 0f) {
				inputDirection = new Vector2(direction3D.x, direction3D.z);
			}
			else {
				inputDirection = new Vector2(0,0);
			}
			previousLeaderDistance = leaderDistance;
		}
	}
}
