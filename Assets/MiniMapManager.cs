using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    public CanvasGroup minimapCanvas;
    private bool minimapOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Minimap"))
        {
            if (minimapOpen)
            {               
                minimapCanvas.alpha = 0;
                minimapCanvas.blocksRaycasts = false;
                minimapOpen = false;
            }
            else
            {             
                minimapCanvas.alpha = 1;
                minimapCanvas.blocksRaycasts = true;
                minimapOpen = true;
            }
        }
    }
}
