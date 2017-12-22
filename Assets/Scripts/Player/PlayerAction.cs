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

	private void Awake()
	{
		playerTf = transform;

		playerAni = gameObject.GetComponent<Animator>();
	}

	private void Update()
	{
		if (!jumpOn)
		{
			// 마우스 클릭하면 점프
			if (Input.GetMouseButton(0))	// 버튼 누름
			{
				// UI가 위가 아닐때
				if (EventSystem.current.IsPointerOverGameObject() == false)
				{
					StartCoroutine("CheckButtonDownSec");	
				}
			} 
			else if (Input.GetMouseButtonUp(0))	// 버튼에서 뗌
			{
				
				StopCoroutine("CheckButtonDownSec");
				
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

		if (checkTime > 0.15f)
		{
			if (checkTime < 0.25f)
			{
				// 낮은 점프
				playerAni.SetTrigger("jump1");	// 트리거 사용 -> 애니메이션 시작
			}
			else
			{
				// 높은 점프
				playerAni.SetTrigger("jump2");	// 트리거 사용 -> 애니메이션 시작		
			}
			
			yield return new WaitForSeconds(0.15f);
		}
		
		
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

		
		if (checkTime < 0.25f)
		{
			playerAni.SetTrigger("balldrop1"); // 트리거 사용 -> 애니메이션 시작
		}
		else
		{
			playerAni.SetTrigger("balldrop2");	// 트리거 사용 -> 애니메이션 시작
		}
		
		jumpOn = false;
	}
}
