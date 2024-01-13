using UnityEngine;

public class ChooseDriver : MonoBehaviour
{
    public void ChooseDriverButton(string driverName)
    {
        PlayerPrefs.SetString("Driver", driverName);
    }
}
