using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // ITEM DATA //
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    public GameObject coinPrefab; // Prefab cho tiền, gán trong Unity Editor
    public GameObject meatPrefab; // Prefab cho thịt, gán trong Unity Editor

    // ITEM SLOT //
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage;

    // ITEM DESCRIPTION SLOT //
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;
    private InventoryManager inventoryManager;

    private void Start() {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    
    public void AddItem(string newItemName, int newQuantity, Sprite newItemSprite, string itemDescription)
    {
        itemName = newItemName;
        quantity = newQuantity;
        itemSprite = newItemSprite;
        this.itemDescription = itemDescription;
        isFull = true;

        itemImage.sprite = itemSprite;
        UpdateQuantityText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        if (!isFull) // Kiểm tra nếu slot trống thì không làm gì cả
        {
            return;
        }
        if(thisItemSelected)
        {
            bool usable = inventoryManager.UseItem(itemName);
            if(usable)
            {
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if(this.quantity <= 0)
                    EmptySlot();
            }
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
            if(itemDescriptionImage.sprite == null)
                itemDescriptionImage.sprite = emptySprite;
        }
    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        isFull = false; // Set isFull to false when the slot is emptied

        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
    }

    public void OnRightClick()
    {
        GameObject itemToDrop = null;

        // Chọn prefab đúng cho từng loại vật phẩm
        if (itemName == "Coin" && coinPrefab != null)
        {
            itemToDrop = Instantiate(coinPrefab, GameObject.FindWithTag("Player").transform.position + new Vector3(2, 0, 0), Quaternion.identity);
        }
        else if (itemName == "Meat" && meatPrefab != null)
        {
            itemToDrop = Instantiate(meatPrefab, GameObject.FindWithTag("Player").transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        }

        // Điều chỉnh kích thước nếu cần thiết
        if (itemToDrop != null)
        {
            if (itemName == "Coin")
            {
                itemToDrop.transform.localScale = new Vector3(3f, 3f, 1f); // Kích thước của coin
            }
            else
            {
                itemToDrop.transform.localScale = new Vector3(1f, 1f, 1f); // Điều chỉnh kích thước phù hợp
            }

            
        }

        // Giảm số lượng và kiểm tra nếu ô trống
        this.quantity -= 1;
        quantityText.text = this.quantity.ToString();
        if(this.quantity <= 0)
            EmptySlot();
    }

    public void UpdateQuantityText()
    {
        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
    }
}
