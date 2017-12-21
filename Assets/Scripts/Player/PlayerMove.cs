using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

	public float speed = 0.5f;
	private Transform playerTf;
	private Vector3 playerPos;

	private void Awake()
	{
		playerTf = transform;
	}

	private void Start()
	{
		playerPos = playerTf.position;
	}

	private void FixedUpdate()
	{
		playerPos.x += speed;
		playerTf.position = playerPos;
	}
}
