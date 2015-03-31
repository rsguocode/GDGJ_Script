﻿﻿using UnityEngine;
using System.Collections;

public class ModelControl : MonoBehaviour {

    Animation animation;
    public Transform target;
    public float speed = 1f;

    Transform mTrans;
    bool isDrag;

    private Animator animator;
    private int state;

	// Use this for initialization
	void Start () {
        //animation = GetComponent<Animation>();
        //animation.playAutomatically = false;
        //animation["stand"].wrapMode = WrapMode.Loop;
        //animation.Play("stand");
        mTrans = transform;
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {   
        {
            if (Input.GetMouseButtonDown(0))
            {
                state = Random.Range(0, 2);
                animator.SetInteger("State", state);
            }
        }
	}

    void OnClick()
    {
        //animation.Play("attack2");
        state++;
        if (state >= 3) {
            state = 0;
        }
        animator.SetInteger("State", state);
    }

    void OnDrag(Vector2 delta)
    {
       
        if (animation.IsPlaying("stand"))
        {
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
            if (target != null)
            {
                target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * target.localRotation;
            }
            else
            {
                mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * speed, 0f) * mTrans.localRotation;
            }
        }
     
        
    }
}
