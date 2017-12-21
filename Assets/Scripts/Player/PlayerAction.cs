using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	private bool jumpOn = false;
	private float jumpPower = 1f;
	private float tempJump;
	private Transform playerTf;
	private Vector3 tempVec;

	private void Awake()
	{
		playerTf = transform;
	}

	private void Update()
	{
		if (!jumpOn)
		{
			// 마우스 클릭하면 점프
			if (Input.GetMouseButton(0) || Input.GetButtonDown("Jump"))
			{
				StartCoroutine("JumpAction");
			}
		}
	}

	IEnumerator JumpAction()
	{
		// 누르는 시간에 따라 혹은 드래그에 따라 점프 파워 달리함
		jumpOn = true;
		tempVec = playerTf.position;

		tempJump = jumpPower;
		tempVec.y += tempJump;
		playerTf.position = tempVec;

		while (tempVec.y > 0)
		{
			yield return new WaitForSeconds(0.03f);
			tempJump -= 0.05f;
			tempVec.y -= tempJump;
			playerTf.position = tempVec;
		}

		tempVec.y = 0;
		playerTf.position = tempVec;
		jumpOn = false;
	}
}
