using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BestehendesAngebot : MonoBehaviour
{
    public bool MHDAbgelaufen;
    public int selectedImageNum;
    public GameObject ausgewähltesAngebotInListe;

    public List<GameObject> images2;

    public List<TextMeshProUGUI> angebotLabels;
    public List<GameObject> angebotGameobjects;
    public OnlineManager onlineManager;
    public PlayerData playerData;
    public ulong creatorID;

    public GameObject AcceptButton;


    public void FillAngebot() {
        if (MHDAbgelaufen == false) {
            angebotGameobjects[0].SetActive(true);
            angebotGameobjects[1].SetActive(false);
        }
        else {
            angebotGameobjects[0].SetActive(false);
            angebotGameobjects[1].SetActive(true);
        }
        imageActivator(images2);

        //Disable Accept Button if Publisher
        //if(creatorID == playerData.playerNetId) {
        //    AcceptButton.SetActive(false);
        //}
        //else {
        //    AcceptButton.SetActive(true);
        //}
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

    public void AcceptAngebot() {

        //Add XP to other User UserID
        ListObjectValues listValue = ausgewähltesAngebotInListe.GetComponent<ListObjectValues>();
        onlineManager.AddXP(listValue.creatorID);

        //remove from Internet
        //remove from List

        Destroy(ausgewähltesAngebotInListe);
    }


}
