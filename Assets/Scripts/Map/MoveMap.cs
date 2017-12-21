using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
	public GameObject tile;
//	private GameObject[] tiles;
	private int tileNum = 3;

	struct TileStruct
	{
		public GameObject obj;
		public Transform tf;
		public bool active;
		public Vector3 pos;
	}

	private TileStruct[] tiles;

	private Vector3 tileCenterVec; 	// 기준점
	private float tileGap = 81.9f;	// 블록 길이 차이
	private float tileEndPoint = -70f;

	private Vector3 tempVec;

	private float speed = 0.5f;
	private int lastTileNum = 0;
	
	
	// === 장애물 추가 === 
	public GameObject obstacle;

	private int obsNum = 10;

	struct ObstacleStruct
	{
		public GameObject obj;
		public bool active;
		public int parentTileNum;
	}

	private ObstacleStruct[] obss;
	// === 장애물 추가 === 
	
	
	private void Awake()
	{
		tileCenterVec = new Vector3(0, -5.1f, 0);
		CreateTiles();
		CreateObss();
	}
	
	// 반복해서 사용할 타일 생성
	private void CreateTiles()
	{
		tempVec = tileCenterVec;
		
		tiles = new TileStruct[tileNum];
		for (int i = 0; i < tileNum; i++)
		{
			tiles[i].obj = Instantiate(tile, tempVec, Quaternion.identity) as GameObject; // 오브젝트 생성
			tiles[i].tf = tiles[i].obj.transform;
			tiles[i].pos = tiles[i].tf.position;
			tiles[i].active = true;

			tempVec.x += tileGap;
		}

		lastTileNum = 2;
	}

	// 장애물 생성
	private void CreateObss()
	{
		// 반복 사용할 장애물들을 생성
		
		obss = new ObstacleStruct[obsNum];

		for (int i = 0; i < obsNum; i++)
		{
			obss[i].obj = Instantiate(obstacle, Vector3.zero, Quaternion.identity) as GameObject;
			obss[i].active = false;
			obss[i].parentTileNum = -1;
			obss[i].obj.SetActive(false);	// 초기상태는 비활성화
		}
	}

	private void FixedUpdate()
	{
		// 블록이 계속하여 일정한 속도로 이동하도록 생성
		for (int i = 0; i < tileNum; i++)
		{
			tiles[i].pos.x -= speed;
			if (tiles[i].pos.x > tileEndPoint)
			{
				// 화면의 보이지 않는 지점으로 정해준 곳보다 더 가지 않았으면
				tiles[i].tf.position = tiles[i].pos;
			}
			else
			{
				// endPoint 초과 -> 가장 마지막 블록으로 위치시킴

				DeleteObs(i);	// 넘어간 블록에 있던 장애물들 제거처리
				tiles[i].pos = tiles[lastTileNum].pos;
				tiles[i].pos.x += tileGap;

				if (lastTileNum > i)
				{
					tiles[i].pos.x -= 0.5f;
				}

				tiles[i].tf.position = tiles[i].pos; // 실제 위치 변경
				AddedObs(i, 1);
				
				lastTileNum = i;	// 다음 변경을 위해 마지막 블록의 번호 변경
			}
			
		}
	}

	private void AddedObs(int tileN, int obsN) // 타일번호, 장애물 수
	{
		// 해당하는 타일에 장애물 추가해줌
		tempVec.x = tiles[tileN].pos.x;	// 블록 중앙 테스트
		tempVec.y = 0;
		tempVec.z = 0;
		
		for(int i = 0; i < obsNum; i ++)
		{
			if (!obss[i].active)	// 장애물 활성화
			{
				obss[i].obj.SetActive(true); // 장애물 활성화
				obss[i].active = true;
				obss[i].obj.transform.position = tempVec;
				obss[i].obj.transform.SetParent(tiles[tileN].tf);	// 부모 변경
				obss[i].parentTileNum = tileN;
				break;	// 하나라도 생성되면 종료
			}
		}
	}

	private void DeleteObs(int tileN)
	{
		for (int i = 0; i < obsNum; i++)
		{
			if (obss[i].active)
			{
				if (obss[i].parentTileNum == tileN)
				{
					obss[i].obj.transform.parent = null;	// 부모없앰 -> 다시 obss 오브젝트 하위로
					obss[i].parentTileNum = -1;	// 값 없음
					obss[i].obj.SetActive(false); // 비활성화
					obss[i].active = false;
				}
			}
		}
	}
}
