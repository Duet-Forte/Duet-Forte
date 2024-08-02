using Unity.VisualScripting;
using UnityEngine;

namespace Util
{
    public static class Const
    {
        #region 레벨당 스탯 계산을 위한 기초 스탯
        public const int PLAYER_BASE_HEALTH = 50;
        public const int PLAYER_BASE_ATTACK = 6;
        public const int PLAYER_BASE_DEFENSE = 2;
        #endregion

        #region 디버깅
        public const int DEBUG_PLAYER_LEVEL = 4;
        public const int DEBUG_PLAYER_CURRENT_EXP = 0;
        #endregion


        #region 판정 범위
        public const double MINUTE_TO_SECOND = 60d;
        public const int QUARTER_NOTE = 4;
        public const float BAD_JUDGE = 0.5f;
        public const float GOOD_JUDGE = 0.4f;
        public const float GREAT_JUDGE = 0.3f;
        public const float PERFECT_JUDGE = 0.2f;
        #endregion
        #region 패링시 얻는 클라이맥스 게이지
        public const int GUARD_COUNTER_GAUGE = 100;
        public const int PERFECT_ICREASE_GAUAGE = 15;
        public const int GREAT_ICREASE_GAUAGE = 8;
        public const int GOOD_ICREASE_GAUAGE = 4;
        #endregion

        #region 
        public const float ATTACK_DELAY_BEATS = 2f; // �� ���� ���� ���� �������� �̾����� ��.
        public const int MAX_EARLY_COUNT = 3;
        #endregion

        #region 경로
        public const string PATTERN_CSV_PATH = "CSV/PatternData";
        public const string STAGE_PATH = "Scriptable/Stage";
        public const string CUTSCENE_PATH = "Scriptable/Cutscene";
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

        public const string UI_STATUS_CANVAS_PATH = "UI/Status/UI_ParryGauge_Gauge01_Canvas";
        public const string UI_FIRST_TUTORIAL_PATH = "UI/Tutorial";

        public const string SAVE_FILE_NAME = "dfSave.json";
        #endregion

        #region UI 연출 스피드
        public const float STATUSUI_PROCESS_SPEED = 1f;
        public const float STATUSUI_WAIT_TIME = 2f;
        public const float STATUSUI_MAX_HP_RATIO = 1f;
        public const float STATUSUI_FADE_SPEED = 1f;
        public const float DAMAGEUI_UI_WIDTH = 35f;
        public const float DAMAGEUI_UI_HEIGHT = 50f;
        public const float DAMAGEUI_FADE_SPEED = 3.3f;
        #endregion

        #region �޴� UI ����
        public const int CONTENT_IN_ROW = 3;
        #endregion
        #region PlayerPrefs의 Key값
        public const string IS_TUTORIAL_END = "isTutorialEnd";
        #endregion
        #region 애니메이션 트리거
        public static int dustHash = Animator.StringToHash("Dust");
        public static int angryHash = Animator.StringToHash("Angry");
        public static int surpriseHash = Animator.StringToHash("Surprise");
        public static int dumbHash = Animator.StringToHash("Dumbfounded");
        public static int questionHash = Animator.StringToHash("Question");
        public static int drinkHash = Animator.StringToHash("Drink");
        #endregion

        #region ���̾�
        public static int PLAYER_LAYER = LayerMask.NameToLayer("Player");
        public static int WARPED_PLAYER_LAYER = LayerMask.NameToLayer("WarpedPlayer");
        #endregion
    }

    public static class Method
    { 
        public static T GetOrAddComponent<T>(Transform transform) where T : Component
        {
            T temp = transform.GetComponent<T>();
            if (temp == null) 
            { 
                temp = transform.AddComponent<T>();
            }
            return temp;
        }

        public static string ChangeToKRName(string speakerName)
        {
            switch(speakerName) 
            {
                case "Zio":
                    return "지오";
                case "Timmy":
                    return "티미";
                case "Ludius":
                    return "루디우스";
                case "Hila":
                    return "일라";
                case "Faber":
                    return "파베르";
                case "Novérca":
                    return "노베르카";
                default:
                    return speakerName;
            }
        }
    }
}
