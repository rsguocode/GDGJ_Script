using com.game.module.test;
using Proto;

namespace Com.Game.Module.TopList
{
	class RankMode : BaseMode<RankMode>
	{
        
        public const int LEVEL = 1; //等级排行
        public const int FIGHT= 2;//战力
        public const int GOLD = 3; //金币排行
	    public const int Battle = 4; //竞技场排行

	    private RankInfoMsg_20_1[] rankInfoList = new RankInfoMsg_20_1[3];

        /// <summary>
        /// 设置排行榜信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="rankInfo">排行榜信息</param>
	    public void SetRankInfo(int type, RankInfoMsg_20_1 rankInfo)
        {
            rankInfoList[type-1] = rankInfo;
            DataUpdate(type);
        }

        /// <summary>
        /// 获取对应的排行榜信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public RankInfoMsg_20_1 GetRankInfo(ushort type)
	    {
	        return rankInfoList[type-1];
	    }

	}
}
