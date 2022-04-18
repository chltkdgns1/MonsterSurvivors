using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineMananger
{
    public const int GAME_REACT_STATE_WAIT      = 0;        // 게임 시작 전
    public const int GAME_REACT_STATE_READY     = 1;        // 게임 시작 후 빨간색 대기 상태
    public const int GAME_REACT_STATE_START     = 2;        // 게임 시작 후 초록색 시작 상태
    public const int GAME_REACT_STATE_ERROR     = 3;        // 게임 시작 후 에러 상태
    public const int GAME_REACT_STATE_END       = 4;        // 게임 종료
}
