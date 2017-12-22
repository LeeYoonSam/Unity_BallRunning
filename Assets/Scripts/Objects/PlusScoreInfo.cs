using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusScoreInfo : MonoBehaviour
{
    private static ScoreManager scoreManager;

    private void Awake()
    {
        if (scoreManager == null)
        {
            scoreManager = GameObject.FindWithTag("GameManager").GetComponent<ScoreManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scoreManager.PlusScore(200);
            scoreManager.ScoreTextColorEffect();
            gameObject.SetActive(false);
        }
    }
}
