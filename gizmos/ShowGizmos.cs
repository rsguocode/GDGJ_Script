using UnityEngine;
using System.Collections;

public class ShowGizmos : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;     //在变换位置处绘制一个绿色圆
        Gizmos.DrawSphere(transform.position, 0.05f);
    }

}
