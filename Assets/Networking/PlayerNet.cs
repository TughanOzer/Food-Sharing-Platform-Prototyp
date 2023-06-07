using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNet : NetworkBehaviour
{

    [SerializeField] float moveSpeed = 3f;
    [SerializeField] private Transform bulletPrefab;


    private void Update() {
        if(!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;


        //

        if (Input.GetKey(KeyCode.F)) {

            if (NetworkManager.Singleton.IsServer) {
                SpawnPrefab();
            }
            else {
                SpawnPrefabServerRpc();
            }
        }
    }

    void SpawnPrefab() {
        Transform bulletTransform = Instantiate(bulletPrefab);
        bulletTransform.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnPrefabServerRpc(ServerRpcParams rpcParams = default) {
        SpawnPrefab();
    }

    //



    private NetworkVariable<InventoryData> playerInventory = new NetworkVariable<InventoryData>(new InventoryData {
        ammunition = 60,
        shooting = false,
    }, 
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public struct InventoryData : INetworkSerializable {
        public int ammunition;
        public bool shooting;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref ammunition);
            serializer.SerializeValue(ref shooting);
        }
    }
}
