using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using TMPro;

public class TestRelay : MonoBehaviour
{
    public TextMeshProUGUI beitrittsCodeUI;
    public TextMeshProUGUI feldBeitrittsCodeJoinUI;
    public string beitrittscodeEingabe;

    private async void Start() {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    //Call to Create:::
    public async void CreateRelay() {
        try {
           Allocation allocation = await RelayService.Instance.CreateAllocationAsync(20);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            beitrittsCodeUI.text = "Beitrittscode: " + joinCode;
            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartServer();

        } catch (RelayServiceException e) {
            Debug.Log(e.ToString());
        }
    }

    //Call to Join
    private async void JoinRelay(string joinCode) {
        try {
            Debug.Log("Joining relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException ex) { Debug.Log(ex.ToString()); }

    }



    public void JoinRelayButton() {
        beitrittscodeEingabe = feldBeitrittsCodeJoinUI.text;
        beitrittscodeEingabe = beitrittscodeEingabe.Substring(0, beitrittscodeEingabe.Length - 1);
        JoinRelay(beitrittscodeEingabe);
    }

}
