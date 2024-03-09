
using System;
using System.Collections.Generic;


namespace BlackJack.ProjectL.Misc
{
    // 只支援数组，不支援List
    public partial class LocalAccountConfigData
    {
        // 已读的公告及活动
        public string[] HaveReadAnnounceActivities;
        public Dictionary<ulong, DateTime> LastReadActivityTimeList;
        public Dictionary<int, DateTime> LastReadRecommendGiftBoxTimeDic;
        public Dictionary<long, DateTime> LastReadStoreItemTimeDic;

        // 已经读过的世界树记忆剧情
        public HashSet<int> ReadTreeMemoryScenarios;

        // 已经读过的英雄生平和语音
        public int[] HaveReadHeroBiographyIds;
        public int[] HaveReadHeroPerformanceIds;

        // 已经解锁的英雄生平,语音,英雄副本和羁绊
        public int[] UnlockHeroBiographyIds;
        public int[] UnlockHeroPerformanceIds;
        public int[] UnlockHeroDungeonLevelIds;
        public int[] UnlockHeroFetterIds;

        // 竞技场上阵英雄，按照行动顺序排序
        public int[] ArenaAttackerHeroIds;

        // 组队默认等级范围
        public int TeamPlayerLevelMin = 0;
        public int TeamPlayerLevelMax = 0;

        // 实时PVP上阵保护，强制下阵说明
        public bool IsRealtimePVPShowNotice = true;

        // 是否完成过碎片萃取
        public bool HaveDoneMemoryExtraction = false;

        // 上一次淘汰赛提示1的时间
        public DateTime LastNotifyPeakArenaTime1 = DateTime.MinValue;

        // 上一次淘汰赛提示2的时间
        public DateTime LastNotifyPeakArenaTime2 = DateTime.MinValue;

        public string GMString;

        // 上次巅峰比赛的轮次
        public int LastPeakArenaMatchRound = 0;

        // 巅峰比赛的轮次是否发生改变
        public bool IsPeakArenaMatchRoundChanged = false;

        //世界背景音乐名字
        public string WorldMusicName;

        // 炼金批量添加界面装备toggle勾选状态
        public int AlchemyAddPanelEquipmentToggleState = 0;
        // 炼金批量添加界面强化toggle勾选状态
        public int AlchemyAddPanelStrengthenToggleState = 0;
    }

    // 储存在本地的数据，每个账号独立
    public partial class LocalAccountConfig
    {
        public LocalAccountConfig()
        {
            m_data = new LocalAccountConfigData();
        }

        public void SetFileName(string name)
        {
            m_fileName = name;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(m_fileName))
                return false;

            string saveText = JsonUtility.Serialize(m_data);
            return FileUtility.WriteText(m_fileName, saveText);
        }

        public bool Load()
        {
            if (string.IsNullOrEmpty(m_fileName))
            {
                ResetLocalAccountConfigData();
                return false;
            }

            if (!FileUtility.IsFileExist(m_fileName))
            {
                ResetLocalAccountConfigData();
                return false;
            }

            string saveText = FileUtility.ReadText(m_fileName);
            if (string.IsNullOrEmpty(saveText))
            {
                ResetLocalAccountConfigData();
                return false;
            }

            var data = JsonUtility.Deserialize<LocalAccountConfigData>(saveText);

            if (data == null)
            {
                Debug.LogError(string.Format("LocalAccountConfig.Load {0} failed.", saveText));
                ResetLocalAccountConfigData();
                return false;
            }

            var jsonData = JsonMapper.ToObject(saveText);

            m_data = data;

            return true;
        }

        /// <summary>
        /// 重置数据(于Load失败时调用)
        /// </summary>
        private void ResetLocalAccountConfigData()
        {
           
        }

        public LocalAccountConfigData Data { get { return m_data; } }
        public static LocalAccountConfig Instance { set { s_instance = value; } get { return s_instance; } }

        static LocalAccountConfig s_instance;
        static string m_fileName;
        LocalAccountConfigData m_data;
    }
}
