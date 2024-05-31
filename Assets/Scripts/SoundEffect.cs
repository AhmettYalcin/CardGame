using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    // Bu, çağırılacak ses dosyasıdır.
    public AudioClip sesDosyasi;
    
    // Bu, ses çalma bileşenidir.
    private AudioSource audioKaynak;
    
    void Start()
    {
        // Ses çalma bileşenini oluştur.
        audioKaynak = gameObject.AddComponent<AudioSource>();
    }

    // Bu metot, sesi çağırmak için kullanılır.
    public void Sound()
    {
        // Eğer ses dosyası yoksa, işlemi durdur.
        if (sesDosyasi == null)
        {
            Debug.LogWarning("Ses dosyası atanmamış!");
            
            return;
        }

        // Ses nesnesini oluştur ve oynat.
        audioKaynak.clip = sesDosyasi;
        audioKaynak.Play();

        print("sesmetod");
    }
}
