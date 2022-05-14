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
    // �巡�״� first Point �� ���ؼ��� �Է� ����.
    void OnFirstTouch(Vector3 touchPoint);
    void OnFirstTouchDrag(Vector3 touchPoint);
    void OnOtherTouch(List<TouchCircle> touchPoint);
    void OnFirstTouchEnd();
}

public interface ITouchGameEvent
{
    void OnTargetPosition(Vector3 position);

    void OnStop();

    void OnSkill(List<TouchCircle> skills);
}