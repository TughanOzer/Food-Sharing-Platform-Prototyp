using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;


    private void Awake() {
        serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            Debug.Log("Joined as Server");
        });
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Joined as Host");
        });
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Joined as Client");
        });
    }

    bool onlyOnce = true;
    public GameObject ServerButton;
    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.Period) && onlyOnce){
            onlyOnce = false;
            ServerButton.SetActive(true);
        }
    }
}
