using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIevent : MonoBehaviour
{
	private bool pauseOn = false;
	private GameObject normalPanel;
	private GameObject pausePanel;

	private void Awake()
	{
		normalPanel = GameObject.Find("Canvas").transform.FindChild("NormalUI").gameObject;
		pausePanel = GameObject.Find("Canvas").transform.FindChild("PauseUI").gameObject;
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
}
