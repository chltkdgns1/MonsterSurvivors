using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Text;

public class GoogleCloudManager : MonoBehaviour
{
    static public GoogleCloudManager instance = null;

    int m_nRecvEnd;
    int m_nSendEnd;

    private string m_sSaveData;
    private string m_sRecvData;

    private byte[] m_btSaveData;
    private byte[] m_btRecvData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            m_nRecvEnd = DefineManager.EMPTY;
            m_nSendEnd = DefineManager.EMPTY;

            PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build());
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }
        else Destroy(gameObject);
    }

    public void SaveToCloud(string fileName)
    {
        if (GoogleManagers.instance.CheckLogin() == false) return; 
        string sGoogleId = Social.localUser.id;
        OpenSavedGame(sGoogleId + fileName, true);
    }

    void OpenSavedGame(string filename, bool bSave)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if (bSave)
            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToSave); //�����ƾ����
        else
            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToRead); //�ε���ƾ ����
    }



    //savedGameClient.OpenWithAutomaticConflictResolutionȣ��� �Ʒ� �Լ��� �ݹ����� �����߽��ϴ�. �غ�Ȱ�� �ڵ����� ȣ��ɰ̴ϴ�.

    void OnSavedGameOpenedToSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {         
        if (status == SavedGameRequestStatus.Success)
        {
            //ToastMessage.instance.CreateToastMessage("OnSavedGameOpenedToSave true", 2.0f);

            // handle reading or writing of saved game.
            //������ �غ�Ǿ����ϴ�. ���� ���� ������ �����մϴ�.
            //�����ҵ����͹���Ʈ�迭�� �����Ͻ� �������� ����Ʈ �迭�� �����մϴ�.

            byte[] btStoreData = Encoding.UTF8.GetBytes(m_sSaveData);         
            SaveGame(game, btStoreData, DateTime.Now.TimeOfDay);
        }

        else
        {
            //���Ͽ��⿡ ���� �߽��ϴ�. �����޽����� ����ϵ��� �մϴ�.
        }
    }

    void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);
        /*
        if (savedImage != null)
        {
            // This assumes that savedImage is an instance of Texture2D
            // and that you have already called a function equivalent to
            // getScreenshot() to set savedImage
            // NOTE: see sample definition of getScreenshot() method below
            byte[] pngData = savedImage.EncodeToPNG();
            builder = builder.WithUpdatedPngCoverImage(pngData);
        }*/

        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }


    void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            //������ ������ �Ϸ�Ǿ����ϴ�.
            m_nSendEnd = 1;
        }

        else
        {
            m_nSendEnd = 404;
            //������ ���忡 ���� �߽��ϴ�.
        }
    }


    //----------------------------------------------------------------------------------------------------------------

    //Ŭ����� ���� �����б�

    public void LoadFromCloud(string fileName)
    {
        if (GoogleManagers.instance.CheckLogin() == false) return;

        string sGoogleId = Social.localUser.id;
        OpenSavedGame(sGoogleId + fileName, false);
    }

    void OnSavedGameOpenedToRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            LoadGameData(game);
        }
        else
        {
            //���Ͽ��⿡ ���� �Ѱ��, �����޽����� ����ϴ��� �մϴ�.
        }

    }

    //������ �б⸦ �õ��մϴ�.

    void LoadGameData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }


    void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            m_sRecvData = Encoding.UTF8.GetString(data);
            m_nRecvEnd = 1;
        }
        else
        {
            m_nRecvEnd = 404;
        }
    }

    public int GetSendEndFlag()
    {
        return m_nSendEnd;
    }

    public int GetRecvEndFlag()
    {
        return m_nRecvEnd;
    }

    public void SetSendEndFlag(int value)
    {
        m_nSendEnd = value;
    }

    public void SetRecvEndFlag(int value)
    {
        m_nRecvEnd = value;
    }

    public string GetRecvData()
    {
        return m_sRecvData;
    }

    public void SetSendData(string value)
    {
        m_sSaveData = value;
    }
}
