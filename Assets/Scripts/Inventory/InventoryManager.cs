using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public CanvasGroup inventoryCanvas;
    private bool inventoryOpen = false;    
    public ItemSlot[] itemSlot;
    public ItemSo[] itemSos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Inventory"))
        {
            if(inventoryOpen)
            {
                //Time.timeScale = 1;
                inventoryCanvas.alpha = 0;
                inventoryCanvas.blocksRaycasts = false;
                inventoryOpen = false;
            }
            else
            {
                //Time.timeScale = 0;
                inventoryCanvas.alpha = 1;
                inventoryCanvas.blocksRaycasts = true;
                inventoryOpen = true;
            }
        }
    }

    public bool UseItem(string itemName)
    {
        for(int i = 0; i < itemSos.Length; i++)
        {
            if(itemSos[i].itemName == itemName)
            {
                bool usable = itemSos[i].UseItem();
                return usable;
            }
        }
        return false;
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
{
    for (int i = 0; i < itemSlot.Length; i++)
    {
        // Kiểm tra nếu item đã tồn tại trong slot
        if (itemSlot[i].isFull && itemSlot[i].itemName == itemName)
        {
            itemSlot[i].quantity += quantity; // Cộng thêm số lượng nếu trùng
            itemSlot[i].UpdateQuantityText();
            return;
        }
    }

    // Nếu không tìm thấy item trùng, thêm vào slot trống
    for (int i = 0; i < itemSlot.Length; i++)
    {
        if (itemSlot[i].isFull == false)
        {
            itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
            return;
        }
    }
}

    public void DeselectAllSlots()
    {
        for(int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }

}
