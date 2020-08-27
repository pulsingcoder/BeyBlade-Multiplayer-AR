using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Photon CallBack Methods
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            Debug.Log("HI");
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerBeybladeARGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                int randomSpawnPoints = Random.Range(0, spawnPositions.Length - 1);
                Vector3 instantiatePosition = spawnPositions[randomSpawnPoints].position;

                PhotonNetwork.Instantiate(playerPrefabs[(int) playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
                Debug.Log("player selection number is " + (int)playerSelectionNumber);
            }
                
        }
    }
    #endregion
}
