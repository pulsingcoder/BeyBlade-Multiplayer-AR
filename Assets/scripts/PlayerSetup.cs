using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            // the player is local player
            transform.GetComponent<PlayerMovement>().enabled = true;
            transform.GetComponent<PlayerMovement>().joystick.gameObject.SetActive(true);

        }
        else
        {
            // the player is remote 
            transform.GetComponent<PlayerMovement>().enabled = false;
            transform.GetComponent<PlayerMovement>().joystick.gameObject.SetActive(false);
        }
        SetPlayerName();
    }

    // Update is called once per frame
    void SetPlayerName()
    {
        if (playerNameText != null)
        {
            if(photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }
            
        }
    }
}
