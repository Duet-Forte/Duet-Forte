/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID CHARACTER_EMOTION_ANGRY_SFX = 179926370U;
        static const AkUniqueID CHARACTER_EMOTION_DUMBFOUNDED_SFX = 1920166314U;
        static const AkUniqueID CHARACTER_EMOTION_QUESTIONMARK_SFX = 3095903108U;
        static const AkUniqueID COMBAT_BGM_STOP = 2307662469U;
        static const AkUniqueID COMBAT_STAGE_01_BGM = 890880275U;
        static const AkUniqueID COMBAT_TEST01 = 207956517U;
        static const AkUniqueID ENEMY_ELECGUITAR_ATTACK_SIGNAL_SFX = 3211681449U;
        static const AkUniqueID ENEMY_TAMBOURINE_ATTACK_SFX = 2328612313U;
        static const AkUniqueID ENEMY_TAMBOURINE_ATTACK_SIGNAL_SFX = 909052356U;
        static const AkUniqueID ENEMY_TIMMY_ATTACK_SFX = 837159709U;
        static const AkUniqueID MAINMENU_CLICK_GAMESTART_SFX = 162289589U;
        static const AkUniqueID MAINMENU_CLICK_SFX = 1154772126U;
        static const AkUniqueID MAINMENU_HOVER_SFX = 294506560U;
        static const AkUniqueID NONCOMBAT_BGM = 918533349U;
        static const AkUniqueID NONCOMBAT_BGM_STOP = 2869548328U;
        static const AkUniqueID OB_DOOR_SFX = 4145632761U;
        static const AkUniqueID OB_FIRE_FIRE_AMB = 3989126589U;
        static const AkUniqueID OB_FIRE_IGNITION_SFX = 198066997U;
        static const AkUniqueID OB_FIRE_SWITCHON_SFX = 3661802867U;
        static const AkUniqueID PLAYER_ATTACK_A_SFX = 3452252061U;
        static const AkUniqueID PLAYER_ATTACK_B_SFX = 1532296734U;
        static const AkUniqueID PLAYER_ATTACK_STRONGSIGNAL_SFX = 1248207521U;
        static const AkUniqueID PLAYER_BODYFALL_SFX = 173655414U;
        static const AkUniqueID PLAYER_PARRY_SFX = 3410875789U;
        static const AkUniqueID PLAYER_SKILL_AAA_SFX = 2518233790U;
        static const AkUniqueID PLAYER_SKILL_ABA_SFX = 2846259367U;
        static const AkUniqueID PLAYER_SKILL_BBB_SFX = 3250415283U;
        static const AkUniqueID PLAYER_SKILL_CLIMAX_SFX = 127602315U;
        static const AkUniqueID PLAYER_STEP_SFX = 4038898239U;
        static const AkUniqueID TRANSITION_FADEIN = 544226120U;
        static const AkUniqueID TRANSITION_FADEOUT = 1873933263U;
        static const AkUniqueID UI_GETSKILL_SFX = 1260125929U;
        static const AkUniqueID UI_TYPING_SFX = 1024104135U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace CAMPFIRE
        {
            static const AkUniqueID GROUP = 1931646578U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID TIMMY_HOUSE = 2206092402U;
                static const AkUniqueID TOWN = 3091570009U;
            } // namespace STATE
        } // namespace CAMPFIRE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace NONCOMBATBGM
        {
            static const AkUniqueID GROUP = 927944968U;

            namespace SWITCH
            {
                static const AkUniqueID TIMMYHOUSE = 268061649U;
                static const AkUniqueID TOWN = 3091570009U;
            } // namespace SWITCH
        } // namespace NONCOMBATBGM

        namespace STAGE_01
        {
            static const AkUniqueID GROUP = 344719509U;

            namespace SWITCH
            {
                static const AkUniqueID STAGE_01_END = 1119828183U;
                static const AkUniqueID STAGE_01_START = 3995731396U;
            } // namespace SWITCH
        } // namespace STAGE_01

        namespace STATE
        {
            static const AkUniqueID GROUP = 1382476432U;

            namespace SWITCH
            {
                static const AkUniqueID DIRT = 2195636714U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace STATE

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID SFX_SIDECHAIN = 2862064063U;
        static const AkUniqueID VOLUME = 2415836739U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAINMENU = 3604647259U;
        static const AkUniqueID NEW_SOUNDBANK = 4072029455U;
        static const AkUniqueID NONCOMBATBANK = 1020116044U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID COMBAT_BGM = 3004711510U;
        static const AkUniqueID MAINMENU_SFX = 193649807U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID PLAYER_SFX = 817096458U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
