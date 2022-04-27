using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;


public class GoogleAdsManager : MonoBehaviour
{
    public delegate void AdsEndCallBack(bool bSuccess, string[] param = null);
    static public GoogleAdsManager instance = null;

    private InterstitialAd m_InterstitialAd;
    private RewardedAd m_RewardedAd;

    private bool m_bIsTest = false;
    //private bool m_bCompleteFront = false;
    private bool m_bCompleteReward = false;

    private int m_nStartType;

    private bool m_bAdCheck = true;        // 광고가 거절 당한 것인지.
                                           //private AdRequest m_request;

    private AdsEndCallBack m_AdsEndFunc;
    private string[] m_param;
    //private CommunicationTypeDataClass m_value;

    //private List<AdsEnd> list = new List<AdsEnd>();


#if UNITY_ANDROID
    const string adUnitIdFront                  = "ca-app-pub-4598746676104401/5286860398";
    const string adUnitTestIdFront              = "ca-app-pub-3940256099942544/8691691433";

#elif UNITY_IPHONE
      
#else
       
#endif

    private void Awake()
    {
        m_nStartType = -1;
        if (instance == null)   instance = this;       
        else                    Destroy(gameObject);
    }

    private void Start()
    {
        m_bAdCheck = true;
        RequestFrontAd();
    }

    //public void Register(AdsEnd instance)
    //{
    //    list.Add(instance);
    //}

    //void OnAdsEndSuccess()
    //{
    //    int sz = list.Count;
    //    for(int i = 0;i< sz; i++)
    //    {
    //        list[i].OnAdsEndSuccess();
    //    }
    //}

    //void OnAdsEndFailed()
    //{
    //    int sz = list.Count;
    //    for (int i = 0; i < sz; i++)
    //    {
    //        list[i].OnAdsEndFailed();
    //    }
    //}

    private void RequestFrontAd()
    {
        m_InterstitialAd = new InterstitialAd(m_bIsTest ? adUnitTestIdFront : adUnitIdFront);
        AdRequest request = new AdRequest.Builder().Build();

        m_InterstitialAd.OnAdClosed += (sender, e) =>
        {
            m_AdsEndFunc(true, m_param);
        };

        m_InterstitialAd.OnAdFailedToShow += (sender, e) =>
        {
            m_AdsEndFunc(false);
        };

        m_InterstitialAd.OnAdFailedToLoad += (sender, e) =>
        {
            //Debug.Log("광고를 받지 못함 : " + e.LoadAdError.ToString());
            m_bAdCheck = true;
        };
        m_InterstitialAd.OnAdLoaded += (sender, e) =>
        {
            //Debug.Log("광고를 성공적으로 로드함");
            m_bAdCheck = false;
        };


        m_InterstitialAd.LoadAd(request);
    }

    void GiveReward()
    {
        if(m_nStartType == DefineManager.POTION_AD)
        {
            //Debug.Log("들어옴?");          
        }
    }

    public void FrontShow(AdsEndCallBack AdsEndFunc, string [] param = null)
    {
        m_AdsEndFunc = AdsEndFunc;
        m_param = param;

        if (m_bAdCheck == false)
            m_InterstitialAd.Show();

        else {
            m_bAdCheck = false;
            m_AdsEndFunc(false);
        }
        RequestFrontAd();
    }

    //public bool GetCompleteFront()
    //{
    //    return m_bCompleteFront;
    //}

    public bool GetCompleteReward()
    {
        return m_bCompleteReward;
    }      

    public void SetStartType(int value)
    {
        m_nStartType = value;
    }
}

