using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCheckManager : MonoBehaviour
{
    private List<Vector3> m_PlayerPointsList = new List<Vector3>();
    private int[] m_dirX = { 0, 1, -1, 0, 0, 1, 1, -1, -1 };
    private int[] m_dirY = { 0, 0, 0, 1, -1, 1, -1, 1, -1 };

    private void OnEnable()
    {
        InitEnabe();
    }

    private void Start()
    {
        InitStart();
    }

    void InitEnabe()
    {
        InitPlayerPoints();
        CheckCollisionMonster();
    }

    void InitStart()
    {

    }

    void InitPlayerPoints()
    {
        m_PlayerPointsList.Clear();
        PolygonCollider2D polygonComp = PlayerOffline2D.instance.GetComponent<PolygonCollider2D>();

        if (polygonComp == null) return;

        Vector2[] temp = polygonComp.points;
        for (int i = 0; i < temp.Length; i++) m_PlayerPointsList.Add(temp[i]);
        m_PlayerPointsList.Add(PlayerOffline2D.instance.transform.position);
    }

    List<Solid2D> GetSolids(Vector3 vPosition)
    {
        List<Solid2D> list = new List<Solid2D>();
        vPosition = Module.GetGrid(vPosition); 

        for (int i = 0; i < m_dirX.Length; i++) // 9 방위에 대하여 이동시킴.
        {
            Solid2D solidTemp = new Solid2D(new Vector3(vPosition.x + m_dirX[i], vPosition.y + m_dirY[i], vPosition.z));
            solidTemp.SetEdgePoint(0.5f);
            solidTemp.SortSeq();
            list.Add(solidTemp);
        }
        return list;
    }

    bool CheckCollision(List<Solid2D> list, Vector3 vPosition)
    {
        for(int i = 0; i < list.Count; i++)
        {
            bool flag = list[i].IsInside(vPosition);
            if (flag) return flag;
        }
        return false;
    }

    void CheckCollisionMonster()
    {
        List<float> tempList = MonsterManager.instance.GetMonsterCollisionRadList();
        List<GameObject> tempMonsterList = MonsterManager.instance.GetMonsterObject();

        // tempList 와 tempMonsterList 의 사이즈는 같음.

        int sz = tempList.Count;
        for (int i = 0; i < sz; i++)
        {
            Vector3 pos = tempMonsterList[i].transform.position;
            List<Solid2D> solidList = GetSolids(pos);

            int solSz = solidList.Count;
            for(int k = 0; k < solSz; k++)
            {
                bool flag = CollisionReneual2D.IsCollisionSolid(solidList[k], pos, tempList[i]);
                // pos 를 기준으로 하여 8방위와 가운데 사각형이 tempList[i] 반지름 내에 충돌하는지 판단.
                if (flag)
                {
                    Vector3? vCenterPoint = solidList[k].GetCenterPoint();
                    if (vCenterPoint == null) continue;
                    long lKey = Module.GetHashFunc(vCenterPoint.Value);
                    CraftManager.instance.SetTemporaryEle(lKey);
                    // 충돌이 난 블록은 충돌 처리함.
                }
            }
        }
    }

    private void Update()
    {
        
    }

    //[SerializeField]
    //private GameObject[] m_ObCheckerPlayer; // 플레이어 체크
    //private SampleStructure m_SampleStructure = null;

    //private bool m_bEnable = false;

    //private int[] m_dirX = {0, 1, -1, 0, 0, 1, 1, -1, -1 };
    //private int[] m_dirY = {0, 0, 0, 1, -1, 1, -1, 1, -1 };

    //private float m_fDiff;

    //void Start()
    //{
    //    StartInit();
    //}

    //private void OnEnable()
    //{
    //    m_bEnable = true;
    //}

    //private void OnDisable()
    //{
    //    m_bEnable = false;
    //}

    //void StartInit()
    //{
    //    m_fDiff = Mathf.Sqrt(0.5f * 0.5f + 0.5f * 0.5f);



    //    m_SampleStructure = m_ObCheckerPlayer.GetComponent<SampleStructure>();
    //    m_SampleStructure.CallBack = CheckPosCallBack;
    //}

    //IEnumerator CheckCharPos()
    //{
    //    Vector3 pPos = PlayerOffline2D.instance.gameObject.transform.position;
    //    Vector3 transPos = Module.GetGrid(pPos);

    //    m_ObCheckerPlayer.SetActive(true);
    //    m_ObCheckerPlayer.transform.position = transPos;

    //    if (CheckSamePosition(transPos, pPos, m_fDiff))
    //    {
    //        Debug.Log("여기들옴?");
    //        CheckPosCallBack();
    //        // visit 처리
    //    }

    //    for (int i = 0; i < 9; i++)
    //    {
    //        Vector3 nextPos = new Vector3(transPos.x + (float)m_dirX[i], transPos.y + (float)m_dirY[i], transPos.z);
    //        m_ObCheckerPlayer.transform.position = nextPos;
    //        //Debug.Log("Start Pos : " + nextPos.x + " " + nextPos.y);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    m_ObCheckerPlayer.SetActive(false);
    //}

    //bool CheckSamePosition(Vector3 vFirst, Vector3 vSecond, float fDiff) // 같은 위치에 있는데 콜라이더에 안걸리는 경우가 있음.
    //{
    //    float fDistance = Vector2.Distance(vFirst, vSecond);
    //    return fDistance <= fDiff;
    //}

    //void CheckPosCallBack()
    //{
    //    Vector3 position = m_ObCheckerPlayer.transform.position;
    //    Module.GetGrid(position);
    //    Debug.Log("position : " + position.x + " " + position.y);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (m_bEnable)
    //    {
    //        StartCoroutine(CheckCharPos());
    //        m_bEnable = false;
    //    }
    //}
}
