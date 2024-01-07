using TMPro;
using UnityEngine;

public class SkipTextController : MonoBehaviour
{
    [SerializeField] private TMP_Text _skipText;
    [SerializeField] private string _phoneSkipText;
    [SerializeField] private string _pcSkipText;
    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _skipText.text = _phoneSkipText;
        }
        else
        {
            _skipText.text = _pcSkipText;
        }
    }
}
