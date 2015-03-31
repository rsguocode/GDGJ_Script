using UnityEngine;
using System.Collections;
using PCustomDataType;

public class DangJiTester : MonoBehaviour {

	public static uint copyId = 10001;
	public static byte job = 1;
	public static uint playerVoCurHp = 100;

	public static PMapMon SetMonsterInfo()
	{
		PMapMon mon = new PMapMon ();
		mon.monid = 610001;
		mon.id = 610001;
		mon.lvl = 1;
		mon.dir = 1;
		mon.hp = 100;
		mon.hpFull = 100;
		mon.speed = 1;
		mon.born = 1;
		mon.state = 1;

		return mon;
	}

}
