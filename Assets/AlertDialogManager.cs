using UnityEngine;
using UnityEngine.UI;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text messageText;
    public Button okButton;
    public Button cancelButtton;

    private System.Action<bool> responceCallback;

    void Start()
    {
        dialogBox.SetActive(false);

        okButton.onClick.AddListener(()=> HandleResponse(true));
        cancelButtton.onClick.AddListener(()=> HandleResponse(false));
    }

    public void ShowDialog(string message, System.Action<bool> callback)
    {
        responceCallback = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    private void HandleResponse(bool response)
    {
        dialogBox.SetActive(false);
        responceCallback?.Invoke(response);
    }
}
