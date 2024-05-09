using UnityEngine;
using System.Collections.Generic;

public class CardList : MonoBehaviour
{
    public List<CardData> cards = new List<CardData>(); // Tüm kartların listesi

    // Kartların resimleri dizisi
    public Sprite[] cardImages;

    // Kartların arka resmi
    public Sprite backImage;

    void Start()
    {
        // Kartları oluşturmak için fonksiyonu çağır
        CreateCards(16); // Örnek olarak 16 kart oluşturduk
    }

    void CreateCards(int count)
    {
        // İstenen sayıda kart ekleyelim
        for (int i = 0; i < count; i++)
        {
            // Yeni bir kart oluştur
            CardData newCard = new CardData();

            // Kartın özelliklerini belirle
            newCard.Initialize(i, cardImages[i].name, cardImages[i], backImage);

            // Oluşturulan kartı listeye ekle
            cards.Add(newCard);
        }
    }
}
