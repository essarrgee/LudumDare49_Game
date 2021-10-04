using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
	public GameObject fadeUIObject;
	protected Animator fadeUIAnimator;
	
	protected bool startingNextLevel;
	protected float inputCooldown;
	
	protected AudioManager audioManager;
	
	protected virtual void Awake()
	{
		if (fadeUIObject != null) {
			fadeUIAnimator = fadeUIObject.GetComponent<Animator>();
		}
		
		audioManager = GetComponent<AudioManager>();
		
		startingNextLevel = false;
		inputCooldown = 0.6f;
	}
	
    protected virtual void Update()
	{
		inputCooldown = (inputCooldown > 0) ? inputCooldown - Time.deltaTime : 0;
		if (inputCooldown <= 0 && Input.GetKeyDown(KeyCode.Return)) {
			StartGame();
		}
	}
	
	protected virtual IEnumerator LoadScene(string sceneName)
	{
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(sceneName);
	}
	
	public virtual void StartGame()
	{
		if (!startingNextLevel) {
			startingNextLevel = true;
			if (audioManager != null) {
				audioManager.Play("Select");
			}
			if (fadeUIAnimator != null) {
				fadeUIAnimator.SetTrigger("FadeIn");
			}
			DataManager.floorLevel = 1;
			StartCoroutine(LoadScene("SceneGame"));
		}
	}
}
