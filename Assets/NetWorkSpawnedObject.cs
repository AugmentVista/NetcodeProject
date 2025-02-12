using Unity.Netcode;
using UnityEngine;

public class NetWorkSpawnedObject : NetworkBehaviour
{
    [SerializeField] private GameObject BallPrefab;

    private void Spawn()
    {
        Instantiate(BallPrefab, new Vector3(3f, 3f, 0f), Quaternion.identity);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn(); // Call base method (good practice)
        Spawn();
    }
}

