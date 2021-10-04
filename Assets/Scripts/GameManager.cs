using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public float timer = 300f;
	protected float currentTime;
	
	public GameObject fadeUIObject;
	protected Animator fadeUIAnimator;
	
	protected GameObject playerObject;
	protected PlayerController playerController;
	protected Character playerCharacter;
	
	protected bool startingNextLevel;
	
	protected virtual void Awake()
	{
		currentTime = timer;
		
		if (fadeUIObject != null) {
			fadeUIAnimator = fadeUIObject.GetComponent<Animator>();
		}
		
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null) {
			playerCharacter = playerObject.GetComponent<Character>();
			playerController = playerObject.GetComponent<PlayerController>();
		}
		
		startingNextLevel = false;
	}
	
	protected virtual void Update()
	{
		currentTime = (currentTime > 0) ? currentTime - Time.deltaTime : 0;
		if (currentTime <= 0) {
			EndGame();
		}
	}
	
	public virtual void StartNextLevel()
	{
		if (!startingNextLevel) {
			startingNextLevel = true;
			if (playerController != null) {
				playerController.lockInput = true;
			}
			if (fadeUIAnimator != null) {
				fadeUIAnimator.SetTrigger("FadeIn");
			}
			StartCoroutine(LoadScene("SceneGame"));
		}
	}
	
	protected virtual IEnumerator LoadScene(string sceneName)
	{
		yield return new WaitForSeconds(2f);
		DataManager.floorLevel++;
		SceneManager.LoadScene(sceneName);
	}
	
	public virtual void EndGame()
	{
		// Game Over screen, option to restart or return to title
	}
}
