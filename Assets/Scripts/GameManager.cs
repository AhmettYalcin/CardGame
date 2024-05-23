using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public TMP_Text commentText; // Kaybettiniz veya Kazandınız mesajını gösterecek metin alanı
    public TMP_Text scoreText; // Skoru gösterecek metin alanı
    public GameObject EndPanel; // Endpaneli

    public void GameLose(int score)
    {
        // Kaybettiniz mesajını göster
        commentText.text = "You Lose! Try Again";
        // Skoru göster
        scoreText.text = "Score: " + score.ToString();
        // Paneli aktif et ve dotween animasyonu ekle
        if (EndPanel != null)
        {
            EndPanel.SetActive(true);
            EndPanel.transform.localScale = Vector3.zero; // Başlangıç ölçeğini sıfır yap
            EndPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Büyüme animasyonu
        }
        else
        {
            Debug.LogError("Lose panel not assigned!");
        }
    }

    public void GameWin(int score)
    {
        // Kazandınız mesajını göster
        commentText.text = "Congratulations! You Win";
        // Skoru göster
        scoreText.text = "Score: " + score.ToString();
        // Paneli aktif et ve dotween animasyonu ekle
        if (EndPanel != null)
        {
            EndPanel.SetActive(true);
            EndPanel.transform.localScale = Vector3.zero; // Başlangıç ölçeğini sıfır yap
            EndPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); // Büyüme animasyonu
        }
        else
        {
            Debug.LogError("Win panel not assigned!");
        }
    }
}

