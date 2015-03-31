﻿﻿﻿
using com.game.Public.PoolManager;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/23 12:46:11 
 * function:
 * *******************************************************/

namespace com.game.module.Task
{
    public class TaskItemPool
    {
        public static TaskItemPool Instance = Instance ?? new TaskItemPool();
        private Transform _taskItemPrefab;
        private const string _poolName = "TaskItem";
        private SpawnPool pool;

        public void Init(Transform taskItemPrefab)
        {
            _taskItemPrefab = taskItemPrefab;

            //设置一个Group，便于管理
            this.pool = PoolManager.Pools.Create(_poolName);
            this.pool.group.parent = PoolManager.PoolParent;
            this.pool.group.localPosition = new Vector3(0, 0, 0);
            this.pool.group.localRotation = Quaternion.identity;

            //设置一些其他属性，比如预加载，比如最大缓存数量
            PrefabPool prefabPool = new PrefabPool(_taskItemPrefab);
            prefabPool.preloadAmount = 5;      // 设置预加载的数量
            //prefabPool.limitInstances = true;
            //prefabPool.limitAmount = 5;      //设置最大缓存数量
            //prefabPool.limitFIFO = true;
            this.pool.CreatePrefabPool(prefabPool);
        }

        /// <summary>
        /// 通过GameObject池获取TaskItem
        /// </summary>
        /// <returns></returns>
        public Transform SpawnTaskItem()
        {
            return this.pool.Spawn(_taskItemPrefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// 通过GameObject池回收TaskItem
        /// </summary>
        /// <param name="taskItem"></param>
        public void DeSpawn(Transform taskItem)
        {
            //回收前重新设置一下父对象，便于管理
            taskItem.parent = pool.transform;
            this.pool.Despawn(taskItem);
        }

         
    }
}