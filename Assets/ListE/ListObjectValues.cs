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
    public TextMeshProUGUI title;
    public TextMeshProUGUI standort;
    public TextMeshProUGUI UsernameRank;
    public bool MHDAbgelaufen;
    public int selectedImageNum;
    public string description;
    public List<GameObject> images;
    string[] myCity = { "Stuttgart", "Esslingen", "Fellbach", "Fellbach", "Waiblingen", "Feuerbach", "Vahingen", "Leonberg", "Ludwigsburg" }; 

    public GameObject angebotsAnzeige;
    PlayerData playerData;
    public ulong creatorID;
    public bool orderedToDestroy = false;

    void Start()
    {
        int rdm = Random.Range(0, 8);
        standort.text = myCity[rdm];

        imageActivator(images);
        Button thisButton = this.GetComponent<Button>();

        playerData = FindFirstObjectByType<PlayerData>();
        thisButton.onClick.AddListener(activateObject);


        //this last
        StartUp();
    }


    public void UpdateTextObjects(TextMeshProUGUI titleNew, TextMeshProUGUI descriptionNew) {
        title.text = titleNew.text;
        description = descriptionNew.text;
    }

    void imageActivator(List<GameObject> imageList) {
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
        BestehendesAngebot bA = playerData.angebotsFenster.GetComponent<BestehendesAngebot>();
        bA.angebotLabels[0].text = title.text;
        bA.angebotLabels[1].text = description;
        bA.MHDAbgelaufen = MHDAbgelaufen;
        bA.selectedImageNum = selectedImageNum;
        bA.ausgewähltesAngebotInListe = this.gameObject;
        bA.FillAngebot();
        bA.creatorID = creatorID;
    }

    public void activateObject() {
        playerData.angebotsFenster.SetActive(true);
        TransferAllVariables();
    }


    GameObject panel;
    public NetworkObject netObj;
    void StartUp() {
        //Sortiert alle ListObject in das Panel ein (weil Netobject ganz oben spawnen)
        panel = GameObject.FindWithTag("panel");

        //Letzter Schritt! Zerstörung des Network Objects um zu verschieben!
        netObj = gameObject.GetComponent<NetworkObject>();
        disableNetObject();
        Invoke("DelayedMethod", 0.01f);
    }


    private void DelayedMethod() {
        //SortObjects.transform.parent = panel.transform;
        gameObject.transform.SetParent(panel.transform, false);
        gameObject.transform.SetSiblingIndex(0);
        Debug.Log("SortedList");
    }

    public void disableNetObject() {
        if (netObj) {
            //netObj.enabled = !netObj.enabled;
            Destroy(netObj);
        }
    }


    private void FixedUpdate() {
        if (orderedToDestroy == true) Invoke("DestroyThisObject", 0.01f);
    }

    private void DestroyThisObject() {
        Destroy(gameObject);
    }
}
