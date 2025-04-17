using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    private AudioSource audioSource;
    private CapsuleCollider2D itemcollider;
    private SpriteRenderer sr;

    [SerializeField] private AudioClip pickupSound; // Âm thanh khi nhặt
    [SerializeField] private InventoryManager inventory; // Tham chiếu đến InventoryManager
    [SerializeField] private string itemName; // Tên của item
    [SerializeField] private int quantity = 1; // Số lượng
    [SerializeField] private Sprite itemSprite; // Sprite của item
    [SerializeField] private string itemDescription; // Mô tả item

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        itemcollider = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Phát âm thanh nhặt item
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }

            // Vô hiệu hóa object (ẩn khỏi game)
            itemcollider.enabled = false;
            sr.enabled = false;

            // Thêm item vào inventory
            if (inventory != null)
            {
                Debug.Log("Item added to inventory!");
                inventory.AddItem(itemName, quantity, itemSprite, itemDescription);
            }

            // Hủy object sau một thời gian để đảm bảo âm thanh phát xong
            Destroy(gameObject, pickupSound.length);
        }
    }
}
