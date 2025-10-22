using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class GameController : MonoBehaviour
{
    [Header("Card & Board Setup")]
    public GameObject cardPrefab;
    public Transform grid;
    public int rows = 4, cols = 3;
    public Sprite[] frontSprites;
    public Sprite backSprite;
    public Sprite fillerSprite;

    [Header("UI Elements")]
    public TMP_Text currentScoreText;
    public TMP_Text highestScoreText;
    public TMP_Text comboText;
    public TMP_Text gameOverText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip flipSound, matchSound, mismatchSound, gameOverSound;

    [HideInInspector] public bool IsBusy = false;

    private List<Card> cards = new List<Card>();
    private Card firstCard;

    private int score = 0;
    private int highScore = 0;
    private int pairsFound = 0;

    // Combo system
    private int comboCount = 0;
    private int comboMultiplier = 1;

    void Start()
    {
       // PlayerPrefs.DeleteAll();
        gameOverText.gameObject.SetActive(false);
        LoadHighScore();
        CreateBoard();
        StartCoroutine(ShowAllCardsThenHide());
        UpdateScoreUI();
    }

    void CreateBoard()
    {
        int totalCards = rows * cols;
        int pairCount = totalCards / 2;

        if (pairCount > frontSprites.Length)
        {
            Debug.Log("Not enough front sprites for all pairs, limiting to available sprites.");
            pairCount = frontSprites.Length;
        }

        List<int> ids = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        if (ids.Count < totalCards)
        {
            int randomIndex = Random.Range(0, frontSprites.Length);
            ids.Add(randomIndex);
        }

        // Shuffle ids
        for (int i = 0; i < ids.Count; i++)
        {
            int rand = Random.Range(i, ids.Count);
            (ids[i], ids[rand]) = (ids[rand], ids[i]);
        }

        // Dynamic grid sizing
        GridLayoutGroup glg = grid.GetComponent<GridLayoutGroup>();
        RectTransform gridRect = grid as RectTransform;
        float totalWidth = gridRect.rect.width;
        float totalHeight = gridRect.rect.height;
        float spacingX = glg.spacing.x * (cols - 1);
        float spacingY = glg.spacing.y * (rows - 1);
        float availableWidth = totalWidth - spacingX;
        float availableHeight = totalHeight - spacingY;
        float cellW = availableWidth / cols;
        float cellH = availableHeight / rows;
        float cellSize = Mathf.Min(cellW, cellH);
        glg.cellSize = new Vector2(cellSize, cellSize);

        float totalUsedHeight = (cellSize * rows) + spacingY;
        float yOffset = (totalHeight - totalUsedHeight) / 2f;
        gridRect.offsetMin = new Vector2(gridRect.offsetMin.x, yOffset);
        gridRect.offsetMax = new Vector2(gridRect.offsetMax.x, -yOffset);

        // Instantiate cards
        foreach (int id in ids)
        {
            GameObject cardGO = Instantiate(cardPrefab, grid);
            Card card = cardGO.GetComponent<Card>();
            Sprite frontToUse = (id >= 0 && id < frontSprites.Length) ? frontSprites[id] : fillerSprite;
            card.id = id;
            card.SetImages(frontToUse, backSprite);
            cards.Add(card);
        }
    }

    IEnumerator ShowAllCardsThenHide()
    {
        foreach (var c in cards) c.Flip(true);
        yield return new WaitForSeconds(2f);
        foreach (var c in cards) if (!c.isMatched) c.Flip(false);
    }

    public void CardFlipped(Card card)
    {
        audioSource.PlayOneShot(flipSound);

        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            StartCoroutine(CheckMatch(card));
        }
    }

    IEnumerator CheckMatch(Card secondCard)
    {
        IsBusy = true;
        yield return new WaitForSeconds(0.5f);

        if (firstCard.id == secondCard.id)
        {
            audioSource.PlayOneShot(matchSound);

            firstCard.StayFlipped();
            secondCard.StayFlipped();
            pairsFound++;

            // Combo system
            comboCount++;
            comboMultiplier = 1 + comboCount - 1; // 1x for first, increases each consecutive match
            int pointsEarned = 10 * comboMultiplier;
            score += pointsEarned;

            // Update high score
            if (score > highScore)
            {
                highScore = score;
                SaveHighScore();
            }

            UpdateScoreUI();

            if (pairsFound == (rows * cols) / 2)
            {
                gameOverText.gameObject.SetActive(true);
                audioSource.PlayOneShot(gameOverSound);
            }
        }
        else
        {
            audioSource.PlayOneShot(mismatchSound);
            yield return new WaitForSeconds(0.5f);
            firstCard.Flip(false);
            secondCard.Flip(false);

            // Reset combo on mismatch
            comboCount = 0;
            comboMultiplier = 1;
            UpdateScoreUI();
        }

        firstCard = null;
        IsBusy = false;
    }

    void UpdateScoreUI()
    {
        currentScoreText.text = "Score: " + score;
        highestScoreText.text = "Best Score: " + highScore; 
        
        if (comboText != null)
            comboText.text = comboCount > 0 ? "Combo x " + comboCount : "";

    }

    void SaveHighScore()
    {
        PlayerPrefs.SetInt("highscore", highScore);
        PlayerPrefs.Save();
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("highscore", 0);
    }
}
