using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;
    public bool isMatched = false;
    public Button button;
    public Image frontImage;
    public Image backImage;

    private bool isFlipped = false;
    private GameController gameManager;

    void Start()
    {
        button.onClick.AddListener(OnClick);
        gameManager = FindObjectOfType<GameController>();
    }

    public void SetImages(Sprite front, Sprite back)
    {
        frontImage.sprite = front;
        backImage.sprite = back;
        Flip(false);
    }

    public void OnClick()
    {
        if (isMatched || isFlipped || gameManager.IsBusy) return;
        Flip(true);
        gameManager.CardFlipped(this);
    }

    public void Flip(bool showFront)
    {
        isFlipped = showFront;
        frontImage.gameObject.SetActive(showFront);
        backImage.gameObject.SetActive(!showFront);
    }

    public void StayFlipped() => isMatched = true;
    public void ResetCard()
    {
        isMatched = false;
        Flip(false);
    }
}
