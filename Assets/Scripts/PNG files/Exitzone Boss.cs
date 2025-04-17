using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitzoneBoss : MonoBehaviour
{
    //public Collider2D[] mountainColliders;
    public Collider2D[] boundaryColliders;
    private bool isActivated = false; // Biến kích hoạt

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (isActivated && collision.CompareTag("Player"))
        {
            Debug.Log("Người chơi đã chạm vào Elevation Exit.");

            foreach (Collider2D boundary in boundaryColliders) 
            {
                Debug.Log("Tắt Boundary Collider: " + boundary.name);
                boundary.enabled = false; // Tắt các Collider biên giới
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5; // Thay đổi thứ tự hiển thị
        }
        else if (!isActivated)
        {
            Debug.Log("Elevation Exit chưa được kích hoạt.");
        }
    }

    public void Activate()
    {
        isActivated = true; // Kích hoạt khi boss bị đánh bại
        Debug.Log("Elevation Exit đã được kích hoạt!");
    }
}
