using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

	public int scorePoint = 0;
	private Text scoreTx;

	private void Awake()
	{
		scoreTx = GameObject.Find("NormalUI").transform.FindChild("scoreTx").GetComponent<Text>();
		scoreTx.text = "SCORE : 0";
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
}
