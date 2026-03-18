using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class OnlineManager : NetworkBehaviour {
    [SerializeField] private GameObject theListPrefab;
    [SerializeField] private Transform panel;
    GameObject ServerPrefab;
    GameObject ClientPrefab;

    [SerializeField] private List<TextMeshProUGUI> textArray;
    [SerializeField] private PlayerData playerData;
    private bool isExpired = false;
    [SerializeField] private List<GameObject> imagesNewOffer;
    private int selectedImage;
    [SerializeField] private GameObject playerObject;
    ulong serverId = 0;
    [SerializeField] private NetworkManager networkManager;
    private string inviteCode;

    // Network Variables
    private NetworkVariable<int> selectedImageNet = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> textNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> descNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> nameNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> rankNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> expiredNet = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<ulong> clientIdNet = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void SpawnServerPrefab() {
        if (NetworkManager.Singleton.IsServer) {
            Debug.Log("AsServer");
            SpawnServerObjects(selectedImage, textArray[0].text, textArray[1].text, playerData.playerName, playerData.playerRank, isExpired, serverId);
        }
        else {
            Debug.Log("AsRPC");
            SpawnServerRpc(selectedImage, textArray[0].text, textArray[1].text, playerData.playerName, playerData.playerRank, isExpired);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnServerRpc(int value, string textValue, string DescValue, string nameValue, string rankValue, bool isExpired, ServerRpcParams rpcParams = default) {
        ulong clientId = rpcParams.Receive.SenderClientId;
        SpawnServerObjects(value, textValue, DescValue, nameValue, rankValue, isExpired, clientId);
    }

    private void SpawnServerObjects(int value, string textValue, string DescValue, string nameValue, string rankValue, bool isExpired, ulong clientId) {
        if (IsServer) {
            ServerPrefab = Instantiate(theListPrefab, panel);
            
            // Network values
            selectedImageNet.Value = value;
            textNet.Value = new FixedString128Bytes(textValue);
            descNet.Value = new FixedString128Bytes(DescValue);
            nameNet.Value = new FixedString128Bytes(nameValue);
            rankNet.Value = new FixedString128Bytes(rankValue);
            expiredNet.Value = isExpired;
            clientIdNet.Value = clientId;
            InserListValues(ServerPrefab, selectedImageNet.Value, textNet.Value.ToString(), descNet.Value.ToString(), nameNet.Value.ToString(), rankNet.Value.ToString(), expiredNet.Value, clientIdNet.Value);

            ServerPrefab.GetComponent<NetworkObject>().Spawn(true);
            SpawnClientRpc(selectedImageNet.Value, textNet.Value.ToString(), descNet.Value.ToString(), nameNet.Value.ToString(), rankNet.Value.ToString(), expiredNet.Value, clientIdNet.Value);
        }

    }

    [ClientRpc]
    private void SpawnClientRpc(int value, string textValue, string descValue, string nameValue, string rankValue, bool isExpired, ulong clientId) {
        ClientPrefab = Instantiate(theListPrefab, panel);
        // Transfered values
        InserListValues(ClientPrefab, value, textValue, descValue, nameValue, rankValue, isExpired, clientId);
        textNet.Value = new FixedString128Bytes(textValue);
        descNet.Value = new FixedString128Bytes(descValue);
        nameNet.Value = new FixedString128Bytes(nameValue);
        rankNet.Value = new FixedString128Bytes(rankValue);
        expiredNet.Value = isExpired;
    }

    private void InserListValues(GameObject serverPrefab, int value, string textValue, string descValue, string nameValue, string rankValue, bool isExpired, ulong clientId) {
        
        // Transfer of Text for Frontend
        ListObjectValues listObject = serverPrefab.GetComponent<ListObjectValues>();

        listObject.UpdateUsernameRankText(nameValue, rankValue);
        listObject.isExpired = isExpired;
        listObject.selectedImageNum = value;
        listObject.creatorID = clientId;

        listObject.UpdateTextObjects(textValue, descValue);
    }

    // XP
    public void AddXP(ulong receiverId) {
        if (NetworkManager.Singleton.IsServer) {
            Debug.Log("AsServer-XP");
            XPOnServer(receiverId);
        }
        else {
            Debug.Log("AsRPC-XP");
            XPServerRpc(receiverId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void XPServerRpc(ulong receiverId) {
        XPOnServer(receiverId);
    }

    private void XPOnServer(ulong receiverId) {
        if (IsServer) {
            var clientRpcParams = new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { receiverId } } };
            GiveXPClientRpc(clientRpcParams);
        }
    }

    [ClientRpc]
    private void GiveXPClientRpc(ClientRpcParams clientRpcParams = default) {
        playerData.GiveXP();
    }

    // Button functions
    private void Start() {
        if (playerObject != null) {
            playerData = playerObject.GetComponent<PlayerData>();
        }
    }

    public void SaveHeadline(TextMeshProUGUI text) {
        textArray[0] = text;
    }
    public void SaveDescription(TextMeshProUGUI textDescription) {
        textArray[1] = textDescription;
    }

    public void SaveMHD(bool expired) {
        isExpired = expired;
    }

    // Only for the 'create offer' window
    public void SelectImage(int imageNum) {
        selectedImage = imageNum;
        imagesNewOffer[selectedImage].SetActive(true);
        for (int i = 0; i < imagesNewOffer.Count; i++) {
            var element = imagesNewOffer[i];
            if (i != selectedImage) {
                element.SetActive(false);
                continue;
            }
        }
    }


}
