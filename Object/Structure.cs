using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour, ITouchCraftManager // 모든 구조물 데이터는 StructureData 를 상속받음.
{
    [SerializeField]
    protected float m_fShield;
    [SerializeField]
    protected float m_fHp;
    [SerializeField]
    protected int m_nPrice;
    [SerializeField]
    protected bool m_bGameType = false;   // UI 용으로 쓸 것인지, 인게임용으로 쓸 것인지

    [SerializeField]
    protected int m_nGroupType              = 0;

    protected float m_fReinforceHp          = 0f;
    protected float m_fReinforceShield      = 0f;
    protected float m_fReinforcePrice       = 0f;

    public void SetShield(float fShield) { m_fShield = fShield; }
    public float GetShield() { return m_fShield; }
    public void SetHp(float fHp) { m_fHp = fHp; }
    public float GetHp() { return m_fHp; }
    public float GetPrice() { return m_nPrice; }
    public void SetPrice(int nPrice) { m_nPrice = nPrice; }
    public void SetGroupType(int nGroupType) { m_nGroupType = nGroupType; }
    public int GetGroupType() { return m_nGroupType; }

    public void OnOneDrag(Vector3 touchPoint)
    {
        transform.position = touchPoint;
    }
    public void OnManyDrag(List<Vector3> touchPoint)
    {
        Vector3 maxYPosition = touchPoint[0];
        Vector3 minYPosition = touchPoint[1];
        if (touchPoint[0].y < touchPoint[1].y)
        {
            Vector3 temp = maxYPosition;
            maxYPosition = minYPosition;
            minYPosition = temp;
        }

        Vector2 vDir = maxYPosition - minYPosition;
        transform.Rotate(new Vector3(0, 0, Module.GetAngle(vDir, new Vector3(1, 0))));
    }

    public void RegistTouchEvnet()
    {
        TouchManager.instance.RegisterEvent(this);
    }

    public void DeleteTouchEvent()
    {
        TouchManager.instance.DeleteEvent(this);
    }
}
