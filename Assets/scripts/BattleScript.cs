using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;


public class BattleScript : MonoBehaviourPun
{
    public Spinner spinnerScript;
    private float startSpinSpeed;
    private float currentSpinSpeed;
    public float commonDamageCoefficient = 0.04f;
    public bool isAttacker;
    public bool isDefender;
    private Rigidbody rb;
    private bool isDead = false;
    [Header("UI")]
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text;
    public GameObject uI_3DGameObject;
    public GameObject deathPanelUIPrefab;
    private GameObject deathPanelUIGameObject;
    [Header("Player Type Damage Coeeficient")]
    public float do_Damage_Coefficient_Attacker = 10f;  // Do more damage // Advantage
    public float get_Damage_Coeeficient_Attacker = 1.2f;  // get more damage // Disadvantage
    public float do_Damage_Coefficient_Defender = 0.75f;  // do less damage // Disadvantage
    public float get_Damage_Coeeficient_Defender = 0.2f; // get less damage // Advantage


    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;
        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isDefender = true;
            isAttacker = false;
            // Making defender more health
            spinnerScript.spinSpeed = 4400;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;
            spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            Debug.Log("My speed " + mySpeed + "othrer player speed " + otherPlayerSpeed);
            float default_Damage_Amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600f * commonDamageCoefficient;
            if (isAttacker)
            {
                default_Damage_Amount *= do_Damage_Coefficient_Attacker;
            }
            else if (isDefender)
            {
                default_Damage_Amount *= do_Damage_Coefficient_Defender;
            }

            if (mySpeed > otherPlayerSpeed)
            {
                Debug.Log("You damage the ohter player");
                if (collision.collider.GetComponent<PhotonView>().IsMine)
                {
                    // apply damage
                    // it will broadcast message to all players in the room
                  
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered,default_Damage_Amount);
                }
              
            }
          
        }
    }

    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if (!isDead)
        {
            if (isAttacker)
            {
                _damageAmount *= get_Damage_Coeeficient_Attacker;
                if (_damageAmount > 1000f)
                {
                    _damageAmount = 400f;
                }
            }
            else if (isDefender)
            {
                _damageAmount *= get_Damage_Coeeficient_Defender;
            }
            spinnerScript.spinSpeed -= _damageAmount;
            currentSpinSpeed = spinnerScript.spinSpeed;
            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
            spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

            if (currentSpinSpeed < 100f)
            {
                // Die
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        spinnerScript.spinSpeed = 0;
        uI_3DGameObject.SetActive(false);
        if (photonView.IsMine)
        {
            // coutdown start
            StartCoroutine(ReSpawn());
        }

    }

    IEnumerator ReSpawn()
    {
        GameObject canvasGameObject = GameObject.Find("Canvas");   
        if (deathPanelUIGameObject == null)
        {
            deathPanelUIGameObject = Instantiate(deathPanelUIPrefab, canvasGameObject.transform);
        }
        else
        {
            deathPanelUIGameObject.SetActive(true);
        }
        Text reSpawnText = deathPanelUIGameObject.transform.Find("RespawnTimeText").GetComponent<Text>();
        float respawnTime = 8.0f;
        reSpawnText.text = respawnTime.ToString(".00");
        while(respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1f);
            respawnTime -= 1;
            reSpawnText.text = respawnTime.ToString(".00");
            GetComponent<PlayerMovement>().enabled = false;
        }

        deathPanelUIGameObject.SetActive(false);
        GetComponent<PlayerMovement>().enabled = true;

        photonView.RPC("ReBorn", RpcTarget.AllBuffered);

    }


    [PunRPC] 
    public void ReBorn()
    {
        spinnerScript.spinSpeed = startSpinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;
        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;

        rb.freezeRotation = true;
        rb.rotation = Quaternion.Euler(Vector3.zero);
        uI_3DGameObject.SetActive(true);
        isDead = false;
    }
}
