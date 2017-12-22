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
		public ObstacleInfo info;	// 장애물 정보 스크립트
	}

	private ObstacleStruct[] obss;
	// === 장애물 추가 === 
	
	
	
	// === 보너스 오브젝트 추가 ===
	public GameObject plusScore;

	private int plusScoreNum = 5;

	struct PlusScoreStruct
	{
		public GameObject obj;
		public bool active;
		public int parentTileNum;
	}

	private PlusScoreStruct[] plusScoreSets;
	// === 보너스 오브젝트 추가 ===
	
	private void Awake()
	{
		tileCenterVec = new Vector3(0, -5.1f, 0);
		CreateTiles();
		CreateObss();	// 장애물 추가
		CreatePSs();	// plusScoreObj등 생성
	}
	
	// 반복해서 사용할 타일 생성
	private void CreateTiles()
	{
		tempVec = tileCenterVec;
		
		tiles = new TileStruct[tileNum];
		for (var i = 0; i < tileNum; i++)
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

		for (var i = 0; i < obsNum; i++)
		{
			obss[i].obj = Instantiate(obstacle, Vector3.zero, Quaternion.identity) as GameObject;
			obss[i].active = false;
			obss[i].parentTileNum = -1;
			obss[i].info = obss[i].obj.GetComponent<ObstacleInfo>();	// 스크립트 컴포넌트 가져옴
			obss[i].obj.SetActive(false);	// 초기상태는 비활성화
		}
	}

	private void CreatePSs()
	{
		plusScoreSets = new PlusScoreStruct[plusScoreNum];
		for (var i = 0; i < plusScoreNum; i++)
		{
			plusScoreSets[i].obj = Instantiate(plusScore, Vector3.zero, Quaternion.identity) as GameObject;
			plusScoreSets[i].active = false;
			plusScoreSets[i].parentTileNum = -1;
			plusScoreSets[i].obj.SetActive(false);
		}
	}

	public void FreezeMap()
	{
		speed = 0;
	}

	private int tempLevel = 1;
	
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
				DeletePSs(i);	// 넘어간 블록에 있던 장애물들 제거처리
				
				tiles[i].pos = tiles[lastTileNum].pos;
				tiles[i].pos.x += tileGap;

				if (lastTileNum > i)
				{
					tiles[i].pos.x -= 0.5f;
				}

				tiles[i].tf.position = tiles[i].pos; // 실제 위치 변경

				tempLevel = Random.Range(1, 9);
				
				AddedObs(i, tempLevel);
				
				AddPSs(i, tempLevel);
				
				lastTileNum = i;	// 다음 변경을 위해 마지막 블록의 번호 변경
			}
			
		}
	}

	private void AddedObs(int tileN, int level) // 타일번호, 장애물 수
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
				obss[i].info.SetObstacle(level);
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

	private void AddPSs(int tileN, int level)
	{
		// 해당하는 타일에 보너스 점수 추가
		tempVec.x = tiles[tileN].pos.x;
		tempVec.y = 2.75f + 1.25f * level;
		tempVec.z = 0;

		for (var i = 0; i < plusScoreNum; i++)
		{
			if (!plusScoreSets[i].active)
			{
				plusScoreSets[i].obj.SetActive(true);
				plusScoreSets[i].active = true;
				plusScoreSets[i].obj.transform.position = tempVec;
				plusScoreSets[i].obj.transform.SetParent(tiles[tileN].tf);	// 부모 변경
				plusScoreSets[i].parentTileNum = tileN;
				break;
			}
		}
	}

	private void DeletePSs(int tileN)
	{
		for (var i = 0; i < plusScoreNum; i++)
		{
			if (plusScoreSets[i].active)
			{
				if (plusScoreSets[i].parentTileNum == tileN)
				{
					plusScoreSets[i].obj.transform.parent = null;
					plusScoreSets[i].parentTileNum = -1;

					// 보너스의 경우 대부분 먹는다고 가정해둬야 함.
					if (plusScoreSets[i].obj.activeSelf)
					{
						plusScoreSets[i].obj.SetActive(false);
					}

					plusScoreSets[i].active = false;
				}
			}
		}
	}
}
