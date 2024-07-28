using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Util.CustomEnum;
using SoundSet;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour //�÷��̾��� �Է��� �޾Ƽ� ��ų Ŀ�ǵ�� ��
{
    //A������ ��� B������ ���� / A������ Ű���� FŰ B������ Ű���� JŰ�� ����(������ ��Ÿ��)
    [SerializeField]bool canAttack = true;           //���ݰ��ɿ��� (��Ÿ�ӿ� ����)
    private bool isCastSkill = false;
    private int currentLimitBeat = 0;//���� ���ѽð�
    private int limitBeat = 10;                      //�ִ� ���ѽð� ������ ����
    private int bpm;
    private float playerAttackStat;
    
    #region ��Ÿ�� ���� ����
    
    double elapsedRestTime = 0d;
    #endregion

    #region Ŀ�ǵ� ���� �ڷ���
    string skillCommandEntered="";                                 //�Է¹��� Ŀ�ǵ�
    [SerializeField] List<JudgeName> timingList;                   //��������
    [SerializeField] ParticleSystem castSkillParticle;
    Queue<string> inputBuffer;
    bool canInputBuffer;
    #endregion

    #region �ܺ� Ŭ����
    SkillSet playerSkillSet;
    PlayerSkill.Skill currentSkill;
    PlayerAnimator playerAnimator;
    PlayerAttackTimingCheck thePlayerAttackTimingCheck;
    StageManager theStageManager;
    BattlePresenter battlePresenter;
    PlayerSoundSet playerSoundSet=new PlayerSoundSet();
    Animation anim;

    SkillPopUpUI skillPopUpUI;
    
    #endregion
    // Start is called before the first frame update
    public void InitSettins(StageManager stageManager,PlayerAttackTimingCheck playerAttackTimingCheck)
    {
        
        playerSkillSet = GetComponent<SkillSet>();
        playerAttackStat= GetComponent<PlayerStatus>().PlayerAttack;
        bpm = Metronome.instance.getStage.BPM;

        playerAnimator = GetComponent<PlayerAnimator>();
        thePlayerAttackTimingCheck = playerAttackTimingCheck;
        thePlayerAttackTimingCheck.InitSettings();
        theStageManager = stageManager;
        battlePresenter = theStageManager.BattlePresenter;
        inputBuffer = new Queue<string>();

    }

    public void AddCommand(string attackKey) {
        if (attackKey != "R")//���� Ȥ�� ����̸� 
        {
            skillCommandEntered+=attackKey;
            Debug.Log("������ ���� ����� : "+timingList.Last());
            if (attackKey == "A") {
                battlePresenter.PlayerBasicAttackToEnemy(new Damage(playerAttackStat,timingList.Last(),new SlashDamage())); //battlepresenter���� ����� ����
                return;
            }
            if (attackKey == "B") {
                battlePresenter.PlayerBasicAttackToEnemy(new Damage(playerAttackStat,timingList.Last(), new PierceDamage()));
                return;
            }
            
        }
        skillCommandEntered+=attackKey;
        timingList.Add(JudgeName.Rest);

    }
    void AttackEnd() {//���ݱ�ȸ�� ������ ȣ��Ǵ� �Լ�
        skillCommandEntered="";//�Է¹޾Ҵ� Ŀ�ǵ�Ű �ʱ�ȭ
        timingList.Clear();//���� �ʱ�ȭ
        ClearBuffer();//��ǲ ���� �ʱ�ȭ
        Metronome.instance.OnBeating -= CountDownOnBeat;//���ѽð� ���ҽ�Ű�� �̺�Ʈ �������
        Metronome.instance.OffBeating -= AttackOnBeat;
        canAttack = false;//���� �Ұ���
    }
    
    void AttackSetting() {//���ݽ��۽� ����

        
        isCastSkill = false;//��ų��뿩��
        canAttack = true;   //���ݰ��ɿ��� (���ڸ��� true�� ����)
        currentLimitBeat = limitBeat;//���ѽð� �ʱ�ȭ
    }
    void CountDownOnBeat() {//���� ���ѽð�
        currentLimitBeat--;
    }
    
   
    private void CheckSkillCommand() {
       
        
       
        
        for (int skillCommandEnteredIndex = 0; skillCommandEnteredIndex < skillCommandEntered.Length; skillCommandEnteredIndex++) { //

            string comparisonCommands = skillCommandEntered.Substring(skillCommandEnteredIndex,skillCommandEntered.Length-skillCommandEnteredIndex);

            for (int skillSetIndex = 0; skillSetIndex < playerSkillSet.SkillCommand.ToArray().Length; skillSetIndex++)
            {
                if ( comparisonCommands==playerSkillSet.SkillCommand[skillSetIndex])//�Է��� Ŀ�ǵ尡 ��ų���� ������ ��ų Ŀ�ǵ�� ����
                { 
                  //��ų qte���� Ȯ�� �� ó��
                  //��ų�̸��� ��ų�̸��˾��� �Ѱ��ֱ�
                  //��ƼŬ�� ���� ����ɶ� ���� ���
                    currentSkill = playerSkillSet.ArrayOfSkill[skillSetIndex];
                    
                    SkillUI(playerSkillSet.ArrayOfSkillIcon[skillSetIndex] , playerSkillSet.ArrayOfSkillName[skillSetIndex]);

                    JudgeName[] judgeArr= timingList.ToArray();

                    StartCoroutine(SkillCast(currentSkill,comparisonCommands,judgeArr));

                    isCastSkill = true;
                    //��ų�׼Ǿ�
                    //����� ó��
                    //��ų ���ʽ� ȿ�� ����
                }
            }
        }

        
    }

    private JudgeName CalcJudgeName(string comparisonCommands, JudgeName[] judgeArr)
    {
        if (judgeArr.Length > comparisonCommands.Length)
        {
            judgeArr = judgeArr.Skip(judgeArr.Length-comparisonCommands.Length).ToArray();
        }
        int sum=0;
        int restCount = 0;
        int judgeAverage;
        for (int i = 0; i < comparisonCommands.Length; i++) {
            if (comparisonCommands[i] == 'R') { restCount++; }
            sum += (int)judgeArr[i];
        }
        judgeAverage = sum/(comparisonCommands.Length-restCount);
        switch (judgeAverage) {
            case 1:return JudgeName.Perfect;
            case 2:return JudgeName.Great;
            case 3:return JudgeName.Good;
            case 4:return JudgeName.Bad;
            default:return JudgeName.Bad;
        
        }



    }
    IEnumerator SkillCast(PlayerSkill.Skill currentSkill,string comparisonCommand, JudgeName[] judgeArr) {
        yield return new WaitForSeconds(0.5f); // �ڿ������� ������


        currentSkill.PlaySkillSound(currentSkill.soundEventName, gameObject); //��ų ����
        ParticleSystem tmp =Instantiate(currentSkill.skillParticle, transform.position, Quaternion.identity); //��ƼŬ ����
        tmp.transform.parent = gameObject.transform;  //��ƼŬ �θ�����
        tmp.gameObject.transform.localScale = gameObject.transform.localScale; //��ġ ����
        
        //�ִϸ��̼� Ŭ�� �����Ҵ�
        playerAnimator.Skill(currentSkill.skillClip);
        
        for (int i = 0; i < currentSkill.damage.Length; i++) { 
        yield return new WaitForSeconds(currentSkill.waitTimes[i]);
            battlePresenter.PlayerSkillToEnemy(new Damage(currentSkill.damage[i]*playerAttackStat, CalcJudgeName(comparisonCommand,judgeArr), currentSkill.damageType[i]?new SlashDamage():new PierceDamage()));
        }

    }

    #region ��ų �˾� UI
    void SkillUI(Sprite skillImage,string skillName) {
        //skillPopUpUI=Instantiate(Resources.Load<GameObject>("UI/SkillPopUpUICanvas")).GetComponentInChildren<SkillPopUpUI>();
        //skillPopUpUI.Appear(skillImage, skillName);
        theStageManager.UIMAnager.AppearSkillPopUp(skillImage, skillName);


    }
    void SkillUI() { 
        //skillPopUpUI.Disappear(); 
        theStageManager.UIMAnager.DisAppearSkillPopUp();
    }
    #endregion
    // Update is called once per frame
    void BasicAttackCoolDown() {
        
        if (Metronome.instance.CurrentTime >= Metronome.instance.SecondsPerBeat * 0.5 && Metronome.instance.CurrentTime <= Metronome.instance.SecondsPerBeat * 0.5 + Time.deltaTime) {//���� ������ ����...������ ���� ����� ��ã��
            canAttack = true;
            return ;
        }
        
    }
    

    void AttackOnBeat() {  //Metronome.Onbeat�� ������ ����
                           //queueī��Ʈ�� �ּ� 1�̻� ���� ��
                           //Metronome�� OnBeat���� dequeue�ϱ�
        if (inputBuffer.Count == 0) { AddCommand("R"); return; }//���۰� ����ִٸ� ��ǥ �߰�
        if (inputBuffer.Count >= 1) {
            string attackKey = inputBuffer.Dequeue();
            if (attackKey == "A") {
                AddCommand(attackKey);
                playerAnimator.Attack(true);
                playerSoundSet.PlayerAttack(gameObject, true);
            }
            if (attackKey == "B")
            {
                AddCommand(attackKey);
                playerAnimator.Attack(false);
                playerSoundSet.PlayerAttack(gameObject, false);
            }

        }
        
        //�ִϸ��̼�
        //Ŀ�ǵ� �Է�
    }
    private void EnqueueAttackBuffer(string attackKey) {
        if (inputBuffer.Count < 2)
        {
            if (inputBuffer.Count == 1) Debug.Log("���ۿ� �߰���");
          
                inputBuffer.Enqueue(attackKey);
                checkAttackTiming();
        }
    
    }
    private void ClearBuffer() { 
        inputBuffer.Clear();
    }
    private void checkAttackTiming() {
        timingList.Add(thePlayerAttackTimingCheck.CheckTiming());
    }
    public IEnumerator StartAttack() {
        AttackSetting();
        Metronome.instance.OnBeating += CountDownOnBeat;
        Metronome.instance.OffBeating += AttackOnBeat;
        
        while (true)
        {
            //RestCoolTime();
            BasicAttackCoolDown();
            if (Input.GetKeyDown(KeyCode.F))
            {  //���߿� PlayerInput Ŭ�������� ������ ������ �� ����

                Debug.Log("PlayerAttackŬ�������� ���� �Էµ�. ");
                EnqueueAttackBuffer("A");
                canAttack = false;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("PlayerAttackŬ�������� ���� �Էµ�. ");
                EnqueueAttackBuffer("B");
                canAttack = false;
            }
            CheckSkillCommand();
            if (isCastSkill) {
                
                Debug.Log("��ų �ߵ���");
                yield return new WaitForSeconds(3f);//��ų �ִϸ��̼� Ŭ���� ���̸�ŭ ��� �� ����� �����ؾ���. 04.01
                SkillUI();
                AttackEnd();
                
               
                yield break;// ��ų�� �ߵ��Ǿ� �ݺ��� Ż�� ���� ����
            }
            if (currentLimitBeat == 0) {
                
                Debug.Log("���� �õ� �� �ִ��Ʈ �ʰ�");
                
                AttackEnd();
                yield return new WaitForSeconds((float)Metronome.instance.SecondsPerBeat);
                yield break;//���ѽð� ���� ��������
            }
            yield return null;
        }
    }
    
   

  
}
