using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class OnlineManager : NetworkBehaviour {


    public GameObject theListPrefab;
    public Transform panel;
    GameObject ServerPrefab;
    GameObject ClientPrefab;

    public List<TextMeshProUGUI> textArray;
    public PlayerData playerData;
    public bool MHDAbgelaufen = false;
    public List<GameObject> imagesNeuesAngebot;
    public int selectedImage;
    public GameObject playerObject;
    ulong serverId = 0;
    public NetworkManager networkManager;
    public string Beitrittscode;

    //Network Variables
    private NetworkVariable<int> selectedImageNet = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> textNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> DescNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> nameNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<FixedString128Bytes> rankNet = new NetworkVariable<FixedString128Bytes>("null", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> mHDAbgelaufenNet = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<ulong> clientIdNet = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void SpawnServerPrefab() {
        if (NetworkManager.Singleton.IsServer) {
            Debug.Log("AsServer");
            SpawnServerObjects(selectedImage, textArray[0].text, textArray[1].text, playerData.playerName, playerData.playerRang, MHDAbgelaufen, serverId);
        }
        else {
            Debug.Log("AsRPC");
            SpawnServerRpc(selectedImage, textArray[0].text, textArray[1].text, playerData.playerName, playerData.playerRang, MHDAbgelaufen);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnServerRpc(int value, string textValue, string DescValue, string nameValue, string rankValue, bool mhdAbgelaufenValue, ServerRpcParams rpcParams = default) {
        ulong clientId = rpcParams.Receive.SenderClientId;
        SpawnServerObjects(value, textValue, DescValue, nameValue, rankValue, mhdAbgelaufenValue, clientId);
    }

    private void SpawnServerObjects(int value, string textValue, string DescValue, string nameValue, string rankValue, bool mhdAbgelaufenValue, ulong clientId) {
        if (IsServer) {
            ServerPrefab = Instantiate(theListPrefab, panel);
            //
            //Hier immer die ...Net Values
            InserListValues(ServerPrefab, selectedImageNet.Value, textNet.Value.ToString(), DescNet.Value.ToString(), nameNet.Value.ToString(), rankNet.Value.ToString(), mHDAbgelaufenNet.Value, clientIdNet.Value);
            selectedImageNet.Value = value;
            textNet.Value = new FixedString128Bytes(textValue);
            DescNet.Value = new FixedString128Bytes(DescValue);
            nameNet.Value = new FixedString128Bytes(nameValue);
            rankNet.Value = new FixedString128Bytes(rankValue);
            mHDAbgelaufenNet.Value = mhdAbgelaufenValue;
            clientIdNet.Value = clientId;
            //
            ServerPrefab.GetComponent<NetworkObject>().Spawn(true);
            SpawnClientRpc(selectedImageNet.Value, textNet.Value.ToString(), DescNet.Value.ToString(), nameNet.Value.ToString(), rankNet.Value.ToString(), mHDAbgelaufenNet.Value, clientIdNet.Value);
        }

    }

    [ClientRpc]
    private void SpawnClientRpc(int value, string textValue, string descValue, string nameValue, string rankValue, bool mhdAbgelaufenValue, ulong clientId) {
        ClientPrefab = Instantiate(theListPrefab, panel);
        //
        //Hier immer die übergebenen Values
        InserListValues(ClientPrefab, value, textValue, descValue, nameValue, rankValue, mhdAbgelaufenValue, clientId);
        textNet.Value = new FixedString128Bytes(textValue);
        DescNet.Value = new FixedString128Bytes(descValue);
        nameNet.Value = new FixedString128Bytes(nameValue);
        rankNet.Value = new FixedString128Bytes(rankValue);
        mHDAbgelaufenNet.Value = mhdAbgelaufenValue;
        //
    }

    TextMeshProUGUI textUGUI;
    TextMeshProUGUI textUGUI2;
    private void InserListValues(GameObject serverPrefab, int value, string textValue, string descValue, string nameValue, string rankValue, bool mhdAbgelaufenValue, ulong clientId) {
        //Transfer von Text zur Anzeige
        ListObjectValues listObject = serverPrefab.GetComponent<ListObjectValues>();
        listObject.UsernameRank.text = nameValue + System.Environment.NewLine + rankValue;

        listObject.MHDAbgelaufen = mhdAbgelaufenValue;
        listObject.selectedImageNum = value;
        textUGUI = new GameObject().AddComponent<TextMeshProUGUI>();
        textUGUI.SetText(textValue);
        textUGUI2 = new GameObject().AddComponent<TextMeshProUGUI>();
        textUGUI2.SetText(descValue);
        listObject.description = textUGUI2.text;
        listObject.UpdateTextObjects(textUGUI, textUGUI2);
        listObject.creatorID = clientId;
    }




    //XP
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










    // Button Functions
    private void FixedUpdate() {
        playerData = playerObject.GetComponent<PlayerData>();
    }

    public void SaveHeadline(TextMeshProUGUI text) {
        textArray[0] = text;
    }
    public void SaveDescription(TextMeshProUGUI textDescription) {
        textArray[1] = textDescription;
    }

    public void SaveMHD(bool abgelaufen) {
        MHDAbgelaufen = abgelaufen;
    }

    //Nur AngebotsErstellungsfenster
    public void SelectImage(int imageNum) {
        selectedImage = imageNum;
        imagesNeuesAngebot[selectedImage].SetActive(true);
        for (int i = 0; i < imagesNeuesAngebot.Count; i++) {
            var element = imagesNeuesAngebot[i];
            if (i != selectedImage) {
                element.SetActive(false);
                continue;
            }
        }
    }


}
