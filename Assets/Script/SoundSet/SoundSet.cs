using UnityEngine;
using Util.CustomEnum;
namespace SoundSet
{

    public class PlayerSoundSet
    {
        //�Ʒ� �ش� ����ó�� public void �����̸�()���� �޼��带 ����� �ȿ� wwise ��ũ��Ʈ�� �ۼ��Ͻø� �˴ϴ�. 
        //������ ���� �ݵ�� public void�� ���� ���ֽð� �ּ����� ��� ��Ȳ���� ���̴� �������� ���� �����ּ���.


        //���� �ڵ� ����
        public void PerfectParrySound(GameObject parameterObject) { //����Ʈ ������ �и��� ���� �ÿ� ������ ����

            AkSoundEngine.PostEvent("Player_Parry_SFX", parameterObject);

        }
        //�����ڵ� ��
       
        public void PlayerAttack(GameObject parameterObject,bool isSlash)//�÷��̾ ������ �� ���� ����
        {
            if (isSlash)
            {
                AkSoundEngine.PostEvent("Player_Attack_A_SFX", parameterObject);
            }
            else {
                AkSoundEngine.PostEvent("Player_Attack_B_SFX", parameterObject);
            }

        }
        public void PlayerGuardCounter(GameObject parameterObject) { // ����ī���� ����
            AkSoundEngine.PostEvent("Player_Skill_Climax_SFX", parameterObject);
        }

      

    }

    public class EnemySoundSet
    { 
    }
    public class UISound {

        public void SignalSound(GameObject parameterObject,bool isStrong) {
            if (isStrong)
            {
                AkSoundEngine.PostEvent("Player_Attack_StrongSignal_SFX", parameterObject);
            }
            else {
                AkSoundEngine.PostEvent("Player_Attack_WeakSignal_SFX", parameterObject);
            }

        }
        public void AttackSignal(GameObject parameterObject, SignalInstrument signalInstrument) {
            AkSoundEngine.PostEvent($"Enemy_{signalInstrument.ToString()}_Attack_Signal_SFX", parameterObject);
        }
     
    }
    public class BackGroundMusic {

        public void GameOver(bool isWin) {
                Metronome.instance.GameOverMusic(isWin);
        }
    
    }

}