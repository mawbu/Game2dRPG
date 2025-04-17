
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----------Audio Soure---------")]
    [SerializeField] AudioSource musicSoure;
    [SerializeField] AudioSource SFXSoure;

    [Header("----------Audio Clip---------")]
    public AudioClip background;

    private void Start()
    {
        musicSoure.clip = background;
        musicSoure.Play();
    }
}
