using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    public static GameEnd instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject tryAgainButton;
    
    public void OpenEndScreen()
    {
        Time.timeScale = 0;
        tryAgainButton.SetActive(true);
    }

    public void RestartGame()
{
    Time.timeScale = 1;  // Bật lại thời gian nếu đã dừng lại
    SceneManager.LoadScene(1);  // Tải lại chính scene hiện tại

    // Sau khi scene được tải lại, khôi phục các giá trị cần thiết
    //StatsManager.Instance.ResetPlayerStats();  // Khôi phục giá trị HP và damage
    // Tải lại dữ liệu EXP và kỹ năng nếu cần
    //ExpManager.instance.LoadPlayerData();
}

}
