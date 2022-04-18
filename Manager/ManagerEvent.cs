using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ManagerEvent 
{
    void OnMouseMoveEvent(Vector3 mousePoint, Vector3 mouseConvertPosition);
    void OnMouseUpEvent(Vector3 mousePoint, Vector3 mouseConvertPosition);
    void OnMouseDonwEvent(Vector3 mousePoint, Vector3 mouseConvertPosition);
    // Start is called before the first frame update
}

public interface TouchManagerEvent
{
    // 드래그는 first Point 에 한해서만 입력 받음.
    void OnFirstTouch(Vector3 touchPoint);
    void OnFirstTouchDrag(Vector3 touchPoint);
    void OnOtherTouch(List<TouchCircle> touchPoint);
    void OnFirstTouchEnd();
}

public interface TouchGameEvent
{
    void OnTargetPosition(Vector3 position);

    void OnStop();

    void OnSkill(List<TouchCircle> skills);
}