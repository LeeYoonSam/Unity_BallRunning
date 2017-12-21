using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	private bool jumpOn = false;
	private float jumpPower = 3.5f;
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
			if (Input.GetMouseButton(0))	// 버튼 누름
			{
				StartCoroutine("CheckButtonDownSec");
			} 
			else if (Input.GetMouseButtonUp(0))	// 버튼에서 뗌
			{
				StartCoroutine("JumpAction");	
			}
		}
	}

	private float checkTime;
	private IEnumerator CheckButtonDownSec()
	{
		// 버튼 누르고있는 시간 측정을 위한 코루틴
		checkTime = 0;

		while (!jumpOn)
		{
			yield return new WaitForSeconds(0.04f);	// 0.04초에 한번씩 시간측정
			checkTime += 0.04f;
		}
	}

	private IEnumerator JumpAction()
	{
		// 누르는 시간에 따라 혹은 드래그에 따라 점프 파워 달리함
		jumpOn = true;
		tempVec = playerTf.position;

		if (checkTime > 0.35f) // 차지 가능한 최대 시간
		{
			checkTime = 0.35f;
		}

		Debug.Log("checkTime: " + checkTime);
		tempJump = jumpPower * checkTime;
		tempVec.y += tempJump;
		playerTf.position = tempVec;

		while (tempVec.y > 0)
		{
			yield return new WaitForSeconds(0.03f);
			tempJump -= 0.05f;
			tempVec.y += tempJump;
			playerTf.position = tempVec;
		}

		tempVec.y = 0;
		playerTf.position = tempVec;
		jumpOn = false;
	}
}
