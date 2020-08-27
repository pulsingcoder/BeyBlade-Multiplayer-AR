using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

using Photon.Realtime;

public class LoobyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerInputField;
    public GameObject ui_LoginGameObject;

    [Header("Lobby UI")]
    public GameObject ui_LobbyGameObject;
    public GameObject ui_3DGameObject;

    [Header("Connection stats UI")]
    public GameObject ui_ConnectionStatsGameObject;
    public Text connectionStatstext;
    private bool showConnectionStatus = false;

    #region UnityMethods

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            ui_3DGameObject.SetActive(true);
            ui_ConnectionStatsGameObject.SetActive(false);
            ui_LobbyGameObject.SetActive(true);
            ui_LoginGameObject.SetActive(false);
        }
        else
        {
            ui_3DGameObject.SetActive(false);
            ui_ConnectionStatsGameObject.SetActive(false);
            ui_LobbyGameObject.SetActive(false);
            ui_LoginGameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (showConnectionStatus)
        {
            connectionStatstext.text = "Connection Status:  " + PhotonNetwork.NetworkClientState;
        }
        
    }

    #endregion


    #region UiCallBackMethods
    public void OnEnterGameButtonClick()
    {
        


        string playerName = playerInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            ui_3DGameObject.SetActive(false);
            ui_LobbyGameObject.SetActive(false);
            ui_LoginGameObject.SetActive(false);

            showConnectionStatus = true;
            ui_ConnectionStatsGameObject.SetActive(true);


            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid");
        }
    }


    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }
    #endregion

    #region Photon Callbacks

    public override void OnConnected()
    {
        Debug.Log("we are connected to internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to photon server ");

        ui_ConnectionStatsGameObject.SetActive(false);
       
        ui_LoginGameObject.SetActive(false);

        ui_3DGameObject.SetActive(true);
        ui_LobbyGameObject.SetActive(true);
    }


    #endregion
}
