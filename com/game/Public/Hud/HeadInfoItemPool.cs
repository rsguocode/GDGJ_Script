﻿﻿﻿
using com.game.Public.PoolManager;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/04/03 11:54:23 
 * function: 头顶信息对象池
 * *******************************************************/

namespace com.game.Public.Hud
{
    public class HeadInfoItemPool
    {

        public static HeadInfoItemPool Instance = Instance ?? new HeadInfoItemPool();
        private Transform _headInfoItemPrefab;
        private const string PoolName = "HeadInfoItem";
        private SpawnPool _pool;
        private readonly Vector3 _swapPos = new Vector3(0,-2000,0); //生成在屏幕外，防止闪烁

        public void Init(Transform headInfoItemPrefab)
        {
            _headInfoItemPrefab = headInfoItemPrefab;

            //设置一个Group，便于管理
            _pool = PoolManager.PoolManager.Pools.Create(PoolName);
            _pool.group.parent = PoolManager.PoolManager.PoolParent;
            _pool.group.localPosition = new Vector3(0, 0, 0);
            _pool.group.localRotation = Quaternion.identity;

            //设置一些其他属性，比如预加载，比如最大缓存数量
            var prefabPool = new PrefabPool(_headInfoItemPrefab) {preloadAmount = 30};
            //prefabPool.limitInstances = true;
            //prefabPool.limitAmount = 5;      //设置最大缓存数量
            //prefabPool.limitFIFO = true;
            _pool.CreatePrefabPool(prefabPool);
        }

        /// <summary>
        /// 通过GameObject池获取HeadInfoItem
        /// </summary>
        /// <returns></returns>
        public Transform SpawnHeadInfoItem()
        {
            return _pool.Spawn(_headInfoItemPrefab, _swapPos, Quaternion.identity);
        }

        /// <summary>
        /// 通过GameObject池回收MailItem
        /// </summary>
        /// <param name="headInfoItem"></param>
        public void DeSpawn(Transform headInfoItem)
        {
            //回收前重新设置一下父对象，便于管理
            headInfoItem.parent = _pool.transform;
            _pool.Despawn(headInfoItem);
        }

         
    }
}