﻿﻿﻿
using com.game.Public.PoolManager;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/31 04:48:30 
 * function: DeedItem对象池
 * *******************************************************/

namespace Com.Game.Module.Role.Deed
{
    public class DeedItemPool
    {

        public static DeedItemPool Instance = Instance ?? new DeedItemPool();
        private Transform _deedItemPrefab;
        private const string _poolName = "DeedItem";
        private SpawnPool pool;

        public void Init(Transform deedItemPrefab)
        {
            _deedItemPrefab = deedItemPrefab;

            //设置一个Group，便于管理
            this.pool = PoolManager.Pools.Create(_poolName);
            this.pool.group.parent = PoolManager.PoolParent;
            this.pool.group.localPosition = new Vector3(0, 0, 0);
            this.pool.group.localRotation = Quaternion.identity;

            //设置一些其他属性，比如预加载，比如最大缓存数量
            PrefabPool prefabPool = new PrefabPool(deedItemPrefab);
            prefabPool.preloadAmount = 35;      // 设置预加载的数量
            //prefabPool.limitInstances = true;
            //prefabPool.limitAmount = 5;      //设置最大缓存数量
            //prefabPool.limitFIFO = true;
            this.pool.CreatePrefabPool(prefabPool);
        }

        /// <summary>
        /// 通过GameObject池获取MailItem
        /// </summary>
        /// <returns></returns>
        public Transform SpawnDeedItem()
        {
            return this.pool.Spawn(_deedItemPrefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// 通过GameObject池回收MailItem
        /// </summary>
        /// <param name="deedItem"></param>
        public void DeSpawn(Transform deedItem)
        {
            //回收前重新设置一下父对象，便于管理
            deedItem.parent = pool.transform;
            this.pool.Despawn(deedItem);
        }

         
    }
}