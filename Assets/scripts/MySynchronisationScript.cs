using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using System;

public class MySynchronisationScript : MonoBehaviour, IPunObservable
{
    Rigidbody rb;
    PhotonView photonView;
    Vector3 networkPosition;
    Quaternion networkRotation;
    public bool synchronizedVelocity = true;
    public bool synchronizedAngularVelocity = true;
    private float distance;
    private float angle;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceGreaterThan = 1.0f;
    private GameObject battleArenaGameObject;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        networkPosition = new Vector3();
        networkRotation = new Quaternion();

        battleArenaGameObject = GameObject.Find("BattleArena");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkPosition, distance * (1.0f / PhotonNetwork.SerializationRate));
            // sometime change is very quick and small that why we multiply with 100
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }

        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            // then I am the one who is the owner and I'll send data to 
            // other players about my position, velocity and data to other player
            stream.SendNext(rb.position - battleArenaGameObject.transform.position);
            stream.SendNext(rb.rotation);

            if (synchronizedVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if (synchronizedAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        else
        {

            networkPosition = (Vector3) stream.ReceiveNext() + battleArenaGameObject.transform.position;
            networkRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(rb.position, networkPosition) > teleportIfDistanceGreaterThan)
                {
                    rb.position = networkPosition;
                }
            }
            if (synchronizedAngularVelocity || synchronizedVelocity)
            {
                float lag = Math.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                
                if (synchronizedVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();
                    networkPosition += rb.velocity * lag;
                    distance = Vector3.Distance(rb.position, networkPosition);
                }

                if (synchronizedAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();
                    networkRotation = Quaternion.Euler((rb.angularVelocity * lag)) * networkRotation;
                    angle = Quaternion.Angle(rb.rotation, networkRotation);
                }
            }
        }

    

    }
}
