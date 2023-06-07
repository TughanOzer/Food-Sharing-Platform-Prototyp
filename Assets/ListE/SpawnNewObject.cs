using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SpawnNewObject : MonoBehaviour
{
    public GameObject GameObject;
    public Transform panel;
    public List<TextMeshProUGUI> textArray;
    PlayerData playerData;
    public bool MHDAbgelaufen = false;
    public List<GameObject> imagesNeuesAngebot;
    public int selectedImage;

    public GameObject playerObject;
    public GameObject OnlineManager;
    OnlineManager onlinemanager;

    public void OnClickSpawnAsChild() {

        onlinemanager = OnlineManager.GetComponent<OnlineManager>();
        //TransferAllToOnlineManager();
        onlinemanager.SpawnServerPrefab();
    }


    public void TransferAllToOnlineManager() {

    }


    //Wird über publish button aufgerufen
    public void ResetAllEntries() {
        //onlinemanager.textArray = null;
        //onlinemanager.MHDAbgelaufen = false;
        //onlinemanager.selectedImage = 3;
    }
}
