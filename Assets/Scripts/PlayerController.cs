using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 3f;

    private Camera mainCamera;

    private Vector3 mouseInput = Vector3.zero;

    private void Initiialize()
    {
        mainCamera = Camera.main;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initiialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            speed = (speed == 0f) ? 50f : 0f;
        }

        if (!Application.isFocused) return;

        Vector2 mousePosition = (Vector2)Input.mousePosition;
        mouseInput.x = Input.mousePosition.x;
        mouseInput.y = Input.mousePosition.y;
        
        Vector3 mouseWorldCoordinates = mainCamera.ScreenToWorldPoint((Vector3)mousePosition);
        transform.position = Vector3.MoveTowards(current: transform.position, target:mouseWorldCoordinates, 
                maxDistanceDelta:Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (mouseWorldCoordinates != transform.position)
        {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0f;
            transform.up = targetDirection;
        }
    }

    [ServerRpc]
    private void DetermineCollisionWinnerServerRPC()
    { 
    
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballrb = collision.gameObject.GetComponent<Rigidbody2D>();
            ballrb.AddForce(new Vector2(transform.position.x, transform.position.y));
        }


        Debug.Log("Player Collision");
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!IsOwner) return;
    }


    struct PlayerData : INetworkSerializable
    {
        public ulong Id;
        public ushort Length;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
            serializer.SerializeValue(ref Length);
        }
    }
}