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
	private float tileEndPoint = -60f;

	private Vector3 tempVec;

	private float speed = 0.5f;
	private int lastTileNum = 0;
	
	private void Awake()
	{
		tileCenterVec = new Vector3(0, -5.1f, 0);
		CreateTiles();
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
				tiles[i].pos = tiles[lastTileNum].pos;
				tiles[i].pos.x += tileGap;

				if (lastTileNum > i)
				{
					tiles[i].pos.x -= 0.5f;
				}

				tiles[i].tf.position = tiles[i].pos; // 실제 위치 변경
				lastTileNum = i;
			}
			
		}
	}
}
