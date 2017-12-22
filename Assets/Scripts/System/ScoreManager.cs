using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

	public int scorePoint = 0;
	private Text scoreTx;

	private int bestScorePoint; // 최고점 임시 저장

	private Color effColor;
	private Color normalColor;

	private void Awake()
	{
		bestScorePoint = PlayerPrefs.GetInt("BestScore"); // 시작 시 베스트 스코어 얻어와서 대기
			
		scoreTx = GameObject.Find("NormalUI").transform.FindChild("scoreTx").GetComponent<Text>();
		scoreTx.text = "SCORE : 0";

		normalColor = scoreTx.color;
		effColor = new Color(0, 0.545f, 1f);
		
		StartCoroutine("PlusScoreRoutine");
	}

	public void PlusScore(int plusPoint)
	{
		scorePoint += plusPoint;

		if (plusIng)
		{
			StopCoroutine("PlusValue");
		}
		
		StartCoroutine("PlusValue");
//		scoreTx.text = "SCORE : " + scorePoint.ToString("N0");
	}

	private bool plusIng = false;
	private float tempPer;
	private int pastScorePoint = 0;

	private IEnumerator PlusValue()
	{
		plusIng = true;
		tempPer = 0;
		
		while (tempPer < 1f)
		{
			tempPer += 0.1f;

			if (pastScorePoint < scorePoint)
			{
				// 카운트 형식으로 상승하기 위하여 Lerp 사용
				pastScorePoint = (int) Mathf.Lerp(pastScorePoint, scorePoint, tempPer);
				
				scoreTx.text = "SCORE : " + pastScorePoint.ToString("N0");
			}
			
			yield return new WaitForSeconds(0.033f); // 1초 간격으로 스코어 점수 체크
		}

		pastScorePoint = scorePoint;
		scoreTx.text = "SCORE : " + pastScorePoint.ToString("N0");

		plusIng = false;
	}
	
	private IEnumerator PlusScoreRoutine()
	{
		while (true)
		{			
			yield return new WaitForSeconds(1f); // 1초 간격으로 스코어 점수 체크
			PlusScore(100);
		}
	}

	public void EndCountScore()
	{
		StopAllCoroutines();
		if (plusIng)
		{
			plusIng = false;
			scoreTx.text = "SCORE : " + scorePoint.ToString("N0");
		}

		// 신기록
		if (scorePoint > bestScorePoint)
		{
			bestScorePoint = scorePoint;
			PlayerPrefs.SetInt("BestScore", bestScorePoint);
		}
	}

	public void ScoreTextColorEffect()
	{
		StartCoroutine("ScoreTextEffRoutine");
	}

	private IEnumerator ScoreTextEffRoutine()
	{
		scoreTx.color = effColor;
		yield return new WaitForSeconds(0.5f);
		scoreTx.color = normalColor;
	}
}
