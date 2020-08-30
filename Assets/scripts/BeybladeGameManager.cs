using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class BeybladeGameManager : MonoBehaviourPunCallbacks
{

    [Header("UI")]
    public GameObject ui_InfoPanelGameObejct;
    public TextMeshProUGUI ui_InfoText;
    public GameObject SearchForGameButtonObject;
    public GameObject adjustButton;
    public GameObject rayCastCenter_Image;
    // Start is called before the first frame update
    void Start()
    {
        ui_InfoPanelGameObejct.SetActive(true);
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    #region UI Callbacks Method 
    public void JoinRandomRoom()
    {
        ui_InfoText.text = "searching for availabe rooms...";
        PhotonNetwork.JoinRandomRoom();
        SearchForGameButtonObject.SetActive(false);
    }


    public void OnClickQuitMatchButton()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
    }
    #endregion


    #region Photon Callbacks Method 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        ui_InfoText.text = message;
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        adjustButton.SetActive(false);
        rayCastCenter_Image.SetActive(false);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            ui_InfoText.text = "joined to " + PhotonNetwork.CurrentRoom.Name + " waiting for other players to join......";
        }
        else
        {
            ui_InfoText.text = "joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(ui_InfoPanelGameObejct, 2.0f));
        }
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        ui_InfoText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount;
        StartCoroutine(DeactivateAfterSeconds(ui_InfoPanelGameObejct, 2.0f));
    }

    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion


    #region Private Methods
    void CreateAndJoinRoom()
    {
        string roomName = "room " + Random.Range(0, 10000);
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName, roomOption);
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        _gameObject.SetActive(false);
    }
    #endregion
}
