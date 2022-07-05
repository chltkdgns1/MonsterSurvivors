using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManagerEvent 
{
    void OnMouseMoveEvent(Vector3 mousePoint, Vector3 mouseConvertPosition);
    void OnMouseUpEvent(Vector3 mousePoint, Vector3 mouseConvertPosition);
    void OnMouseDonwEvent(Vector3 mousePoint, Vector3 mouseConvertPosition);
    // Start is called before the first frame update
}

public interface ITouchManagerEvent
{
    // 드래그는 first Point 에 한해서만 입력 받음.
    void OnFirstTouch(Vector3 touchPoint);
    void OnFirstTouchDrag(Vector3 touchPoint);
    void OnFirstTouchEnd();
}

public interface ITouchGameEvent
{
    void OnTargetPosition(Vector3 position);

    void OnStop();

    void OnSkill(List<TouchCircle> skills);
}

public interface IDoubleTouch
{
    bool IsInsideRange(Vector3 vFirst, Vector3 vSecond);
}

public interface ITouchCraftManager : IDoubleTouch
{
    void OnOneTouch(Vector3 touchPoint);
    void OnDoubleTouch(Vector3 doubleTouchPoint);
    void OnDrag(Vector3 firstTouch, Vector3 touchPoint);
    void OnZoom(float fWheel);
    //void OnManyDrag(List<Vector3> touchPoint);
    void RegistTouchEvnet();
    void DeleteTouchEvent();
}