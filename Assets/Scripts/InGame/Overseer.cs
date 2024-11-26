using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Overseer : MonoBehaviour
{
	[SerializeField] GameObject pauseScreen;
	[SerializeField] GameObject centerScreen;
	[SerializeField] GameObject endGameScreen;

	[SerializeField] TextMeshProUGUI lightScoreText;
	[SerializeField] TextMeshProUGUI darkScoreText;

	[SerializeField] GameObject pauseEventSystem;
	[SerializeField] GameObject gameendEventSystem;

	TextMeshProUGUI centerTextBox;
	TextMeshProUGUI endGameText;

	int lightScore = 0;
	int darkScore = 0;

	//int lightBallActiveCount;		//handled by ActiveBallCountLight and Dark
	//int darkBallActiveCount;

	bool isPaused = false;

	public List<BallBehav> darkBalls = new List<BallBehav>();
	public List<BallBehav> lightBalls = new List<BallBehav>();

	public void Start()
	{
		Resume();
		EventScript.PauseGame.AddListener(PauseCheck);
		EventScript.ResumeGame.AddListener(Resume);
		EventScript.NewRound.AddListener(NewRound);
		EventScript.MultiballCall.AddListener(SpawnMultiball);

		VariableInitialize();

		//NewRound();


										StartCoroutine(Begin());//////////////////////
	}

	public void VariableInitialize()
	{
		lightScore = 0;
		darkScore = 0;
		lightScoreText.text = lightScore.ToString();
		darkScoreText.text = darkScore.ToString();
		centerTextBox = centerScreen.GetComponentInChildren<TextMeshProUGUI>();
		endGameText = endGameScreen.GetComponentInChildren<TextMeshProUGUI>();
		centerScreen.SetActive(false);
		endGameScreen.SetActive(false);

		foreach (var obj in FindObjectsByType<BallBehav>(FindObjectsInactive.Include, FindObjectsSortMode.None))
		{
			if (obj.currentTeam == BallBehav.Team.Dark)
			{
				darkBalls.Add(obj);
			}
			else if (obj.currentTeam == BallBehav.Team.Light)
			{
				lightBalls.Add(obj);
			}
		}

		foreach (var obj in darkBalls)
		{/*After we collect the list, disable all of the clones in the clone pool*/
			if (!obj.isOGBall)
			{
				obj.gameObject.SetActive(false);
			}
		}
		foreach (var obj in lightBalls)
		{
			if (!obj.isOGBall)
			{
				obj.gameObject.SetActive(false);
			}
		}
		//Stick the OG balls to the paddles at the start

		//Debug.Log(darkBalls[0].name);
		//Debug.Log(lightBalls[0].name);
	}

	public void NewRound()
	{
		StartCoroutine(Countdown3());
	}
	public IEnumerator Countdown3()
	{
		yield return new WaitForSeconds(1);
		//reveal the countdown object
		centerScreen.SetActive(true);
		//Change text to 3
		centerTextBox.text = "3";
		StartCoroutine(Countdown2());
	}
	public IEnumerator Countdown2()
	{
		yield return new WaitForSeconds(1);
		//Change text to 2
		centerTextBox.text = "2";
		StartCoroutine(Countdown1());
	}
	public IEnumerator Countdown1()
	{
		yield return new WaitForSeconds(1);
		//Change text to 1
		centerTextBox.text = "1";
		StartCoroutine(CountdownGo());
	}
	public IEnumerator CountdownGo()
	{
		yield return new WaitForSeconds(1);
		//Change text to GO
		centerTextBox.text = "FIRE!!!";
		StartCoroutine(Begin());
	}
	public IEnumerator Begin()
	{
		yield return new WaitForSeconds(1);
		centerScreen.SetActive(false);
		//Send out an event, have all the game objects only move/allow movement after recieving this signal
		EventScript.BeginGame.Invoke();
	}

	public void PauseCheck()
	{
		Debug.Log("PauseCheck Overseer");
		if (!isPaused)
		{   //if not paused, pause
			Pause();
			isPaused = true;
			Debug.Log("Paused");
		}
		else if (isPaused)
		{   //if paused, unpause
			Resume();
			isPaused = false;
			Debug.Log("UnPaused");
		}
	}
	public void Pause()
	{
		Time.timeScale = 0.0f;
		pauseScreen.SetActive(true);
	}
	public void Resume()
	{
		Time.timeScale = 1.0f;
		pauseScreen.SetActive(false);
	}
	public void Restart()   //calls from outside forces
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void ScoreFor(string PlayerColor)
	{   //This is the color of the player THAT SCORED
		if (PlayerColor != null)
		{
			if (PlayerColor == "Light")
			{
				lightScore++;
				lightScoreText.text = lightScore.ToString();
				if (lightScore >= Universals.requiredScore)
				{   //if you won the game
					EventScript.GameWon.Invoke();
					centerScreen.SetActive(true);
					centerTextBox.text = "White Won!!!";
					Invoke("EndGame", 2);
				}
				else
				{
					EventScript.PointScored.Invoke();
					Invoke("ResetGame", 2);
				}
			}
			else if (PlayerColor == "Dark")
			{
				darkScore++;
				darkScoreText.text = darkScore.ToString();
				if (darkScore >= Universals.requiredScore)
				{   //if you won the game
					EventScript.GameWon.Invoke();
					centerScreen.SetActive(true);
					centerTextBox.text = "Black Won!!!";
					Invoke("EndGame", 2);
				}
				else
				{
					EventScript.PointScored.Invoke();
					Invoke("ResetGame", 2);
				}
			}
			Debug.Log(PlayerColor.ToString());
		}
	}

	public void SelfGoalDissapear(BallBehav ball, BallBehav.Team team)
	{   //vanish the ball for a few seconds, unless there are multiple balls active, then just unspawn that ball until there is 1 left

		if (team == BallBehav.Team.Dark)
		{
			if (ActiveBallCountDark()>1)	//if there are multiple balls, dont respawn the one that just left
			{
				ball.gameObject.SetActive(false);
			}
			else
			{
				ball.gameObject.SetActive(false);
				//Only let it reappear if there are no other balls of the same team in play
				StartCoroutine(SelfGoalReappear(ball));
			}
		}
		else if (team == BallBehav.Team.Light)
		{
			if (ActiveBallCountLight() > 1)  //if there are multiple balls, dont respawn the one that just left
			{
				ball.gameObject.SetActive(false);
			}
			else
			{
				ball.gameObject.SetActive(false);
				//Only let it reappear if there are no other balls of the same team in play
				StartCoroutine(SelfGoalReappear(ball));
			}
		}
	}

	int ActiveBallCountLight()
	{
		int count = 0;
		foreach (var obj in lightBalls)
		{
			if (obj.gameObject.activeSelf)
			{
				count++;
			}
		}
		return count;
	}

	int ActiveBallCountDark()
	{
		int count = 0;
		foreach (var obj in darkBalls)
		{
			if (obj.gameObject.activeSelf)
			{
				count++;
			}
		}
		return count;
	}

	public void SpawnMultiball(bool isDark, Vector3 pos)
	{
		Debug.Log($"Multiball Spawn. isDark: {isDark}, pos: {pos}");

		if (isDark)
		{
			for (int i = 0; i < darkBalls.Count - 1; i++)
			{
				if (!darkBalls[i].isActiveAndEnabled)
				{
					Debug.Log($"darkball{i} is being enabled");
					darkBalls[i].gameObject.SetActive(true);
					//darkBalls[i].startingPos = pos;
					darkBalls[i].GameBeginMultiball();
					break;
				}
			}
		}
		else
		{
			for (int i = 0; i < lightBalls.Count - 1; i++)
			{
				if (!lightBalls[i].isActiveAndEnabled)
				{
					Debug.Log($"lightball {i} is being enabled");
					lightBalls[i].gameObject.SetActive(true);
					//lightBalls[i].startingPos = pos;
					lightBalls[i].GameBeginMultiball();
					break;
				}
			}
		}
	}

	public IEnumerator SelfGoalReappear(BallBehav ball)
	{
		//Debug.Log("Reappear Called");
		yield return new WaitForSeconds(2);
		//Debug.Log("Reappear Enacted");
		ball.gameObject.SetActive(true);
		ball.Respawn();
	}

	public void ResetGame()		//Called by PointScored method above affter 2 seconds
	{
		EventScript.NewRound.Invoke();
	}

	public void EndGame()
	{
		Debug.Log("EndGame");
		endGameScreen.SetActive(true);

		pauseEventSystem.SetActive(!true);
		gameendEventSystem.SetActive(true); //this is just here because we are using multiple canvases
	}

	public void ExitToCharSelect()
	{
		SceneManager.LoadScene("CharacterSelect");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
