using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListObjectValues : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI location;
    [SerializeField] private TextMeshProUGUI usernameRank;
    public bool isExpired { get; set; }
    public int selectedImageNum { get; set; }
    public string description;
    [SerializeField] private List<GameObject> images;
    string[] myCity = { "Stuttgart", "Esslingen", "Fellbach", "Fellbach", "Waiblingen", "Feuerbach", "Vahingen", "Leonberg", "Ludwigsburg" };

    [SerializeField] private GameObject advertisement;
    PlayerData playerData;
    public ulong creatorID { get; set; }
    public bool orderedToDestroy = false;

    void Start()
    {
        int rdm = Random.Range(0, 8);
        location.text = myCity[rdm];

        ImageActivator(images);
        Button thisButton = this.GetComponent<Button>();

        playerData = FindFirstObjectByType<PlayerData>();
        thisButton.onClick.AddListener(ActivateObject);

        // this is the last step or the sorting will fail
        StartUp();
    }

    public void UpdateTextObjects(string newTitle, string newDescription) {
        title.text = newTitle;
        description = newDescription;
    }

    void ImageActivator(List<GameObject> imageList) {
        imageList[selectedImageNum].SetActive(true);
        for (int i = 0; i < imageList.Count; i++) {
            var element = imageList[i];
            if (i != selectedImageNum) {
                element.SetActive(false);
                continue;
            }
        }
    }

    public void TransferAllVariables() {
        BestehendesAngebot bA = playerData.offerWindow.GetComponent<BestehendesAngebot>();
        bA.angebotLabels[0].text = title.text;
        bA.angebotLabels[1].text = description;
        bA.expired = isExpired;
        bA.selectedImageNum = selectedImageNum;
        bA.selectedOfferInList = this.gameObject;
        bA.FillOffer();
        bA.creatorID = creatorID;
    }

    public void ActivateObject() {
        playerData.offerWindow.SetActive(true);
        TransferAllVariables();
    }

    public void UpdateUsernameRankText(string nameValue, string rankValue) {
        usernameRank.text = nameValue + System.Environment.NewLine + rankValue;
    }

    GameObject panel;
    public NetworkObject netObj;
    void StartUp() {
        //Arranges all ListObjects in the panel (because NetObjects spawn at the top)
        panel = GameObject.FindWithTag("panel");

        //As last step destroying the Network Object to move it!
        netObj = gameObject.GetComponent<NetworkObject>();
        DisableNetObject();
        Invoke("DelayedMethod", 0.01f);
    }

    private void DelayedMethod() {
        //SortObjects.transform.parent = panel.transform;
        gameObject.transform.SetParent(panel.transform, false);
        gameObject.transform.SetSiblingIndex(0);
        Debug.Log("SortedList");
    }

    public void DisableNetObject() {
        if (netObj) {
            gameObject.transform.SetSiblingIndex(0);
            //netObj.enabled = !netObj.enabled;
            //Destroy(netObj);
        }
    }

    public void TriggerDestruction() {
        Destroy(gameObject, 0.01f);
    }
}
