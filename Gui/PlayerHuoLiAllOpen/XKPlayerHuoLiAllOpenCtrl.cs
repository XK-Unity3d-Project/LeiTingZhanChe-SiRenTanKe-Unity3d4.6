﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XKPlayerHuoLiAllOpenCtrl : MonoBehaviour
{
	public GameObject PlayerHuoLiOpenPre;
	List<XKPlayerHuoLiAllOpenMove> HuoLiOpenList;
	const int MaxPlayerFS = 12;
	static  XKPlayerHuoLiAllOpenCtrl _Instance;
	public static XKPlayerHuoLiAllOpenCtrl GetInstance()
	{
		return _Instance;
	}
	// Use this for initialization
	void Start()
	{
		_Instance = this;
		HuoLiOpenList = new List<XKPlayerHuoLiAllOpenMove>();
		GameObject obj = null;
		for (int i = 0; i < MaxPlayerFS; i++) {
			obj = (GameObject)Instantiate(PlayerHuoLiOpenPre);
			obj.transform.parent = transform;
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.transform.localPosition = Vector3.zero;
			HuoLiOpenList.Add(obj.GetComponent<XKPlayerHuoLiAllOpenMove>());
			obj.SetActive(false);
		}
	}
	
	XKPlayerHuoLiAllOpenMove GetXKPlayerHuoLiOpenMove()
	{
		GameObject obj = null;
		int valTmp = 0;
		for (int i = 0; i < MaxPlayerFS; i++) {
			obj = HuoLiOpenList[i].gameObject;
			if (obj.activeSelf) {
				continue;
			}
			valTmp = i;
			break;
		}
		return HuoLiOpenList[valTmp];
	}
	
	public void ShowPlayerHuoLiOpen(PlayerEnum indexVal)
	{
		XKPlayerHuoLiAllOpenMove huoLiOpenMoveCom = GetXKPlayerHuoLiOpenMove();
		if (huoLiOpenMoveCom == null) {
			return;
		}
		
		Transform playerTr = XKPlayerMoveCtrl.GetXKPlayerMoveCtrl(indexVal).PiaoFenPoint;
		Vector3 startPos = XkGameCtrl.GetInstance().GetWorldObjToScreenPos(playerTr.position);
		huoLiOpenMoveCom.SetPlayerHuoLiOpenVal(startPos);
	}
}