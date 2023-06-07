using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public TextMeshProUGUI playerNameUI;
    public TextMeshProUGUI playerLevelUI;
    public TextMeshProUGUI playerRangUI;
    public string playerName;
    public int playerLevel;
    public string playerRang;
    public GameObject angebotsFenster;
    public int experience = 0;
    public ulong playerNetId;

    public TextMeshProUGUI zielEinsUI;
    public TextMeshProUGUI zielZweiUI;

    //Erungenschaften
    public int anzahlGeteilt = 0;
    public int anzahlAngenommen = 0;
    public Slider sliderGive;
    public Slider sliderTake;

    //Badges
    public int anzahlGeteiltBages = 0;
    public int anzahlAngenommenBages = 0;
    public Image imageTeilen1;
    public Image imageTeilen2;
    public Image imageTeilen3;
    public Image imageTaken1;
    public Image imageTaken2;
    public Image imageTaken3;

    //Xp Bar
    public Slider sliderXP;

    public void SetPlayerName(TextMeshProUGUI newPlayerName) {
        playerName = newPlayerName.text;
        playerNameUI.text = playerName;
    }

    private void Start() {
        playerName = "NoName";
        playerNameUI.text = playerName;
        experience = 0;
        playerLevelUI.text = experience.ToString() + " / " + "200";
        playerLevel = 1;
        playerRang = "Frischer Sharer";
        playerRangUI.text = "Rang " + playerLevel + ": " + playerRang;
    }

    bool finishedAnimation = true;
    public void GiveXP() {
        experience += 100;
        playerLevelUI.text = experience.ToString() + " / " + "200";
        if (experience > 199) {
            experience = 0;
            playerLevelUI.text = experience.ToString() + " / " + "200";
            playerLevel += 1;
            if (playerLevel == 2) playerRang = "Zuverlässiger Sharer";
            else if (playerLevel == 3) playerRang = "Helfender Sharer";
            else if (playerLevel == 4) playerRang = "Aktiver Sharer";
            else if (playerLevel == 5) playerRang = "Erfahrener Sharer";
            else if (playerLevel == 6) playerRang = "Sharing Experte";
            else if (playerLevel == 7) playerRang = "Foodsharing Mentor";
            else if (playerLevel == 8) playerRang = "Großzügiger Spender";
            else if (playerLevel == 9) playerRang = "Lebensmittelretter";
            else if (playerLevel == 10) playerRang = "Sharing Held";
            else if (playerLevel == 11) playerRang = "Sharing Meister";
            else if (playerLevel == 12) playerRang = "Sharing Legende";
            else if (playerLevel == 13) playerRang = "Sharing Guru";
            else if (playerLevel == 14) playerRang = "Sharing Visionär";
            else if (playerLevel == 15) playerRang = "Sharing Pionier";
            else if (playerLevel == 16) playerRang = "Sharing Genie";
            else if (playerLevel == 17) playerRang = "Sharing Magier";
            else if (playerLevel == 18) playerRang = "Sharing König";
            else if (playerLevel == 19) playerRang = "Sharing Imperator";
            else if (playerLevel >= 20) playerRang = "Sharing Gott";
            playerRangUI.text = "Rang " + playerLevel + ": " + playerRang;
        }

        finishedAnimation = false;
    }

    public void Angenommen() {
        anzahlAngenommen += 1;
        if (anzahlAngenommen >= 3) {
            anzahlAngenommen = 3;
        }
        zielZweiUI.text = anzahlAngenommen + " / " + "3";
        sliderTake.value = anzahlAngenommen;

        anzahlAngenommenBages += 1;
        if (anzahlAngenommenBages >= 1) unlockBadge(imageTaken1);
        if (anzahlAngenommenBages >= 2) unlockBadge(imageTaken2);
        if (anzahlAngenommenBages >= 10) unlockBadge(imageTaken3);
    }

    public void Geteilt() {
        anzahlGeteilt += 1;
        if (anzahlGeteilt >= 3) {
            anzahlGeteilt = 3;
        }
        zielEinsUI.text = anzahlGeteilt + " / " + "3";
        sliderGive.value = anzahlGeteilt;
        //Badges
        anzahlGeteiltBages += 1;
        if(anzahlGeteiltBages >= 1) unlockBadge(imageTeilen1);
        if(anzahlGeteiltBages >= 5) unlockBadge(imageTeilen2);
        if (anzahlGeteiltBages >= 20) unlockBadge(imageTeilen3);
    }

    void unlockBadge(Image image) {
        image.color = Color.white;
    }

    public float speed = 0.5f;
    private void FixedUpdate() {

            sliderXP.value = Mathf.Lerp(sliderXP.value, experience, speed * Time.deltaTime);

    }

}
