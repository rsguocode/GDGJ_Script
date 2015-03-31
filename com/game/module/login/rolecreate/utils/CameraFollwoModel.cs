﻿﻿﻿using UnityEngine;
using System.Collections;

public class CameraFollwoModel : MonoBehaviour {
    public Transform target;
    Vector3 position;
    Vector3 delta;
	// Use this for initialization
	void Start () {

        position = transform.localPosition; //保存初始位置
        if(target)
            delta = target.position - transform.position;
	}
    public void setTarget(Transform target)
    {
        this.target = target;
        transform.localPosition = new Vector3(0f, 0f, -3.593668f);//恢复初始位置，重新计算
        transform.localEulerAngles = new Vector3(20.15945f,0f,0f);
        delta = this.target.position - transform.position;
        transform.position = target.position - delta;
        Debug.Log(this.target.localPosition + " " + delta + " " + target.localPosition+target.parent.name);
    }
	
	// Update is called once per frame
	void Update () {
        if (target)
            transform.position = target.position - delta;
        //Debug.Log(target.localPosition.y);
	
	}
}
