using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    private CanvasGroup visible;
    public TextMeshProUGUI money;
    public TextMeshProUGUI CI;
    public TextMeshProUGUI pName;

    public RawImage healthNum;
    public RawImage healthHeart;
    public RawImage portrait;

    public bool isDead;

    private HudSpriteContainer sprites;
    // Start is called before the first frame update
    void Start()
    {
        sprites = GameObject.Find("Hud").GetComponent<HudSpriteContainer>();
        visible = GetComponent<CanvasGroup>();
        Hide();
    }

    public void Hide()
    {
        isDead = true;
        visible.alpha = 0f;
        visible.blocksRaycasts = false;
    }

    public void Show()
    {
        isDead = false;
        visible.alpha = 1f;
        visible.blocksRaycasts = true;
    }

    public void Dim()
    {
        visible.alpha = 0.25f;
    }

    //The genius child
    public void UnDim()
    {
        visible.alpha = 100f;
    }

    public void updateMoney(int newMoney)
    {
        money.text = newMoney.ToString();
    }

    public void updateCI(int newCI)
    {
        CI.text = newCI.ToString();
    }

    public void updateHealth(int newHealth)
    {
        if (newHealth >= 4)
            healthHeart.texture = sprites.healthHearts[4];
        else if (newHealth >= 0)
            healthHeart.texture = sprites.healthHearts[newHealth];
        else
            healthHeart.texture = sprites.healthHearts[0];


        if (newHealth<=0)
        {
            isDead = true;
            healthNum.enabled = false;
            Dim();
        }
        else
            healthNum.texture = sprites.healthNumbers[newHealth];

    }
}
