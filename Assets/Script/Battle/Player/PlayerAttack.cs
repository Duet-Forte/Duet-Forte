using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Util.CustomEnum;
using SoundSet;

public class PlayerAttack : MonoBehaviour //플레이어의 입력을 받아서 스킬 커맨드와 비교
{
    //A공격이 약공 B공격이 강공 / A공격은 키보드 F키 B공격은 키보드 J키로 설정(뮤즈대시 스타일)
    [SerializeField]bool canAttack = true;           //공격가능여부 (쿨타임에 의한)
    private bool isCastSkill = false;
    private int currentLimitBeat = 0;//남은 제한시간
    private int limitBeat = 10;                      //최대 제한시간 단위는 박자
    private int bpm;
    private float playerAttackStat;
    
    #region 쿨타임 관련 변수
    
    double elapsedRestTime = 0d;
    #endregion

    #region 커맨드 관련 자료형
    string skillCommandEntered="";                                 //입력받은 커맨드
    [SerializeField] List<JudgeName> timingList;                   //판정여부
    [SerializeField] string[] skillCommandEnteredToArray = { "R" };//skillCommandEntered를 배열로 바꿔 저장할 변수
    [SerializeField] ParticleSystem castSkillParticle;
    Queue<string> inputBuffer;
    bool canInputBuffer;
    #endregion

    #region 외부 클래스
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
        if (attackKey != "R")//베기 혹은 찌르기이면 
        {
            skillCommandEntered+=attackKey;
            Debug.Log("마지막 판정 디버깅 : "+timingList.Last());
            if (attackKey == "A") {
                battlePresenter.PlayerBasicAttackToEnemy(new Damage(playerAttackStat,timingList.Last(),new SlashDamage())); //battlepresenter에게 대미지 전달
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
    void AttackEnd() {//공격기회가 끝나면 호출되는 함수
        skillCommandEntered="";//입력받았던 커맨드키 초기화
        Array.Clear(skillCommandEnteredToArray, 0, skillCommandEnteredToArray.Length);//마찬가지로 커맨드 초기화
        timingList.Clear();//판정 초기화
        ClearBuffer();//인풋 버퍼 초기화
        Metronome.instance.OnBeating -= CountDownOnBeat;//제한시간 감소시키는 이벤트 구독취소
        Metronome.instance.OffBeating -= AttackOnBeat;
        canAttack = false;//공격 불가능
    }
    
    void AttackSetting() {//공격시작시 세팅

        
        isCastSkill = false;//스킬사용여부
        canAttack = true;   //공격가능여부 (엇박마다 true로 변함)
        currentLimitBeat = limitBeat;//제한시간 초기화
    }
    void CountDownOnBeat() {//공격 제한시간
        currentLimitBeat--;
    }
    
   
    private void CheckSkillCommand() {
       
        
       
        
        for (int skillCommandEnteredIndex = 0; skillCommandEnteredIndex < skillCommandEntered.Length; skillCommandEnteredIndex++) { //

            string comparisonCommands = skillCommandEntered.Substring(skillCommandEnteredIndex,skillCommandEntered.Length-skillCommandEnteredIndex);

            for (int skillSetIndex = 0; skillSetIndex < playerSkillSet.SkillCommand.ToArray().Length; skillSetIndex++)
            {
                if ( comparisonCommands==playerSkillSet.SkillCommand[skillSetIndex])//입력한 커맨드가 스킬셋의 임의의 스킬 커맨드와 같음
                { 
                  //스킬 qte여부 확인 후 처리
                  //스킬이름을 스킬이름팝업에 넘겨주기
                  //파티클이 전부 재생될때 까지 대기
                    Debug.Log($"스킬 {playerSkillSet.ArrayOfSkillName[skillSetIndex]} 발동됨");
                    Debug.Log($"{skillSetIndex}번째 파티클");
                    currentSkill = playerSkillSet.ArrayOfSkill[skillSetIndex];
                    
                    SkillUI(playerSkillSet.ArrayOfSkillIcon[skillSetIndex] , playerSkillSet.ArrayOfSkillName[skillSetIndex]);
                    castSkillParticle = playerSkillSet.ArrayOfSkillParticle[skillSetIndex];//파티클이지만 PlayerSkill.Skill에 포함됨 삭제 예정
                    StartCoroutine(SkillCast(currentSkill));

                    isCastSkill = true;
                    //스킬액션씬
                    //대미지 처리
                    //스킬 보너스 효과 적용
                }
            }
        }

        
    }
    IEnumerator SkillCast(PlayerSkill.Skill currentSkill) {
        yield return new WaitForSeconds(0.5f);


        currentSkill.PlaySkillSound(currentSkill.soundEventName, gameObject); //스킬 사운드
        ParticleSystem tmp =Instantiate(currentSkill.skillParticle, transform.position, Quaternion.identity);
        tmp.transform.parent = gameObject.transform;
        tmp.gameObject.transform.localScale = gameObject.transform.localScale;
        
        //애니메이션 클립 동적할당
        playerAnimator.Skill(currentSkill.skillClip);
        
        for (int i = 0; i < currentSkill.damage.Length; i++) { 
        yield return new WaitForSeconds(currentSkill.waitTimes[i]);
            battlePresenter.PlayerSkillToEnemy(new Damage(currentSkill.damage[i]*playerAttackStat, currentSkill.damageType[i]?new SlashDamage():new PierceDamage()));
        }

    }

    #region 스킬 팝업 UI
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
        
        if (Metronome.instance.CurrentTime >= Metronome.instance.SecondsPerBeat * 0.5 && Metronome.instance.CurrentTime <= Metronome.instance.SecondsPerBeat * 0.5 + Time.deltaTime) {//정말 더러운 조건...이지만 좋은 방법을 못찾음
            canAttack = true;
            return ;
        }
        
    }
    

    void AttackOnBeat() {  //Metronome.Onbeat에 구독할 예정
                           //queue카운트가 최소 1이상 있을 때
                           //Metronome의 OnBeat에서 dequeue하기
        if (inputBuffer.Count == 0) { AddCommand("R"); return; }//버퍼가 비어있다면 쉬표 추가
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
        
        //애니메이션
        //커맨드 입력
    }
    private void EnqueueAttackBuffer(string attackKey) {
        if (inputBuffer.Count < 2)
        {
            if (inputBuffer.Count == 1) Debug.Log("버퍼에 추가됨");
          
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
            {  //나중에 PlayerInput 클래스에서 가져온 변수로 쓸 예정

                
                EnqueueAttackBuffer("A");
                canAttack = false;
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                EnqueueAttackBuffer("B");
                canAttack = false;
            }
            CheckSkillCommand();
            if (isCastSkill) {
                
                Debug.Log("스킬 발동됨");
                
                AttackEnd();
                yield return new WaitForSeconds(3f);//스킬 애니메이션 클립의 길이만큼 대기 후 종료로 수정해야함. 04.01
                SkillUI();
                yield break;// 스킬이 발동되어 반복문 탈출 공격 종료
            }
            if (currentLimitBeat == 0) {
                
                Debug.Log("공격 시도 후 최대비트 초과");
                
                AttackEnd();
                yield return new WaitForSeconds((float)Metronome.instance.SecondsPerBeat);
                yield break;//제한시간 오버 공격종료
            }
            yield return null;
        }
    }
    
   

  
}
