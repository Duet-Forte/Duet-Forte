namespace Util
{
    namespace CustomEnum
    {
        public enum JudgeName
        {
            Rest=0,
            Perfect,
            Great,
            Good,
            Bad,
            Miss
            
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
            GuardCounter
        };

        public enum Speaker
        {
            player,
            NPC,
            Empty
        }


    }
}