using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIevent : MonoBehaviour
{
	private bool pauseOn = false;
	private GameObject normalPanel;
	private GameObject pausePanel;
	private GameObject overPanel;
	
	private void Awake()
	{
		normalPanel = GameObject.Find("Canvas").transform.FindChild("NormalUI").gameObject;
		pausePanel = GameObject.Find("Canvas").transform.FindChild("PauseUI").gameObject;
		overPanel = GameObject.Find("Canvas").transform.FindChild("OverUI").gameObject;
	}

	public void ActivePauseBt()
	{
		if (!pauseOn)
		{
			Time.timeScale = 0; // 시간 흐름 비율 0으로 변경
			pausePanel.SetActive(true);
			normalPanel.SetActive(false);
		}
		else
		{
			Time.timeScale = 1.0f;
			pausePanel.SetActive(false);
			normalPanel.SetActive(true);
		}

		pauseOn = !pauseOn;
	}

	public void RetryBt()
	{
		Debug.Log("게임 재시작");
		Time.timeScale = 1.0f;
		
		SceneManager.LoadScene("GameScene");
	}

	public void QuitBt()
	{
		Debug.Log("게임 종료");
		Application.Quit();
	}

	public void SetGameOverUI()
	{
		normalPanel.SetActive(false);
		overPanel.SetActive(true);

		StartCoroutine("GameOverScoreCount");
	}

	private IEnumerator GameOverScoreCount()
	{
		Text nowScoreTx = overPanel.transform.FindChild("NowScoreTx").GetComponent<Text>();
		Text bestScoreTx = overPanel.transform.FindChild("BestScoreTx").GetComponent<Text>();

		int resultScore = gameObject.GetComponent<ScoreManager>().scorePoint;
		int tempScore = 0;
		float delaySec = 0.15f;

		bestScoreTx.text = "(BEST : " + resultScore.ToString("N0") + ")";

		while (tempScore < resultScore)
		{
			yield return new WaitForSeconds(delaySec);
			tempScore += 100;
			nowScoreTx.text = tempScore.ToString("N0");

			if (delaySec > 0.02f)
			{
				delaySec = 0.005f;
			}
		}

		nowScoreTx.text = resultScore.ToString("N0");
	} 
}
