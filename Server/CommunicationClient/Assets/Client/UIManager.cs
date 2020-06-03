using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject lobby;
    public GameObject inGame;

    public InputField nameField;
    public InputField messengerField;
    public Text serverText;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Client.Instance.SendString(messengerField.text);
        }
    }

    public void UpdateServerText(string msg)
    {
        if(serverText == null)
        {
            return;
        }
        serverText.text = msg;
    }

    public void JoinGame()
    {
        Client.Instance.SetLocalClientName(nameField.text);
        Client.Instance.ConnectWithServer();
        lobby.SetActive(false);
        inGame.SetActive(true);
    }
}
