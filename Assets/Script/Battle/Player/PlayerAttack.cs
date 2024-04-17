using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Util;
using SoundSet;

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
    List<string> skillCommandEntered=new List<string>();//�Է¹��� Ŀ�ǵ�
    [SerializeField] List<CustomEnum.JudgeName> timingList;           //��������
    [SerializeField] string[] skillCommandEnteredToArray = { "R" };           //skillCommandEntered�� �迭�� �ٲ� ������ ����
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
    void Start()
    {
        
        playerSkillSet = GetComponent<SkillSet>();
        playerAttackStat= GetComponent<PlayerStatus>().PlayerAttack;
        bpm = Metronome.instance.getStage.BPM;

        playerAnimator = GetComponent<PlayerAnimator>();
        thePlayerAttackTimingCheck = FindObjectOfType<PlayerAttackTimingCheck>();
        theStageManager = FindObjectOfType<StageManager>();
        battlePresenter = theStageManager.BattlePresenter;
        inputBuffer = new Queue<string>();

    }

    public void AddCommand(string attackKey) {
        if (attackKey != "R")//���� Ȥ�� ����̸� 
        {
            skillCommandEntered.Add(attackKey);
            Debug.Log("������ ���� ����� : "+timingList.Last());
            if (attackKey == "A") {
                battlePresenter.PlayerBasicAttackToEnemy(new Damage(playerAttackStat,timingList.Last(),new SlashDamage())); //battlepresenter���� ����� ����
                return;
            }
            if (attackKey == "B") {
                battlePresenter.PlayerBasicAttackToEnemy(new Damage(playerAttackStat, timingList.Last(), new PierceDamage()));
                return;
            }
            
        }
        skillCommandEntered.Add(attackKey);
        timingList.Add(CustomEnum.JudgeName.Rest);

    }
    void AttackEnd() {//���ݱ�ȸ�� ������ ȣ��Ǵ� �Լ�
        skillCommandEntered.Clear();//�Է¹޾Ҵ� Ŀ�ǵ�Ű �ʱ�ȭ
        Array.Clear(skillCommandEnteredToArray, 0, skillCommandEnteredToArray.Length);//���������� Ŀ�ǵ� �ʱ�ȭ
        timingList.Clear();//���� �ʱ�ȭ
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
    
   
    void CheckSkillCommand() {
       
        
        if (skillCommandEnteredToArray != skillCommandEntered.ToArray())
        {
            skillCommandEnteredToArray = skillCommandEntered.ToArray();
        }
        
        for (int skillCommandEnteredIndex = 0; skillCommandEnteredIndex < skillCommandEnteredToArray.Length; skillCommandEnteredIndex++) {
            string[] comparisonCommand = new string[skillCommandEnteredToArray.Length-skillCommandEnteredIndex];//�Է¹��� Ŀ�ǵ带 �����ؼ� ��
            Array.Copy(skillCommandEnteredToArray,skillCommandEnteredIndex,comparisonCommand,0,skillCommandEnteredToArray.Length-skillCommandEnteredIndex);//�Է¹��� ��ü Ŀ�ǵ带 ������ �Էº��� ����鼭 Ŀ�ǵ� �˻��� �迭 �����


            for (int skillSetIndex = 0; skillSetIndex < playerSkillSet.SkillCommand.ToArray().Length; skillSetIndex++)
            {
                if (Enumerable.SequenceEqual(comparisonCommand, playerSkillSet.SkillCommand[skillSetIndex]))//�Է��� Ŀ�ǵ尡 ��ų���� ������ ��ų Ŀ�ǵ�� ����
                { 
                  //��ų qte���� Ȯ�� �� ó��
                  //��ų�̸��� ��ų�̸��˾��� �Ѱ��ֱ�
                  //��ƼŬ�� ���� ����ɶ� ���� ���
                    Debug.Log($"��ų {playerSkillSet.ArrayOfSkillName[skillSetIndex]} �ߵ���");
                    Debug.Log($"{skillSetIndex}��° ��ƼŬ");
                    currentSkill = playerSkillSet.ArrayOfSkill[skillSetIndex];
                    SkillUI(playerSkillSet.ArrayOfSkillIcon[skillSetIndex], playerSkillSet.ArrayOfSkillName[skillSetIndex]);
                    castSkillParticle = playerSkillSet.ArrayOfSkillParticle[skillSetIndex];//��ƼŬ������ PlayerSkill.Skill�� ���Ե� ���� ����
                    StartCoroutine(SkillCast(currentSkill));

                    isCastSkill = true;
                    //��ų�׼Ǿ�
                    //����� ó��
                    //��ų ���ʽ� ȿ�� ����
                }
            }
        }
    }
    IEnumerator SkillCast(PlayerSkill.Skill currentSkill) {
        yield return new WaitForSeconds(0.5f);

        ParticleSystem tmp = GameObject.Instantiate(currentSkill.skillParticle, transform.position, Quaternion.identity);
        tmp.transform.parent = gameObject.transform;
        tmp.gameObject.transform.localScale = gameObject.transform.localScale;
        
        //�ִϸ��̼� Ŭ�� �����Ҵ�
        playerAnimator.Skill(currentSkill.skillClip);
        
        for (int i = 0; i < currentSkill.damage.Length; i++) { 
        yield return new WaitForSeconds(currentSkill.waitTimes[i]);
            battlePresenter.PlayerSkillToEnemy(new Damage(currentSkill.damage[i], currentSkill.damageType[i]?new SlashDamage():new PierceDamage()));
        }

    }

    #region ��ų �˾� UI
    void SkillUI(Sprite skillImage,string skillName) {
        skillPopUpUI=Instantiate(Resources.Load<GameObject>("UI/SkillPopUpUICanvas")).GetComponentInChildren<SkillPopUpUI>();
        skillPopUpUI.Appear(skillImage, skillName);
        
    }
    void SkillUI() { skillPopUpUI.Disappear(); }
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
        }
    
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

                /*AddCommand("A");
                playerAnimator.Attack(true);
                playerSoundSet.PlayerAttack(gameObject, true);*/
                //����� ó��
                EnqueueAttackBuffer("A");
                checkAttackTiming();
                canAttack = false;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                /*AddCommand("B");
                playerAnimator.Attack(false);
                playerSoundSet.PlayerAttack(gameObject, false);*/
                //����� ó��
                EnqueueAttackBuffer("B");
                checkAttackTiming();
                canAttack = false;
            }
            CheckSkillCommand();
            if (isCastSkill) {
                
                Debug.Log("��ų �ߵ���");
                
                AttackEnd();
                
                Debug.Log(castSkillParticle.main.duration+"�� ���");
                yield return new WaitForSeconds(3f);//��ų �ִϸ��̼� Ŭ���� ���̸�ŭ ��� �� ����� �����ؾ���. 04.01
                SkillUI();
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
    
    //�Է¹��� Ŀ�ǵ尡 ��ų�¿� �ִ� Ŀ�ǵ����� �˻�

    float CalculateBasicAttackDamage(CustomEnum.JudgeName judgeName) {
        if (judgeName == CustomEnum.JudgeName.Miss) { //Rest����
            return 0;
        }
        if (judgeName == CustomEnum.JudgeName.Perfect) {
            return playerAttackStat * 1.0f;
        }
        if (judgeName == CustomEnum.JudgeName.Great) {
            return playerAttackStat * 0.8f;
        }
        if (judgeName == CustomEnum.JudgeName.Good)
        {
            return playerAttackStat * 0.5f;
        }
        if (judgeName == CustomEnum.JudgeName.Bad)
        {
            return playerAttackStat * 0.2f;
        }
        return 0;
    }
}
