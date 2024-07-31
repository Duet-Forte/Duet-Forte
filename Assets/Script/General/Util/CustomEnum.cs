namespace Util
{
    namespace CustomEnum
    {
        public enum JudgeName
        {
            Rest = 0,
            Perfect=1,
            Great=2,
            Good=3,
            Bad=4,
            Miss=5
            
        }
        public enum Turn
        {
            ActionStartTurn=0,// 전투 위치로 이동 카메라 연출- 줌인
            PlayerTurn,
            EnemyTurn,
            GuardCounterTurn,
            ActionEndTurn,// 원래 위치로 이동 카메라 연출 - 줌아웃
            PrepareTurn,
            NumberOfTurnTypes
        };

        public enum QTEJudge
        { 
            Perfect,
            Good,
            Miss
        };
        public enum DamageType { 
        
            Slash,
            Pierce,
            GuardCounter,
            Enemy
        };

        public enum Speaker
        {
            player,
            NPC,
            Empty
        }

        public enum PlayerAction
        {
            Move,
            Interact,
            Skip
        }

        public enum Rank
        {
            S,
            APlus,
            A,
            BPlus,
            B,
            C,
            Empty
        }

        public enum SignalInstrument
        {
            ElecGuitar,
            Tambourine,
            Castanets,
            Timpani
        }
        public enum EventType
        { 
            None,
            Emotion,
            Quest
        }
    
    }
}