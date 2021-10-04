using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	public Vector3 startPosition;
	protected Vector3 goalPosition;
	
	protected bool movingUp;
	
	protected GameObject gameManagerObject;
	protected GameManager gameManager;
	protected bool endedScene;
	
	public GameObject activateLightObject;
	protected Light activateLight;
	
	protected AudioManager audioManager;
	
	public bool activated = false;
	
    protected virtual void Awake()
	{
		movingUp = false;
		
		SetStartPosition(startPosition);
		
		if (activateLightObject != null) {
			activateLight = activateLightObject.GetComponent<Light>();
		}
		
		gameManagerObject = GameObject.FindWithTag("GameManager");
		if (gameManagerObject != null) {
			gameManager = gameManagerObject.GetComponent<GameManager>();
		}
		
		audioManager = GetComponent<AudioManager>();
		
		endedScene = false;
	}
	
	protected virtual void Update()
	{
		if (Mathf.Abs(goalPosition.y - transform.position.y) <= 15f 
		&& movingUp && !endedScene && gameManager != null) {
			// End Scene, load next level
			endedScene = true;
			gameManager.StartNextLevel();
		}
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
		if (collision.gameObject != null && activated) {
			PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
			if (player != null) {
				movingUp = true;
				if (audioManager != null) {
					audioManager.Play("Elevator");
				}
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
	
	public virtual void Activate()
	{
		if (activateLight != null) {
			activated = true;
			activateLightObject.SetActive(true);
		}
	}
}
