using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{	
	protected float currentHitCooldown;

    protected override void Awake()
	{
		base.Awake();
		
		currentHitCooldown = 0;
	}
	
	protected override void Update()
    {
		base.Update();
		
		currentHitCooldown = (currentHitCooldown > 0) 
			? currentHitCooldown - Time.deltaTime : 0;
		
		if (sprite != null) {
			if (currentHitCooldown > 0) {
				sprite.color = (sprite.color.a == 1f) 
					? new Color(1,0,0,0f) 
						: new Color(1,1,1,1f);
			}
			else {
				sprite.color = 
					new Color(1,1,1,1f);
			}
		}
	}
	
	protected virtual void OnCollisionEnter(Collision collision)
	{
		if (!destroyed && currentHitCooldown <= 0 && collision.gameObject != null) {
			GameObject obj = collision.gameObject;
			Hitbox hitbox = obj.GetComponent<Hitbox>();
			if (hitbox != null) {
				currentHitCooldown = 1f;
				Damage(hitbox.damage, hitbox.transform, 
					hitbox.knockback, hitbox.knockbackTime);
			}
		}
	}
}
