namespace Util
{
    public static class Const
    {
        #region 플레이어
        public const int INITIAL_HEALTH_POINT = 6;
        #endregion

        #region 판정
        public const float MINUTE_TO_SECOND = 60f;
        public const int QUARTER_NOTE = 4;
        public const float BAD_JUDGE = 0.5f;
        public const float GOOD_JUDGE = 0.4f;
        public const float GREAT_JUDGE = 0.3f;
        public const float PERFECT_JUDGE = 0.2f;
        #endregion

        #region 적 공격
        public const float ATTACK_DELAY_BEATS = 2f; // 몇 박을 쉬고 실제 공격으로 이어지는 지.
        public const int MAX_EARLY_COUNT = 3;
        #endregion

        #region 폴더 위치
        public const string PATTERN_CSV_PATH = "CSV/PatternData";
        public const string STAGE_PATH = "Scriptable/Stage";
        public const string TOPVIEW_PLAYER = "TopView/Player";
        public const string TOPVIEW_ENTITY = "TopView/Entity/";

        public const string ZERO_TO_STRING = "0";
        public const string ENEMY_CSV_PATH = "CSV/EnemyData";
        public const string PATTERN_SIGNATURE_HEADER = "Signature";
        public const string NAME_HEADER = "Name";
        public const string ENEMY_HP_HEADER = "HealthPoint";
        public const string ENEMY_SPAWNX_HEADER = "SpawnPointX";
        public const string ENEMY_SPAWNY_HEADER = "SpawnPointY";
        public const string SPRITE_PATH = "Sprite/";

        public const string UI_STATUS_PATH = "UI/Status/StatusUI";
        public const string UI_GUARDGAUGE_PATH = "UI/Status/GuardGauge";
        public const string UI_PLAYERHP_PATH = "UI/Status/PlayerHP";
        public const string UI_ENEMYHP_PATH = "UI/Status/EnemyHP";
        public const string UI_PLAYERHP_FRAME_PATH = "UI/Status/PlayerHPFrame";
        public const string UI_PREPARETURN = "UI/PrepareTurnUI";
        public const string UI_DAMAGE = "UI/Damage/Damage";
        public const string UI_TURN = "UI/TurnUI";
        public const string UI_ENEMYSIGNAL = "UI/SignalIconsCanvas";

        public const string TIMELINE_ZOOMIN = "TimeLine/BattlePosZoomIn";
        public const string TIMELINE_ZOOMOUT = "TimeLine/BattlePosZoomOut";

        public const string ENEMY_VFX_PATH = "VFX/VFX_Prefab/Combat/Enemy/";

        #endregion

        #region UI 연출 변수
        public const float STATUSUI_PROCESS_SPEED = 1f;
        public const float STATUSUI_WAIT_TIME = 2f;
        public const float STATUSUI_MAX_HP_RATIO = 1f;
        public const float STATUSUI_FADE_SPEED = 1f;
        public const float DAMAGEUI_UI_WIDTH = 35f;
        public const float DAMAGEUI_UI_HEIGHT = 50f;
        public const float DAMAGEUI_FADE_SPEED = 1f;
        #endregion

        #region 메뉴 UI 변수
        public const int CONTENT_IN_ROW = 3;
        #endregion
    }
}
