﻿using UnityEngine;
using System.Collections;

public class PlayerXueTiaoCtrl : MonoBehaviour
{
	public PlayerEnum PlayerSt = PlayerEnum.Null;
	public Renderer NengLiangRenderer;
	Transform CameraTran;
	Transform NengLianTran;
	Transform NengLianParentTr;
	Vector3 OffsetXT;
	static PlayerXueTiaoCtrl _InstanceOne;
	public static PlayerXueTiaoCtrl GetInstanceOne()
	{
		return _InstanceOne;
	}
	
	static PlayerXueTiaoCtrl _InstanceTwo;
	public static PlayerXueTiaoCtrl GetInstanceTwo()
	{
		return _InstanceTwo;
	}
	
	static PlayerXueTiaoCtrl _InstanceThree;
	public static PlayerXueTiaoCtrl GetInstanceThree()
	{
		return _InstanceThree;
	}
	
	static PlayerXueTiaoCtrl _InstanceFour;
	public static PlayerXueTiaoCtrl GetInstanceFour()
	{
		return _InstanceFour;
	}

	public static PlayerXueTiaoCtrl GetInstance(PlayerEnum indexPlayer)
	{
		PlayerXueTiaoCtrl playerXT = null;
		switch (indexPlayer) {
		case PlayerEnum.PlayerOne:
			playerXT = _InstanceOne;
			break;
		case PlayerEnum.PlayerTwo:
			playerXT = _InstanceTwo;
			break;
		case PlayerEnum.PlayerThree:
			playerXT = _InstanceThree;
			break;
		case PlayerEnum.PlayerFour:
			playerXT = _InstanceFour;
			break;
		}
		return playerXT;
	}
	// Use this for initialization
	void Start()
	{
		CameraTran = Camera.main != null ? Camera.main.transform : null;
	}

	// Update is called once per frame
	void Update()
	{
		if (CameraTran == null) {
			CameraTran = Camera.main != null ? Camera.main.transform : null;
			return;
		}
		CheckPlayerHitCol();

		Vector3 pos = Vector3.zero;
		Vector3 forwardVal = Vector3.zero;
		switch (KeyHitSt) {
		case 0:
			forwardVal = CameraTran.forward;
			forwardVal.y = 0f;
			NengLianTran.forward = forwardVal;
			pos = NengLianParentTr.position;
			pos += forwardVal * OffsetXT.z;
			pos.x += OffsetXT.x;
			pos.y += OffsetXT.y;
			NengLianTran.position = pos;
			break;
		case 1:
			forwardVal = CameraTran.right;
			forwardVal.y = 0f;
			NengLianTran.forward = forwardVal;
			pos = NengLianParentTr.position;
			pos += forwardVal * (OffsetXT.z + 3f);
			pos.x += OffsetXT.x;
			pos.y += OffsetXT.y;
			NengLianTran.position = pos;
			break;
		case 2:
			forwardVal = -CameraTran.right;
			forwardVal.y = 0f;
			NengLianTran.forward = forwardVal;
			pos = NengLianParentTr.position;
			pos += forwardVal * (OffsetXT.z + 3f);
			pos.x += OffsetXT.x;
			pos.y += OffsetXT.y;
			NengLianTran.position = pos;
			break;
		case 3:
			forwardVal = CameraTran.forward;
			forwardVal.y = 0f;
			NengLianTran.forward = forwardVal;
			pos = NengLianParentTr.position;
			pos -= forwardVal * (OffsetXT.z - 3.5f);
			pos.x += OffsetXT.x;
			pos.y += OffsetXT.y;
			NengLianTran.position = pos;
			break;
		}
	}

	public void HandlePlayerXueTiaoInfo(float fillVal)
	{
		float xueLiangVal = 1f - fillVal;
		xueLiangVal = Mathf.Clamp01(xueLiangVal);
		NengLiangRenderer.materials[0].SetTextureOffset("_MainTex", new Vector2(xueLiangVal, 0f));
		bool isActiveXT = xueLiangVal >= 1f ? false : true;
		gameObject.SetActive(isActiveXT);
	}
	
