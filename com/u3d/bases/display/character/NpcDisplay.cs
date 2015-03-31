using com.u3d.bases.display.controler;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/1/6 11:16:24 
 * function: NPC显示对象
 * *******************************************************/
namespace com.u3d.bases.display.character
{
    public class NpcDisplay : ActionDisplay
    {
        override protected string SortingLayer { get { return "Player"; } }

        public int TaskState=0;            //任务状态值
        public GameObject GoTaskState;     //任务状态图标引用

        public NpcDisplay() {
        }

        /**添加控制脚本**/
        override protected void AddScript(GameObject go)
        {
            //增加控制中心控制脚本
            if (go.GetComponent<ActionControler>() != null) return;
            Controller = go.AddComponent<ActionControler>();
            Controller.Me = this;
            Controller.Me.Animator = Controller.Me.GoCloth.GetComponent<Animator>();
            Controller.Me.Animator.applyRootMotion = false;

            BoxCollider2D = GoCloth.GetComponent<BoxCollider2D>();
            if (BoxCollider2D!=null)
            {
                BoxCollider2D.enabled = false;
            }
        }

        override public void Dispose()
        {
            TaskState = 0;
            RemoveTaskState();
            base.Dispose();
        }

        /**移除任务状态**/
        public void RemoveTaskState() {
            if (GoTaskState == null) return;
            if (!GoTaskState.activeSelf) GoTaskState.SetActive(true);
            Dispose(GoTaskState);
            GoTaskState = null;
        }
    }
}
