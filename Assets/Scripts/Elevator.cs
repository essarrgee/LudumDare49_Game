using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	public Vector3 startPosition;
	protected Vector3 goalPosition;
	
	protected bool movingUp;
	
    protected virtual void Awake()
	{
		movingUp = false;
		
		SetStartPosition(startPosition);
	}
	
	protected virtual void FixedUpdate()
	{
		if (movingUp) {
			transform.position = Vector3.Lerp(transform.position,
				goalPosition, Time.fixedDeltaTime*0.4f);
		}
		else {
			transform.position = Vector3.Lerp(transform.position,
				startPosition, Time.fixedDeltaTime*2f);
		}
	}
	
	public virtual void SetStartPosition(Vector3 startPosition)
	{
		this.startPosition = startPosition;
		goalPosition = this.startPosition + new Vector3(0,20,0);
	}
	
	protected virtual void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject != null) {
			PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
			if (player != null) {
				movingUp = true;
			}
		}
	}
	
	protected virtual void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject != null) {
			PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
			if (player != null) {
				movingUp = false;
			}
		}
	}
}
