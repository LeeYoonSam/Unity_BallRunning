using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInfo : MonoBehaviour
{
	public int highLevel = 0;
	private float height;

	private Transform obsTf;

	private void Awake()
	{
		obsTf = transform;
	}

	public void SetObstacle(int lv)
	{
		height = 0.5f + 0.5f * lv; // 기본 0.5 + 레벨당 0.5씩 증가
		Vector3 tempVec = new Vector3(1f, height, 1f);
		obsTf.localScale = tempVec;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("플레이어와 충돌");
			GameObject.FindWithTag("Player").GetComponent<PlayerAction>().FreezeBall();
			GameObject.FindWithTag("GameManager").GetComponent<MoveMap>().FreezeMap();
			GameObject.FindWithTag("GameManager").GetComponent<ScoreManager>().EndCountScore();
			GameObject.FindWithTag("GameManager").GetComponent<UIevent>().SetGameOverUI();
		}
	}
}