	public void SetPlayerIndex(PlayerEnum playerIndex)
	{
		bool isActiveXT = XkGameCtrl.GetIsActivePlayer(playerIndex);
		PlayerSt = playerIndex;
		switch (PlayerSt) {
		case PlayerEnum.PlayerOne:
			_InstanceOne = this;
			break;
			
		case PlayerEnum.PlayerTwo:
			_InstanceTwo = this;
			break;
			
		case PlayerEnum.PlayerThree:
			_InstanceThree = this;
			break;
			
		case PlayerEnum.PlayerFour:
			_InstanceFour = this;
			break;
		}

		NengLianTran = transform;
		OffsetXT = NengLianTran.localPosition;
		NengLianParentTr = NengLianTran.parent;
		NengLianTran.parent = XkGameCtrl.MissionCleanup;
		gameObject.SetActive(isActiveXT);
	}
	
	/**
	 * KeyHitSt == 0 -> 血条在后.
	 * KeyHitSt == 1 -> 血条在左.
	 * KeyHitSt == 2 -> 血条在右.
	 * KeyHitSt == 3 -> 血条在前.
	 */
	byte KeyHitSt = 0;
	void CheckPlayerHitCol()
	{
		if (CameraTran == null || (Time.frameCount % 30) != 0) {
			return;
		}
		/**
		 * keyHit[0] -> 检测后面.
		 * keyHit[1] -> 检测左面.
		 * keyHit[2] -> 检测右面.
		 * keyHit[3] -> 检测前面.
		 * keyHit == 0 -> 没有检测到碰撞.
		 * keyHit == 1 -> 有检测到碰撞.
		 */
		byte[] keyHit = new byte[4];
		RaycastHit hitInfo;
		Vector3 startPos = NengLianParentTr.position + (Vector3.up * 3f);
		Vector3 forwardVal = -CameraTran.forward;
		float disVal = 5f;
		if (Physics.Raycast(startPos, forwardVal, out hitInfo, disVal, XkGameCtrl.GetInstance().XueTiaoCheckLayer)){
			keyHit[0] = 1;
		}

		forwardVal = -CameraTran.right;
		if (Physics.Raycast(startPos, forwardVal, out hitInfo, disVal, XkGameCtrl.GetInstance().XueTiaoCheckLayer)){
			keyHit[1] = 1;
		}
		
		forwardVal = CameraTran.right;
		if (Physics.Raycast(startPos, forwardVal, out hitInfo, disVal, XkGameCtrl.GetInstance().XueTiaoCheckLayer)){
			keyHit[2] = 1;
		}

		forwardVal = CameraTran.forward;
		if (Physics.Raycast(startPos, forwardVal, out hitInfo, disVal, XkGameCtrl.GetInstance().XueTiaoCheckLayer)){
			keyHit[3] = 1;
		}

		byte keyHitVal = (byte)((keyHit[0] << 3) + (keyHit[1] << 2) + (keyHit[2] << 1) + keyHit[3]);
		byte keyHitStVal = 0;
		switch (keyHitVal) {
		case 0x00:
			keyHitStVal = 0;
			break;
		case 0x08:
			keyHitStVal = 3;
			break;
		}

		if ((keyHitVal & 0x04) == 0x04) {
			keyHitStVal = 2;
		}
		
		if ((keyHitVal & 0x02) == 0x02) {
			keyHitStVal = 1;
		}

		if (KeyHitSt != keyHitStVal) {
			KeyHitSt = keyHitStVal;
//#if UNITY_EDITOR
//			switch (KeyHitSt) {
//			case 0:
//				Debug.Log("CheckPlayerHitCol -> KeyHitSt is back! player "+PlayerSt);
//				break;
//			case 1:
//				Debug.Log("CheckPlayerHitCol -> KeyHitSt is left! player "+PlayerSt);
//				break;
//			case 2:
//				Debug.Log("CheckPlayerHitCol -> KeyHitSt is right! player "+PlayerSt);
//				break;
//			case 3:
//				Debug.Log("CheckPlayerHitCol -> KeyHitSt is forward! player "+PlayerSt);
//				break;
//			}
//#endif
		}
	}
}