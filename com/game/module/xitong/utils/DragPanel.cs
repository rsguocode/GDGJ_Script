﻿﻿﻿using UnityEngine;
using System.Collections;


/*
 * 拖拽面板控制脚本  一个脚本搞定
 */
public class DragPanel : MonoBehaviour {
    private Vector3 last_position;
    private Plane plane;
    private bool is_pressed;
    //private BoxCollider box;
    private UIPanel panel;  //被拖拽的Panel
    public DragType dragType=DragType.Vertical;  //拖拽方向 
    public bool scrollByPage=false;    //是否整页滑动
    public int pages=1;    //页数
    private bool is_tranform;
    private float real_time;  //spring 时间控制
    private float start_time;
    private float actual_time;
    private float time_delta;
    
    private Vector3 target_position;   
    private Transform mTrans;
    private Vector3 localPosition;
    private Bounds bounds;
    private Vector3 interval;


    public enum DragType
    {
        Both,
        Vertical,
        Horizontal
    };
    void Awake()
    {
        mTrans = transform;
        localPosition = mTrans.localPosition;
        panel = GetComponent<UIPanel>();
        //bounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
        //Debug.Log("localPosition: " + panel.transform.localPosition);
        
    }
	// Use this for initialization
	void Start () {
        
        //NGUITools.AddWidgetCollider(gameObject);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (is_tranform)
        {

            UpdateRealTimeDelta();
            Vector3 pos = Vector3.Lerp(mTrans.localPosition, target_position, time_delta*4);
            //Debug.Log(time_delta + "......................");
            MoveRelative(pos - mTrans.localPosition);
            if (Vector3.Magnitude(target_position - mTrans.localPosition) <= Vector3.kEpsilon)
            {
                is_tranform = false;
            }
        }
	}
    void OnPress(bool pressed)
    {
        //Debug.Log("DragPanel Pressed: " + pressed);
        if (pressed)
        {
            last_position = UICamera.lastHit.point;
            // Create the plane to drag along
            plane = new Plane(mTrans.rotation * Vector3.back, last_position);
            //is_pressed = true;
            is_tranform = false;
        }
        else
        {
            is_tranform = true;
            Vector3 distance=Vector3.zero;
            if(!scrollByPage)    //
            {
                distance = panel.CalculateConstrainOffset(bounds.min,bounds.max);
                if (distance.magnitude > 0.001f)
                    target_position = mTrans.localPosition + distance;
            }
            else    //处理整页滑动
            {
                int currentPage=1;
               
                interval=(bounds.max-bounds.min)/pages;
                if (dragType == DragType.Vertical)
                {
                    currentPage=(int)Mathf.Ceil((mTrans.localPosition.y - localPosition.y) / interval.y);
                    currentPage = Mathf.Clamp(currentPage, 1, pages);
                    distance.x = localPosition.x;
                    distance.y = localPosition.y + (currentPage - 1) * interval.y;
                }
                else if (dragType == DragType.Horizontal)
                {
                    currentPage = (int)Mathf.Ceil((mTrans.localPosition.x - localPosition.x) / interval.x);   //上取整数
                    currentPage = Mathf.Clamp(currentPage, 1, pages);    //判断页数
                    distance.y = localPosition.y;
                    distance.x = localPosition.x + (currentPage - 1) * interval.x;
                    //distance = panel.CalculateConstrainOffset(new Vector3( bounds.min.x + interval.x * (currentPage - 1), bounds.min.y, bounds.min.z), new Vector3(bounds.min.x + interval.x * currentPage, bounds.max.y, bounds.min.z));
                }
                target_position = distance;
                
            }
            
            
        }
    }

    void OnDrag(Vector2 delta)
    {

        Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
        float dist = 0f;
        if (plane.Raycast(ray, out dist))
        {
            Vector3 currentPos = ray.GetPoint(dist);
            Vector3 offset = currentPos - last_position;
            last_position = currentPos;

            if (offset.x != 0f || offset.y != 0f)
            {
                offset = mTrans.InverseTransformDirection(offset);
                offset.Scale(Vector3.one);
                offset = mTrans.TransformDirection(offset);
            }
            MoveAbsolute(offset);
        }
    }
    public  void refreshBoxCollider()
    {
        bounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
        Debug.Log(bounds + " d sd ds ");
        NGUITools.AddWidgetCollider(gameObject);
    }
    /// <summary>
    /// Move the panel by the specified amount.
    /// </summary>
    
    public void MoveRelative(Vector3 relative)
    {
        //drag only on x axis
        Vector3 tf = mTrans.localPosition;
        Vector4 cr = panel.clipRange;
        if (dragType == DragType.Both)
        {
            tf.y += relative.y;
            tf.x += relative.x;
            cr.y -= relative.y;
            cr.x -= relative.x;
        }
        else if (dragType == DragType.Vertical)
        {
            tf.y += relative.y;
            cr.y -= relative.y;
         }
        else if (dragType == DragType.Horizontal)
        {
            tf.x += relative.x;
            cr.x -= relative.x;
        }
        mTrans.localPosition = tf;
        panel.clipRange = cr;
        

    }
    public void MoveAbsolute(Vector3 absolute)
    {
        Vector3 a = transform.InverseTransformPoint(absolute);
        Vector3 b = transform.InverseTransformPoint(Vector3.zero);
        MoveRelative(a - b);
    }
    protected void UpdateRealTimeDelta()
    {
        real_time = Time.realtimeSinceStartup;
        float delta = real_time - start_time;
        actual_time += Mathf.Max(0f, delta);
        time_delta = 0.001f * Mathf.Round(actual_time * 1000f);
        actual_time -= time_delta;
        if (actual_time > 0.25f) time_delta = 0.25f;
        start_time = real_time;
    }

    
}
