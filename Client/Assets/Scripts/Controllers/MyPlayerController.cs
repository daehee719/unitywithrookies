using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class MyPlayerController : PlayerController
{
	bool _moveKeyPressed = false;
	float originSpeed;

	Button allkillBtn;
	protected override void Init()
	{
		allkillBtn = GameObject.Find("Canvas").transform.Find("Image").GetComponent<Button>();
		allkillBtn.onClick.AddListener(AllKill);
        originSpeed = Speed;

        base.Init();
	}

	protected override void UpdateController()
	{
		switch (State)
		{
			case CreatureState.Idle:
				GetDirInput();
				break;
			case CreatureState.Moving:
				GetDirInput();
				break;
		}

		base.UpdateController();
	}
	void AllKill()
	{
		Debug.Log("AllDIE!!!!");
		C_Alldie alldie = new C_Alldie() { ObjectId = Id };
        Managers.Network.Send(alldie);
    }

	protected override void UpdateIdle()
	{
		// 이동 상태로 갈지 확인
		if (_moveKeyPressed)
		{
			State = CreatureState.Moving;
			return;
		}

		if (_coSkillCooltime == null && Input.GetKey(KeyCode.Space))
		{
			Debug.Log("Skill !");

			C_Skill skill = new C_Skill() { Info = new SkillInfo() };
			skill.Info.SkillId = 2;
			Managers.Network.Send(skill);

			_coSkillCooltime = StartCoroutine("CoInputCooltime", 0.2f);
		}

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Skill !");
            Speed = Speed*2f;
			isChangeSpeed = true;
            C_Speed skill = new C_Speed() { Speed = Speed * 2f };
            Managers.Network.Send(skill);

            _coSkillCooltime = StartCoroutine("CoSpeedUp", 5f);
        }
    }

	Coroutine _coSkillCooltime;
	IEnumerator CoInputCooltime(float time)
	{
		yield return new WaitForSeconds(time);
		_coSkillCooltime = null;
	}

    IEnumerator CoSpeedUp(float time)
    {
        yield return new WaitForSeconds(time);
        C_Speed skill = new C_Speed() { Speed = originSpeed };
        Managers.Network.Send(skill);
		isChangeSpeed = false;
    }

    void LateUpdate()
	{
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
		Debug.Log(Stat.Speed);
	}

	// 키보드 입력
	void GetDirInput()
	{
		_moveKeyPressed = true;

		if (Input.GetKey(KeyCode.W))
		{
			Dir = MoveDir.Up;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			Dir = MoveDir.Down;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			Dir = MoveDir.Left;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			Dir = MoveDir.Right;
		}
		else
		{
			_moveKeyPressed = false;
		}
	}

	protected override void MoveToNextPos()
	{
		if (_moveKeyPressed == false)
		{
			State = CreatureState.Idle;
			CheckUpdatedFlag();
			return;
		}

		Vector3Int destPos = CellPos;

		switch (Dir)
		{
			case MoveDir.Up:
				destPos += Vector3Int.up;
				break;
			case MoveDir.Down:
				destPos += Vector3Int.down;
				break;
			case MoveDir.Left:
				destPos += Vector3Int.left;
				break;
			case MoveDir.Right:
				destPos += Vector3Int.right;
				break;
		}

		if (Managers.Map.CanGo(destPos))
		{
			if (Managers.Object.FindCreature(destPos) == null)
			{
				CellPos = destPos;
			}
		}

		CheckUpdatedFlag();
	}

	protected override void CheckUpdatedFlag()
	{
		if (_updated)
		{
			C_Move movePacket = new C_Move();
			movePacket.PosInfo = PosInfo;
			Managers.Network.Send(movePacket);
			_updated = false;
		}
	}
}
