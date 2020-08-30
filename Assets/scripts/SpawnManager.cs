using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class SpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;
    public GameObject playerBattleArena;
    public enum RaiseEventCodes
    {
        PlayerSpawnEventCode = 0
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    #region Photon CallBack Methods

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 receivedPosition = (Vector3)data[0];
            Quaternion receivedRotation = (Quaternion)data[1];
            int receivedPlayerSelectionData = (int)data[3];

            GameObject player = Instantiate(playerPrefabs[receivedPlayerSelectionData], receivedPosition + playerBattleArena.transform.position, receivedRotation);
            PhotonView _photonView = player.GetComponent<PhotonView>();
            _photonView.ViewID = (int)data[2];
        }
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            //Debug.Log("HI");
            //object playerSelectionNumber;
            //if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerBeybladeARGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            //{
            //    int randomSpawnPoints = Random.Range(0, spawnPositions.Length - 1);
            //    Vector3 instantiatePosition = spawnPositions[randomSpawnPoints].position;

            //    PhotonNetwork.Instantiate(playerPrefabs[(int) playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
            //    Debug.Log("player selection number is " + (int)playerSelectionNumber);
            //}
            SpawnPlayer();
                
        }
    }
    #endregion


    #region Private Methods
    private void SpawnPlayer()
    {
        object playerSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerBeybladeARGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
        {
            int randomSpawnPoints = Random.Range(0, spawnPositions.Length - 1);
            Vector3 instantiatePosition = spawnPositions[randomSpawnPoints].position;

            GameObject playerGameObject = Instantiate(playerPrefabs[(int)playerSelectionNumber], instantiatePosition, Quaternion.identity);
            PhotonView _photonView = playerGameObject.GetComponent<PhotonView>();
            if (PhotonNetwork.AllocateViewID(_photonView))
            {
                object[] data = new object[]
                {
                    playerGameObject.transform.position - playerBattleArena.transform.position, playerGameObject.transform.rotation, _photonView.ViewID, playerSelectionNumber
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };
                // Raise events
                PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEventCode, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.Log("Failed to allocate a view ID");
                Destroy(playerGameObject);
            }
            Debug.Log("player selection number is " + (int)playerSelectionNumber);
        }
    }

    #endregion



}
