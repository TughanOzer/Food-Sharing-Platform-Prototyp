using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public TextMeshProUGUI playerNameUI;
    public TextMeshProUGUI playerLevelUI;
    public TextMeshProUGUI playerRankUI;
    public string playerName;
    public int playerLevel;
    public string playerRank;
    public GameObject offerWindow;
    public int experience = 0;
    public ulong playerNetId;

    public TextMeshProUGUI targetOneUi;
    public TextMeshProUGUI targetTwoUi;

    //Erungenschaften
    public int amountShared = 0;
    public int amountTaken = 0;
    public Slider sliderGive;
    public Slider sliderTake;

    //Badges
    public int amountSharedBadges = 0;
    public int amountTakenBadges = 0;
    public Image imageShared1;
    public Image imageShared2;
    public Image imageShared3;
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
        playerRank = "Frischer Sharer";
        playerRankUI.text = "Rang " + playerLevel + ": " + playerRank;
    }

    bool finishedAnimation = true;
    public void GiveXP() {
        experience += 100;
        playerLevelUI.text = experience.ToString() + " / " + "200";
        if (experience > 199) {
            experience = 0;
            playerLevelUI.text = experience.ToString() + " / " + "200";
            playerLevel += 1;
            if (playerLevel == 2) playerRank = "Zuverlässiger Sharer";
            else if (playerLevel == 3) playerRank = "Helfender Sharer";
            else if (playerLevel == 4) playerRank = "Aktiver Sharer";
            else if (playerLevel == 5) playerRank = "Erfahrener Sharer";
            else if (playerLevel == 6) playerRank = "Sharing Experte";
            else if (playerLevel == 7) playerRank = "Foodsharing Mentor";
            else if (playerLevel == 8) playerRank = "Großzügiger Spender";
            else if (playerLevel == 9) playerRank = "Lebensmittelretter";
            else if (playerLevel == 10) playerRank = "Sharing Held";
            else if (playerLevel == 11) playerRank = "Sharing Meister";
            else if (playerLevel == 12) playerRank = "Sharing Legende";
            else if (playerLevel == 13) playerRank = "Sharing Guru";
            else if (playerLevel == 14) playerRank = "Sharing Visionär";
            else if (playerLevel == 15) playerRank = "Sharing Pionier";
            else if (playerLevel == 16) playerRank = "Sharing Genie";
            else if (playerLevel == 17) playerRank = "Sharing Magier";
            else if (playerLevel == 18) playerRank = "Sharing König";
            else if (playerLevel == 19) playerRank = "Sharing Imperator";
            else if (playerLevel >= 20) playerRank = "Sharing Gott";
            playerRankUI.text = "Rang " + playerLevel + ": " + playerRank;
        }

        finishedAnimation = false;
    }

    public void OfferTaken() {
        amountTaken += 1;
        if (amountTaken >= 3) {
            amountTaken = 3;
        }
        targetTwoUi.text = amountTaken + " / " + "3";
        sliderTake.value = amountTaken;

        amountTakenBadges += 1;
        if (amountTakenBadges >= 1) UnlockBadge(imageTaken1);
        if (amountTakenBadges >= 2) UnlockBadge(imageTaken2);
        if (amountTakenBadges >= 10) UnlockBadge(imageTaken3);
    }

    public void OfferShared() {
        amountShared += 1;
        if (amountShared >= 3) {
            amountShared = 3;
        }
        targetOneUi.text = amountShared + " / " + "3";
        sliderGive.value = amountShared;
        //Badges
        amountSharedBadges += 1;
        if(amountSharedBadges >= 1) UnlockBadge(imageShared1);
        if(amountSharedBadges >= 5) UnlockBadge(imageShared2);
        if (amountSharedBadges >= 20) UnlockBadge(imageShared3);
    }

    void UnlockBadge(Image image) {
        image.color = Color.white;
    }

    public float speed = 0.5f;
    private void FixedUpdate() {

            sliderXP.value = Mathf.Lerp(sliderXP.value, experience, speed * Time.deltaTime);

    }

}
