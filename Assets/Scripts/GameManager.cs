using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
	public float timer = 300f;
	protected float currentTime;
	
	public GameObject fadeUIObject;
	protected Animator fadeUIAnimator;
	public GameObject gameOverUIObject;
	protected Animator gameOverUIAnimator;
	public TextMeshProUGUI timerUI;
	public TextMeshProUGUI floorUI;
	public TextMeshProUGUI highScoreUI;
	
	protected GameObject playerObject;
	protected PlayerController playerController;
	protected Character playerCharacter;
	
	protected bool startingNextLevel;
	protected bool gameOver;
	
	public GameObject lavaObject;
	protected float lavaHeight;
	protected float timeMod;
	
	protected virtual void Awake()
	{
		currentTime = timer;
		
		if (fadeUIObject != null) {
			fadeUIAnimator = fadeUIObject.GetComponent<Animator>();
		}
		if (gameOverUIObject != null) {
			gameOverUIAnimator = gameOverUIObject.GetComponent<Animator>();
		}
		
		playerObject = GameObject.FindWithTag("Player");
		if (playerObject != null) {
			playerCharacter = playerObject.GetComponent<Character>();
			playerController = playerObject.GetComponent<PlayerController>();
		}
		
		startingNextLevel = false;
		gameOver = false;
		lavaHeight = -20f;
		timeMod = 1f;
	}
	
	protected virtual void Update()
	{
		currentTime = (currentTime > 0) ? currentTime - (Time.deltaTime*timeMod) : 0;
		lavaHeight = ((currentTime)/timer)*-20f+0.61f;
		if (timerUI != null) {
			timerUI.text = currentTime.ToString("000.00");
		}
		if (floorUI != null && !gameOver) {
			floorUI.text = "Floor " + DataManager.floorLevel.ToString();
		}
		if (highScoreUI != null && !gameOver) {
			highScoreUI.text = "Floor: " + DataManager.floorLevel.ToString();
		}
		if (lavaObject != null) {
			lavaObject.transform.position =
				new Vector3(
					lavaObject.transform.position.x,
					lavaHeight,
					lavaObject.transform.position.z
				);
		}
		if (currentTime <= 0) {
			EndGame();
		}
		if (gameOver && Input.GetKeyDown(KeyCode.Return)) {
			RestartGame();
		}
	}
	
	public virtual float GetLavaHeight()
	{
		return lavaHeight;
	}
	
	public virtual void StartNextLevel()
	{
		if (!startingNextLevel) {
			startingNextLevel = true;
			timeMod = 500f;
			if (playerController != null) {
				playerController.lockInput = true;
			}
			if (fadeUIAnimator != null) {
				fadeUIAnimator.SetTrigger("FadeIn");
			}
			DataManager.floorLevel++;
			StartCoroutine(LoadScene("SceneGame"));
		}
	}
	
	protected virtual IEnumerator LoadScene(string sceneName)
	{
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(sceneName);
	}
	
	public virtual void EndGame()
	{
		// Game Over screen, option to restart or return to title
		if (!startingNextLevel && !gameOver) {
			gameOver = true;
			if (playerController != null) {
				playerController.lockInput = true;
			}
			if (gameOverUIAnimator != null) {
				gameOverUIAnimator.SetTrigger("GameOver");
			}
		}
	}
	
	public virtual void RestartGame()
	{
		if (!startingNextLevel) {
			startingNextLevel = true;
			if (playerController != null) {
				playerController.lockInput = true;
			}
			if (fadeUIAnimator != null) {
				fadeUIAnimator.SetTrigger("FadeIn");
			}
			DataManager.floorLevel = 1;
			print(DataManager.floorLevel);
			StartCoroutine(LoadScene("SceneGame"));
		}
	}
}
