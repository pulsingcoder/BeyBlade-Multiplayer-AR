using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    ARPlacementManager m_ARPlacementManager;
    ARPlaneManager m_ARPlaneManager;

    [Header("UI")]
    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject searchForGameButton;
    public GameObject scaleSlider;
    public TextMeshProUGUI informUIPanelText;
    private void Awake()
    {
        m_ARPlacementManager = GetComponent<ARPlacementManager>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);
        scaleSlider.SetActive(true);
        informUIPanelText.text = "Move phone to detect plane and place battle arena";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableARPlacementAndPlaneDetection()
    {
        m_ARPlaneManager.enabled = false;
        m_ARPlacementManager.enabled = false;
        SetAllPlaneActiveOrDeactive(false);
        placeButton.SetActive(false);
        adjustButton.SetActive(true);
        searchForGameButton.SetActive(true);
        scaleSlider.SetActive(false);
        informUIPanelText.text = "Great! You have placed the battle arena. Now, search for game to BATTLE!";

    }

    public void EnableARPlacementAndPlaneDetection()
    {
        m_ARPlaneManager.enabled = true;
        m_ARPlacementManager.enabled = true;
        SetAllPlaneActiveOrDeactive(true);
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);
        scaleSlider.SetActive(true);
        informUIPanelText.text = "Move phone to detect plane and place battle arena";
    }

    void SetAllPlaneActiveOrDeactive(bool value)
    {
        foreach (var plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }
    }
}
