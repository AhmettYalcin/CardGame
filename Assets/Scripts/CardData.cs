using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewCard", menuName = "Custom/Card")]
public class CardData : ScriptableObject
{
    public int cardID; // Kartın benzersiz kimliği
    public string cardName; // Kartın ismi veya açıklaması
    public Sprite cardImage; // Kartın ön yüzündeki görüntü
    public Sprite backImage; //Kartın arka yüzündeki görüntü
    public Button cardButton; // Kartın bağlı olduğu button

    public void Initialize(int id, string name, Sprite image,Sprite backImage)
    {
        cardID = id;
        cardName = name;
        cardImage = image;
        this.backImage = backImage;
    }
}

