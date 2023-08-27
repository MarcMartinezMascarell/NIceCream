using System.Collections;
using Inventory;
using Inventory.Inventory_Scripts;
using SaveLoadSystem;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UniqueId))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadious = 1f;
    public InventoryItemData ItemData;

    private Collider2D myCollider;
    private Rigidbody2D myRigidbody;
    private float originalY;

    [Header("Throw Settings")]
    [SerializeField] private float _colliderEnableDelay = 0.5f;
    [SerializeField] private float _throwGravity = 1f;
    [SerializeField] private float _minThrowXForce = 2f;
    [SerializeField] private float _maxThrowXForce = 4f;
    [SerializeField] private float _throwYForce = 2f;

    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;

    private float _enableAnimationTime = 2.5f;

    private void Awake()
    {
        id = GetComponent<UniqueId>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData,  transform.position, transform.rotation);

        myCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider.isTrigger = true;
        myCollider.enabled = false;

    }

    private void Start()
    {
        id = GetComponent<UniqueId>().ID;
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
        StartCoroutine(EnableCollider(_colliderEnableDelay));
    }

    private void Update()
    {
        _enableAnimationTime -= Time.deltaTime;
        if (_enableAnimationTime > 0f) return;
        transform.position = new Vector3(transform.position.x, originalY + Mathf.Sin(Time.time * 2.5f) * 0.04f, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (inventory)
        {
            if (inventory.AddToInventory(ItemData, 1))
            {
                if(ItemData.PickUpSoundEffect != null)
                    AudioSource.PlayClipAtPoint(ItemData.PickUpSoundEffect, transform.position);
                SaveGameManager.data.collectedItems.Add(id);
                Destroy(this.gameObject);
            } ;
            return;
        }

        //Check if the other object is the same item, if so, add to stack
        // var otherItem = other.GetComponent<ItemPickUp>();
        // Debug.Log("Checking if other item is the same");
        // if (!otherItem) return;
        // if (otherItem.ItemData != ItemData) return;
        // if (otherItem.Amount <= 0) return;
        // if (otherItem.Amount + Amount > ItemData.MaxStackSize) return;
        // Debug.Log("Stacking items");
        // Amount += otherItem.Amount;
        // Destroy(otherItem.gameObject);
        // Instantiate(this.ItemData.ItemPrefab, transform.position, transform.rotation);
    }

    private void LoadGame(SaveData data)
    {
        if (data.collectedItems.Contains(id))
        {
            Destroy(this.gameObject);
        }
    }

    public void Throw(float xDir)
    {
        myRigidbody.gravityScale = _throwGravity;
        var throwXForce = Random.Range(_minThrowXForce, _maxThrowXForce);
        myRigidbody.velocity = new Vector2(Mathf.Sign(xDir) * throwXForce, _throwYForce);
        StartCoroutine(DisableGravity(_throwYForce));
    }

    private IEnumerator DisableGravity(float atYVelocity)
    {
        yield return new WaitUntil(() => myRigidbody.velocity.y < -atYVelocity);
        myRigidbody.gravityScale = 0f;
        myRigidbody.velocity = Vector2.zero;
        this.originalY = this.transform.position.y;
    }

    private void OnDestroy()
    {
        if(SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }

    private IEnumerator EnableCollider(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        myCollider.enabled = true;
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemData ItemData;
    public Vector3 Position;
    public Quaternion Rotation;

    public ItemPickUpSaveData(InventoryItemData _itemData,   Vector3 _position, Quaternion _rotation)
    {
        ItemData = _itemData;
        Position = _position;
        Rotation = _rotation;
    }
}