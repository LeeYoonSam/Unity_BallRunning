using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAction : MonoBehaviour
{
	private bool jumpOn = false;
	private float jumpPower = 3.5f;
	private float tempJump;
	private Transform playerTf;
	private Vector3 tempVec;

	private Animator playerAni;

	private bool freezeState = false;

	private void Awake()
	{
		playerTf = transform;

		playerAni = gameObject.GetComponent<Animator>();
	}

	private void Update()
	{
		if (!freezeState)
		{
			// 마우스 클릭하면 점프
			if (Input.GetMouseButton(0))	// 버튼 누름
			{
				// UI가 위가 아닐때
				if (EventSystem.current.IsPointerOverGameObject() == false)
				{
					if (checkTime > 0)
					{
						if (jumpOn)
						{
							StopCoroutine("CheckButtonDownSec");
						}
					}
						
					StartCoroutine("CheckButtonDownSec");	
				}
			}
			
			if (!jumpOn)
			{
				if (Input.GetMouseButtonUp(0))	// 버튼에서 뗌
				{
				
//					StopCoroutine("CheckButtonDownSec");
				
					// UI가 위가 아닐때
					if (EventSystem.current.IsPointerOverGameObject() == false)
					{
						// 점프 수치가 있어야 실행
						if (checkTime > 0)
						{
							StartCoroutine("JumpAction");
						}
					}
					else
					{
						if (checkTime > 0)
						{
							StopCoroutine("CheckButtonDownSec");
							checkTime = 0;
						}
					}
				}	
			}
		}
		
	}

	private float checkTime;
	private IEnumerator CheckButtonDownSec()
	{
		// 버튼 누르고있는 시간 측정을 위한 코루틴
		checkTime = 0.0f;
		yield return new WaitForSeconds(0.08f);

		while (checkTime < 0.4f)
		{
			yield return new WaitForSeconds(0.03f);	// 0.04초에 한번씩 시간측정
			checkTime += 0.03f;			
		}
	}

	private float tempJumpTime;
	private IEnumerator JumpAction()
	{
		// 누르는 시간에 따라 혹은 드래그에 따라 점프 파워 달리함
		jumpOn = true;
		tempVec = playerTf.position;
		
		tempJumpTime = checkTime;
		checkTime = 0;
		StopCoroutine("CheckButtonDownSec");

		// 차지 가능한 최대시간
		if (tempJumpTime > 0.38f)
		{
			tempJumpTime = 0.38f;
		}

		if (tempJumpTime > 0.15f)
		{
			if (tempJumpTime < 0.25f)
			{
				// 낮은 점프
				playerAni.SetTrigger("jump1"); // 트리거 사용 -> 애니메이션 시작
			}
			else
			{
				// 높은 점프
				playerAni.SetTrigger("jump2");	// 트리거 사용 -> 애니메이션 시작
			}
			
			yield return new WaitForSeconds(0.15f);
		}
		
		
		tempJump = jumpPower * tempJumpTime;
		tempVec.y += tempJump;
		playerTf.position = tempVec;

		float tempFloat = 0.09f;
		
		// 상승
		while (tempJump > 0)
		{
			yield return new WaitForSeconds(0.03f);
			if (tempFloat > 0.02f) ;
			{
				tempFloat -= 0.002f;
			}
			tempJump -= tempFloat;
			tempVec.y += tempJump * 1.32f;
			playerTf.position = tempVec;
		}
		
		// 하락
		while (tempVec.y > 0)
		{
			yield return new WaitForSeconds(0.03f);

			if (tempFloat < 0.08f)
			{
				tempFloat += 0.002f;
			}
			tempJump -= tempFloat;
			tempVec.y += tempJump;
			playerTf.position = tempVec;
			
		}

		tempVec.y = 0;
		playerTf.position = tempVec;

		
		if (tempJumpTime < 0.25f)
		{
			playerAni.SetTrigger("balldrop1"); // 트리거 사용 -> 애니메이션 시작
		}
		else
		{
			playerAni.SetTrigger("balldrop2");	// 트리거 사용 -> 애니메이션 시작
		}
		
		jumpOn = false;
	}

	public void FreezeBall()
	{
		// ball의 움직임을 멈춤
		if (jumpOn)
		{
			StopCoroutine("JumpAction");
			jumpOn = false;
		}
		else
		{
			StopAllCoroutines();
		}

		freezeState = true;
	}
}
