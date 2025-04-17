using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ChangeEquipment : MonoBehaviour
{
    public Player_Combat combat;
    public Player_Bow bow;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("ChangeEquipment"))
        {
            if (combat.enabled) // Nếu đang ở trạng thái kiếm
            {
                combat.DisableCombat(); // Hủy trạng thái kiếm
                combat.enabled = false; // Vô hiệu hóa script combat
                bow.enabled = true; // Kích hoạt script bắn cung
            }
            else // Nếu đang ở trạng thái cung
            {
                bow.enabled = false; // Vô hiệu hóa script bắn cung
                combat.enabled = true; // Kích hoạt script combat
                combat.EnableCombat(); // Bật lại trạng thái kiếm
            }
        }
    }
}
