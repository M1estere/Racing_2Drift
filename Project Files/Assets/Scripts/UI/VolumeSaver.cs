using UnityEngine;
using UnityEngine.UI;

public class VolumeSaver : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    
    void Start()
    {
        LoadValues();
    }

    public void SaveVolume()
    {
        float volumeValue = _volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadValues();
    }
    
    private void LoadValues()
    {
        float volumeValue;
        if (PlayerPrefs.HasKey("VolumeValue"))
            volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        else
            volumeValue = 1;
        
        _volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
}
