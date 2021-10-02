using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Character character;
	
	protected Vector2 inputDirection;
	public bool lockInput = false;
	
	public GameObject leaderObject;
	protected Controller leaderController;
	public GameObject followerObject;
	protected Controller followerController;
	protected float leaderDistance;
	protected float previousLeaderDistance;
	
	protected virtual void Awake()
    {
        character = GetComponent<Character>();
		inputDirection = new Vector2(0,0);
		
		SetLeader(leaderObject);
		SetFollower(followerObject);
		
		leaderDistance = 0;
		previousLeaderDistance = 0;
    }
	
    protected virtual void Update()
    {
        GetInput();
    }

	protected virtual void GetInput()
	{
		
	}
	
	public virtual void SetLeader(GameObject leaderObject)
	{
		this.leaderObject = leaderObject;
		if (this.leaderObject != null) {
			leaderController = this.leaderObject.GetComponent<Controller>();
			if (leaderController != null) {
				leaderController.SetFollower(gameObject);
			}
		}
	}
	
	public virtual void SetFollower(GameObject followerObject)
	{
		this.followerObject = followerObject;
		if (this.followerObject != null) {
			followerController = this.followerObject.GetComponent<Controller>();
		}
	}
	
	public virtual void DestroyController()
	{
		if (leaderController != null) {
			leaderController.SetFollower(followerObject);
		}
		if (followerController != null) {
			followerController.SetLeader(leaderObject);
		}
	}
	
	protected virtual void GetLeaderDistance()
	{
		if (leaderObject != null) {
			previousLeaderDistance = leaderDistance;
			leaderDistance = 
				(transform.position-leaderObject.transform.position).magnitude;
		}
	}
}
