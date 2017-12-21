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
		Debug.Log("충돌: " + other.name);
	}
}
