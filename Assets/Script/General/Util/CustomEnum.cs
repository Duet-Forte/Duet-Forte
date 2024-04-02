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
            ActionStartTurn=0,// ���� ��ġ�� �̵� ī�޶� ����- ����
            PlayerTurn,
            EnemyTurn,
            GuardCounterTurn,
            ActionEndTurn,// ���� ��ġ�� �̵� ī�޶� ���� - �ܾƿ�
            PrepareTurn,
            NumberOfTurnTypes
        };

        public enum QTEJudge
        { 
            Perfect,
            Good,
            Miss
        };

        public enum Speaker
        {
            player,
            NPC,
            Empty
        }

    }
}