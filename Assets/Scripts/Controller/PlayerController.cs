using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
	protected override void Update()
	{
		base.Update();
		if (character != null) {
			character.moveDirection = inputDirection;
		}
	}
	
	
	protected override void GetInput()
	{
		if (!lockInput) {
			float inputX = Input.GetAxisRaw("Horizontal");
			float inputY = Input.GetAxisRaw("Vertical");
			inputDirection = new Vector2(inputX, inputY);
		}
		else {
			inputDirection = new Vector2(0, 0);
		}
	}
}
