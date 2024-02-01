using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHUD : MonoBehaviour
{
    public Image[] heartImages;
    public Sprite heartImage;
    public Sprite noHeartImage;

    // Start is called before the first frame update
    void Start()
    {
        UpdateImageCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateImageCanvas();
    }

    private void UpdateImageCanvas()
    {
        if(GameManager.lives == 0){
            for (int i = 0; i < heartImages.Length; i++)
            {
                heartImages[i].sprite = noHeartImage;
            }
        } 
        else
        {
            for (int i = 0; i < heartImages.Length; i++)
            {
                if (i == GameManager.lives - 1)
                {
                    heartImages[i].sprite = noHeartImage;
                }
            }
        }
    }
}
