using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : Singleton<Score>
{
    [SerializeField] private TextMeshProUGUI scoreText, scoreText2, bestText;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScore(int point) => UpdateScore(score + point);

    private void UpdateScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        scoreText2.text = score.ToString();
        UpdateBest();
    }

    private void UpdateBest()
    {
        if (score > LoadBest())
            PlayerPrefs.SetInt("best", score);
        bestText.text = LoadBest().ToString();
    }

    private int LoadBest() => PlayerPrefs.GetInt("best", 0);
}
