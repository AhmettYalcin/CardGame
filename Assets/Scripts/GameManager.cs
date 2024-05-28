using Datas;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Manager;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text commentText; // Kaybettiniz veya Kazandınız mesajını gösterecek metin alanı
    public TMP_Text scoreText; // Skoru gösterecek metin alanı
    public GameObject EndPanel; // Endpaneli

    [field: SerializeField] public CardManager cardManager;
    [SerializeField] private bool isGameWin;

    public void GameLose(int score)
    {
        isGameWin = false;
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
        isGameWin = true;
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

    public void justBackToMenu()
    {
        //Data base e yükleme işlemleri 
        // kayıt işlemleri 

        if (isGameWin)
        {
            GiveGameAwards(cardManager.cardThemeIntCardManager, cardManager.newCardCount);
            
        }
        else
        {
            // Lose Funk
        }
       
        SceneManager.LoadScene("SampleScene");
    }

    private void GiveGameAwards(int thema, int intX)
    {
        switch (thema)
        {
            case 2:
                LevelSelect.instance.levels[0].LevelSliderCount++;
              
                print(LevelSelect.instance.levels[0].LevelSliderCount);
                break;
            case 5:
                LevelSelect.instance.levels[1].LevelSliderCount++;
                break;
            case 3:
                LevelSelect.instance.levels[2].LevelSliderCount++;
                break;
            case 4:
                LevelSelect.instance.levels[3].LevelSliderCount++;
                break;
            case 6:
                LevelSelect.instance.levels[4].LevelSliderCount++;
                break; 
            
        }

        switch (intX)
        {
            //Para 
            //Puan
            //CanPuanı 
            
            case 4:
                GameDataBase.instance.PlayerCoins += ((10 * intX)/2);
                GameDataBase.instance.PlayerPuans +=  ((10 * intX)/2);
                GameDataBase.instance.PlayerStamina += 1;
                break;
            case 16:
                GameDataBase.instance.PlayerCoins +=  ((10 * intX)/2);
                GameDataBase.instance.PlayerPuans +=  ((10 * intX)/2);
                GameDataBase.instance.PlayerStamina += 1;
                break;
            case 12:
                GameDataBase.instance.PlayerCoins +=  ((10 * intX)/2);
                GameDataBase.instance.PlayerPuans +=  ((10 * intX)/2);
                GameDataBase.instance.PlayerStamina += 1;
                break;
            case 24:
                GameDataBase.instance.PlayerCoins +=  ((10 * intX)/2);
                GameDataBase.instance.PlayerPuans +=  ((10 * intX)/2);
                GameDataBase.instance.PlayerStamina += 1;
                break;
        }
    }
}

