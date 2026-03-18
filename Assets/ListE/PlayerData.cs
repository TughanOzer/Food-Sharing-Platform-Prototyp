using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class PlayerData : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI playerNameUI;
    [SerializeField] private TextMeshProUGUI playerLevelUI;
    [SerializeField] private TextMeshProUGUI playerRankUI;
    [SerializeField] private TextMeshProUGUI targetOneUi;
    [SerializeField] private TextMeshProUGUI targetTwoUi;
    [SerializeField] private Slider sliderGive;
    [SerializeField] private Slider sliderTake;
    [SerializeField] private Slider sliderXP;

    [Header("Badge Images")]
    [SerializeField] private Image[] sharedBadges;
    [SerializeField] private Image[] takenBadges;

    [Header("Configuration")]
    [SerializeField] private float xpSliderSpeed = 5f;
    private readonly int maxShares = 3;
    private readonly int xpToLevelUp = 200;
    private readonly int xpPerAction = 100;

    public GameObject offerWindow;

    public string playerName { get; private set; }
    public string playerRank { get; private set; }
    public int playerLevel { get; private set; }
    public ulong playerNetId { get; set; }


    private int experience = 0;
    private int amountShared = 0;
    private int amountTaken = 0;
    private int amountSharedBadges = 0;
    private int amountTakenBadges = 0;

    private readonly string[] rankNames = {
        "Frischer Sharer", "Zuverlässiger Sharer", "Helfender Sharer", "Aktiver Sharer", "Erfahrener Sharer", "Sharing Experte", "Foodsharing Mentor", "Großzügiger Spender",
        "Lebensmittelretter", "Sharing Held", "Sharing Meister", "Sharing Legende", "Sharing Guru", "Sharing Visionär", "Sharing Pionier", "Sharing Genie", "Sharing Magier", 
        "Sharing König", "Sharing Imperator", "Sharing Gott"
    };

    private void Start() {
        SetPlayerName("NoName");
        UpdateLevelUI();
        UpdateRankUI();
    }

    private void Update() {

        if (Mathf.Abs(sliderXP.value - experience) > 0.1f) {
            sliderXP.value = Mathf.Lerp(sliderXP.value, experience, xpSliderSpeed * Time.deltaTime);
        }

    }

    public void SetPlayerName(string newName) {
        playerName = newName;
        playerNameUI.text = playerName;
    }

    public void GiveXP() {
        experience += xpPerAction;

        if (experience >= xpToLevelUp) {
            experience -= xpToLevelUp;
            playerLevel++;
            UpdateRankUI();
        }

        UpdateLevelUI();
    }

    public void OfferTaken() {
        amountTaken = Mathf.Clamp(amountTaken + 1, 0, maxShares);
        targetTwoUi.text = $"{amountTaken} / {maxShares}";
        sliderTake.value = amountTaken;

        amountTakenBadges++;
        CheckTakenBadges();
    }

    public void OfferShared() {
        amountShared = Mathf.Clamp(amountShared + 1, 0, maxShares);
        targetOneUi.text = $"{amountShared} / {maxShares}";
        sliderGive.value = amountShared;

        amountSharedBadges++;
        CheckSharedBadges();
    }




    private void UpdateLevelUI() {
        playerLevelUI.text = $"{experience} / {xpToLevelUp}";
    }

    private void UpdateRankUI() {
        int rankIndex = Mathf.Clamp(playerLevel - 1, 0, rankNames.Length - 1);
        playerRankUI.text = $"Rang {playerLevel}: {rankNames[rankIndex]}";
    }

    private void CheckSharedBadges() {
        if (amountSharedBadges >= 1) UnlockBadge(sharedBadges[0]);
        if (amountSharedBadges >= 5) UnlockBadge(sharedBadges[1]);
        if (amountSharedBadges >= 20) UnlockBadge(sharedBadges[2]);
    }

    private void CheckTakenBadges() {
        if (amountTakenBadges >= 1) UnlockBadge(takenBadges[0]);
        if (amountTakenBadges >= 2) UnlockBadge(takenBadges[1]);
        if (amountTakenBadges >= 10) UnlockBadge(takenBadges[2]);
    }

    private void UnlockBadge(Image image) {
        if (image != null) {
            image.color = Color.white;
        }
    }

}
