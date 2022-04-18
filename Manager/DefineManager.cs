using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineManager
{
    public const int COLLISION_STATE_USE_ERASE = 1;
    public const int COLLISION_STATE_NOT_USE_ERASE = 0;

    public const int FIRST = 0;


    public const int INFINITE = (int)1e9;

    public const int TOUCH_SKILL_INI_COMPLETE = 5;

    public const bool LEFT = false;
    public const bool RIGHT = true;
    public const bool UP = true;
    public const bool DOWN = false;

    public const int NLEFT = 0;
    public const int NRIGHT = 1;
    public const int NUP = 0;
    public const int NDOWN = 1;

    public const bool D3 = false;    // 3D
    public const bool D2 = true;    // 2D

    public const int LEFT_BUY_POTION = 0;
    public const int RIGHT_BUY_POTION = 1;

    public const int SCREEN_HOME_STATUS = 0;
    public const int SCREEN_STORE_STATUS = 1;
    public const int SCREEN_SETTINGS_STATUS = 2;
    public const int SCREEN_NONE_STATUS = -1;

    public const int SCROLL_VERTICLE = 0;
    public const int SCROLL_HORIZON = 1;

    public const int SMALL = 0;
    public const int BIG = 1;

    public const bool OUT_POSITION = true;
    public const bool INIT_POSITION = false;
    public const bool IN_POSITION = false;

    public const int INSIDE_POSITION = 0;
    public const int MIN_OUT_POSITION = 1;
    public const int MAX_OUT_POSITION = 2;
    public const int TOP_OUT_POSITION = 1;
    public const int BOTTOM_OUT_POSITION = 2;


    public const int GOOGLE_LOGIN_TIME_OUT = 3;
    public const int GOOGLE_LOGIN_DEFAULT = 0;
    public const int GOOGLE_LOGIN_SUCCESS = 1;
    public const int GOOGLE_LOGIN_FAIL = 2;

    public const int POINTER_CLICK = 1;
    public const int POINTER_DOWN = 2;
    public const int POINTER_UP = 4;

    public const int DIA = 0;
    public const int RUBY = 1;

    public const int CLICKED_TYPE_DIA = 1;
    public const int CLICKED_TYPE_RUBY = 2;

    public const int EMPTY = 0;
    public const int NOT_EMPTY = 1;

    public const bool SUCCESS = true;
    public const bool FAILED = false;

    public const int DELETE_AD = 1;
    public const int NOT_DELETE_AD = 0;

    public const int INIT_AD = 0;    // 아무 광고 타입 ( 예를 들어 포션 얻을 때의 광고) 등록이 되지 않은 상태
    public const int POTION_AD = 1;
    public const int GAME_END_AD = 2;
    public const int GAME_OUT_AD = 3;

    public const int TEAR_LOCK = 0;
    public const int TEAR_CRISTAL = 1;
    public const int TEAR_PLATINUM = 2;
    public const int TEAR_GRAND_MASTER = 3;
    public const int TEAR_CHALLENGER = 4;

    public const int TEAR_LOCK_LIMIT = 1100;
    public const int TEAR_CRISTAL_LIMIT = 1500;
    public const int TEAR_PLATINUM_LIMIT = 2000;
    public const int TEAR_GRAND_MASTER_LIMIT = 2500;
    public const int TEAR_CHALLENGER_LIMIT = 100000;

    public const int PURCHASE_TYPE_DIA_80 = 1;
    public const int PURCHASE_TYPE_DIA_500 = 2;
    public const int PURCHASE_TYPE_DIA_2500 = 3;
    public const int PURCHASE_TYPE_RUBY_8000 = 4;
    public const int PURCHASE_TYPE_RUBY_50000 = 5;
    public const int PURCHASE_TYPE_RUBY_250000 = 6;
    public const int PURCHASE_TYPE_ADDELETE = 7;
    public const int PURCHASE_TYPE_DIA_PICK_1 = 8;
    public const int PURCHASE_TYPE_DIA_PICK_2 = 9;
    public const int PURCHASE_TYPE_DIA_PICK_3 = 10;
    public const int PURCHASE_TYPE_RUBY_PICK_1 = 11;
    public const int PURCHASE_TYPE_RUBY_PICK_2 = 12;
    public const int PURCHASE_TYPE_RUBY_PICK_3 = 13;


    public const int PURCHASE_TYPE_DIA_80_PRICE = 1000;
    public const int PURCHASE_TYPE_DIA_500_PRICE = 6000;
    public const int PURCHASE_TYPE_DIA_2500_PRICE = 23000;
    public const int PURCHASE_TYPE_RUBY_8000_PRICE = 1000;
    public const int PURCHASE_TYPE_RUBY_50000_PRICE = 6000;
    public const int PURCHASE_TYPE_RUBY_250000_PRICE = 23000;
    public const int PURCHASE_TYPE_ADDELETE_PRICE = 3600;
    public const int PURCHASE_TYPE_DIA_PICK_1_PRICE = 1000;
    public const int PURCHASE_TYPE_DIA_PICK_2_PRICE = 6000;
    public const int PURCHASE_TYPE_DIA_PICK_3_PRICE = 23000;
    public const int PURCHASE_TYPE_RUBY_PICK_1_PRICE = 1000;
    public const int PURCHASE_TYPE_RUBY_PICK_2_PRICE = 6000;
    public const int PURCHASE_TYPE_RUBY_PICK_3_PRICE = 23000;


    public const int PLAYING_MOD = 1;
    public const int HOME_MOD = 2;

    public const int PLAYING_STATE_NOMAL = 0;
    public const int PLAYING_STATE_PAUSE = 1;
    public const int PLAYING_STATE_NO_ENEMY = 2;

    public const int PLAYING_MIN_CONTINUE_COUNT = 0;
    public const int PLAYING_MAX_CONTINUE_COUNT = 1;

    public const int GAME_FIGHT_MODE = 0;
    public const int GAME_AVOID_MODE = 1;
    public const int GAME_SHOOT_MODE = 2;

    public const int ITEM_DIA = 1;
    public const int ITEM_RUBY = 2;
    public const int ITEM_POTION = 3;
    public const int ITEM_SKILL = 4;
    public const int ITEM_AMOR = 5;
    public const int ITEM_WEAPON = 6;
    public const int ITEM_ANYTHING = 7;

    public const int RETURN_ERROR = -1;
    public const int RETURN_NORMAL = 0;

    public const int ADS_CALLBACK_POTION = 0;

    public const int INIT = -1;

    public const string SKILL_PATH = "Skill/SkillTouch/";
    public const string SKILL_SAME_NAME = "icon_skill ";

    public static readonly float[] SKILL_COOL_TIME =
        {5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f,
        5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f, 5f};


    public enum SKILL{
        SKILL_SIZE_PERCENT          ,
        SKILL_COOLTIME_PERCENT      ,
        SKILL_DAMAGE                ,
        SKILL_DAMAGE_PERCENT        ,
        SKILL_COUNT                 ,
        SKILL_NUCKBACK_PERCENT      ,
        SKILL_BLOOD_PERCENT         ,
        SKILL_ALL_SIZE_PERCENT      ,
        SKILL_ALL_COOLTIME_PERCENT  ,
        SKILL_ALL_DAMAGE            ,
        SKILL_ALL_DAMAGE_PERCENT    ,
        SKILL_ALL_DIRECTION         ,
        SKILL_ALL_BLOOD_PERCENT     ,
        SKILL_ALL_NUCK_BACK_PERCENT ,
        SKILL_RANDOM                ,
        SKILL_REINFORCE
    }

    public static int ACTIVE_SKILL_SIZE         = 5;
    public static int ACTIVE_SKILL_TYPE_SIZE    = 7;
    public static int ALL_SKILL_TYPE_SIZE       = 14;

    public static int JEWEL_1                   = 1;
    public static int JEWEL_2                   = 2;
    public static int JEWEL_3                   = 3;
    public static int JEWEL_4                   = 4;
    public static int BOX_1                     = 5;
    public static int BOX_2                     = 6;
    public static int BOX_3                     = 7;
    public static int BOX_4                     = 8;
    public static int BOX_5                     = 9;
}


