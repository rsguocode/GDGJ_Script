﻿﻿﻿using com.game.Public.PoolManager;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/16 04:36:52 
 * function:  消息提示对象池
 * *******************************************************/

namespace com.game.Public.Message
{
    public class MessageItemPool
    {

        public static MessageItemPool Instance = Instance ?? new MessageItemPool();
        private Transform _messageItemPrefab;
        private const string PoolName = "MessageItem";
        private SpawnPool _pool;

        public void Init(Transform mailItemPrefab)
        {
            _messageItemPrefab = mailItemPrefab;

            //设置一个Group，便于管理
            _pool = PoolManager.PoolManager.Pools.Create(PoolName);
            _pool.group.parent = PoolManager.PoolManager.PoolParent;
            _pool.group.localPosition = new Vector3(0, 0, 0);
            _pool.group.localRotation = Quaternion.identity;

            //设置一些其他属性，比如预加载，比如最大缓存数量
            var prefabPool = new PrefabPool(_messageItemPrefab);
            prefabPool.preloadAmount = 3;      // 设置预加载的数量
            //prefabPool.limitInstances = true;
            //prefabPool.limitAmount = 2;      //设置最大缓存数量
            //prefabPool.limitFIFO = true;
            _pool.CreatePrefabPool(prefabPool);
        }

        /// <summary>
        /// 通过GameObject池获取MailItem
        /// </summary>
        /// <returns></returns>
        public Transform SpawnMessageItem()
        {
            return _pool.Spawn(_messageItemPrefab, Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// 通过GameObject池回收MailItem
        /// </summary>
        /// <param name="messageItem"></param>
        public void DeSpawn(Transform messageItem)
        {
            //回收前重新设置一下父对象，便于管理
            messageItem.parent = _pool.transform;
            _pool.Despawn(messageItem);
        }
         
    }
}