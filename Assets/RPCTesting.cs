using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RPCTesting : NetworkBehaviour {

    // NetworkVariable to store the value sent by the client
    private NetworkVariable<int> clientValue = new NetworkVariable<int>();

    // Method called by the client to send a value to the server
    public void SendValueToServer(int value) {
        // Call the ServerRpc method and pass the value as a parameter
        SendValueToServerRpc(value);
    }

    // ServerRpc method called by the client to send a value to the server
    [ServerRpc(RequireOwnership = false)]
    private void SendValueToServerRpc(int value) {
        // Set the value of the NetworkVariable on the server
        clientValue.Value = value;

        // Output the received value using Debug.Log
        Debug.Log("Server received value: " + clientValue.Value);

        // Call the ClientRpc method to send the value to all clients
        SendValueToClientRpc(clientValue.Value);
    }

    // ClientRpc method called by the server to send a value to all clients
    [ClientRpc]
    private void SendValueToClientRpc(int value) {
        // Output the received value using Debug.Log
        Debug.Log("Client received value: " + value);
    }

}
