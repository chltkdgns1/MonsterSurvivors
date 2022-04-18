using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ScoreInterface
{
    void ScoreChangeInteger(int type, int changeData);
    void ScoreChangeFloat(int type, float changeData);
}