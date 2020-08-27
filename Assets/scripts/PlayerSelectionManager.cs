using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitchTransform;
    public int playerSelectionNumber = 0;
    public GameObject[] beybladeModels;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
    public GameObject ui_Selection;
    public GameObject ui_afterSelection;

    #region Unity Methods


    // Start is called before the first frame update
    void Start()
    {
        ui_Selection.SetActive(true);
        ui_afterSelection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region UI Callback Method
    public void NextPlayer()
    {
        playerSelectionNumber += 1;
        if (playerSelectionNumber >= beybladeModels.Length)
        {
            playerSelectionNumber = 0;
        }
        
        StartCoroutine(Rotate(Vector3.up, playerSwitchTransform, 90, 1f)); 
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "Attacker";
        }
        else
        {
            playerModelType_Text.text = "Defender";
        }
    }


    public void PreviousPlayer()
    {
        playerSelectionNumber -= 1;
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = beybladeModels.Length - 1;
        }
        StartCoroutine(Rotate(Vector3.up, playerSwitchTransform, -90, 1f));
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            playerModelType_Text.text = "Attacker";
        }
        else
        {
            playerModelType_Text.text = "Defender";
        }
    }


    public void OnSelectButtonClicked()
    {
        ui_Selection.SetActive(false);
        ui_afterSelection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelectionProb = new ExitGames.Client.Photon.Hashtable { { MultiplayerBeybladeARGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber
            } };
        PhotonNetwork.SetPlayerCustomProperties(playerSelectionProb);

    }


    public void OnReselectButtonClicked()
    {
        ui_Selection.SetActive(true);
        ui_afterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion



    #region private Methods 

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

    }

    #endregion
}
