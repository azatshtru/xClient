using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

public class Client : MonoBehaviour
{
    public static Client Instance;

    public GameObject playerPrefab;

    public int maxPlayers;
    public string ipAddress;
    public int portNumber;

    TcpClient client;

    private string playerName;

    string message;
    bool execute;
    bool executeInstantiation;

    List<string> playersNames = new List<string>(); //IMPORTANT: Please do this later with Player Objects and in dictionary.

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void SetLocalClientName(string _name)
    {
        playerName = _name;
    }

    public void ConnectWithServer()
    {
        client = new TcpClient();
        client.Connect(IPAddress.Parse(ipAddress), portNumber);

        ThreadStart job = new ThreadStart(GetData);
        Thread thread = new Thread(job);
        thread.Start();

        playersNames.Add(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        if(execute == true)
        {
            UIManager.Instance.UpdateServerText(message);
            execute = false;
        }

        if(executeInstantiation == true)
        {
            InstantiatePlayers();
            //string s = "";
            //foreach (string pl in playersNames)
            //{
            //    s += pl;
            //}
            //UIManager.Instance.UpdateServerText(s);
            executeInstantiation = false;
        }
    }

    void StringCallback(string sender, string data)
    {
        if(data == "prologrfootrimplix")
        {
            if (playersNames.Contains(sender))
            {
                return;
            }
            playersNames.Add(sender);
            if(playersNames.Count == maxPlayers)
            {
                executeInstantiation = true;
            }
            return;
        }
        message = sender + ": " + data;
        execute = true;
    }

    void NumberCallback (string sender, int number)
    {
        //do something with the info.
    }

    void VectorCallback (string sender, Vector3 vector)
    {
        //do something with the data.
    }

    void InstantiatePlayers()
    {
        foreach(string player in playersNames)
        {
            GameObject playerGO = Instantiate(playerPrefab);
            playerGO.name = player;
        }
    }

    void GetData ()
    {
        while (true)
        {

            NetworkStream stream = client.GetStream();

            Thread.Sleep(250);

            //Check server/client.
            byte[] checkBytes = new byte[6];
            int checkBytesRead = stream.Read(checkBytes, 0, checkBytes.Length);
            string checkMsg = Encoding.ASCII.GetString(checkBytes, 0, checkBytesRead);

            if(checkMsg == "server")
            {
                //IMPORTANT: Change this bit of code later on to make the project scalable.
                byte[] serverMsgBytes = new byte[5];
                int serverBytesRead = stream.Read(serverMsgBytes, 0, serverMsgBytes.Length);
                string serverMsg = Encoding.ASCII.GetString(serverMsgBytes, 0, serverBytesRead);

                if(serverMsg == "start")
                {
                    HandleStart();
                }
            }

            if(checkMsg == "client")
            {
                //Getting name length.
                byte[] lenBytes = new byte[4];
                int lenBytesRead = stream.Read(lenBytes, 0, lenBytes.Length);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(lenBytes);
                }
                int nameLength = BitConverter.ToInt32(lenBytes, 0);

                //Getting name.
                byte[] nameBytes = new byte[nameLength];
                int nameBytesRead = stream.Read(nameBytes, 0, nameBytes.Length);
                string gotPlayerName = Encoding.ASCII.GetString(nameBytes, 0, nameBytesRead);

                //Getting type of value.
                byte[] typeBytes = new byte[3];
                int typeBytesRead = stream.Read(typeBytes, 0, typeBytes.Length);
                string _type = Encoding.ASCII.GetString(typeBytes, 0, typeBytesRead);

                //Getting data itself.
                if (_type == "STR")
                {
                    //Getting data length.
                    byte[] dataLenBytes = new byte[4];
                    int dataLenBytesRead = stream.Read(dataLenBytes, 0, dataLenBytes.Length);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(dataLenBytes);
                    }
                    int _dataLength = BitConverter.ToInt32(dataLenBytes, 0);

                    byte[] dataBytes = new byte[_dataLength];
                    int bytesRead = stream.Read(dataBytes, 0, dataBytes.Length);
                    string msg = GetStringData(dataBytes, bytesRead);

                    StringCallback(gotPlayerName, msg);
                }

                if (_type == "NUM")
                {
                    //Getting data length.
                    byte[] dataLenBytes = new byte[4];
                    int dataLenBytesRead = stream.Read(dataLenBytes, 0, dataLenBytes.Length);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(dataLenBytes);
                    }
                    int _dataLength = BitConverter.ToInt32(dataLenBytes, 0);

                    byte[] dataBytes = new byte[_dataLength];
                    int bytesRead = stream.Read(dataBytes, 0, dataBytes.Length);
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(dataBytes);
                    }
                    int msgInt = BitConverter.ToInt32(dataBytes, 0);

                    NumberCallback(gotPlayerName, msgInt);
                }
                
                if (_type == "VEC")
                {
                    //Getting data length.
                    byte[] dataLenBytes = new byte[4];
                    int dataLenBytesRead = stream.Read(dataLenBytes, 0, dataLenBytes.Length);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(dataLenBytes);
                    }
                    int _dataLength = BitConverter.ToInt32(dataLenBytes, 0);

                    float[] dir = new float[3];

                    for(int i = 0; i < _dataLength; i++)
                    {
                        byte[] dataBytes = new byte[4];
                        int bytesRead = stream.Read(dataBytes, 0, dataBytes.Length);
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(dataBytes);
                        }
                        dir[i] = BitConverter.ToSingle(dataBytes, 0);
                    }

                    Vector3 vector = new Vector3(dir[0], dir[1], dir[2]);
                    VectorCallback(gotPlayerName, vector);
                }
            }
        }
    }

    void HandleStart()
    {
        SendString("prologrfootrimplix");
    }

    string GetStringData (byte[] data, int dataLength)
    {
        string recievedMsg = Encoding.ASCII.GetString(data, 0, dataLength);

        return recievedMsg;
    }

    public void SendString (string msgToSend)
    {
        NetworkStream stream = client.GetStream();

        //This is for validating that message is sent by client.
        byte[] checkBytes = Encoding.ASCII.GetBytes("client");

        //Getting name length and converting to bytes.
        int nameLength = playerName.Length;
        byte[] lenBytes = BitConverter.GetBytes(nameLength);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(lenBytes);
        }

        //Converting name to bytes.
        byte[] nameBytes = Encoding.ASCII.GetBytes(playerName);

        //Getting Data Type of info and converting to bytes.
        string _type = "STR"; //TODO: This will be changed depending on the Data.
        byte[] typeBytes = Encoding.ASCII.GetBytes(_type);

        //Getting datalength
        int dataLength = msgToSend.Length;
        byte[] dataLenBytes = BitConverter.GetBytes(dataLength);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(dataLenBytes);
        }

        //Converting the actual data into bytes.
        byte[] msgInBytes = Encoding.ASCII.GetBytes(msgToSend);

        byte[] combyte = checkBytes.Concat(lenBytes).Concat(nameBytes).Concat(typeBytes).Concat(dataLenBytes).Concat(msgInBytes).ToArray();

        //Writing the data to stream.
        stream.Write(combyte, 0, combyte.Length);
    }

    public void SendNumber (int numToSend)
    {
        NetworkStream stream = client.GetStream();

        //This is for validating that message is sent by client.
        byte[] checkBytes = Encoding.ASCII.GetBytes("client");

        //Getting name length and converting to bytes.
        int nameLength = playerName.Length;
        byte[] lenBytes = BitConverter.GetBytes(nameLength);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(lenBytes);
        }

        //Converting name to bytes.
        byte[] nameBytes = Encoding.ASCII.GetBytes(playerName);

        //Getting Data Type of info and converting to bytes.
        string _type = "NUM"; //TODO: This will be changed depending on the Data.
        byte[] typeBytes = Encoding.ASCII.GetBytes(_type);

        //Getting datalength
        int dataLength = 4;
        byte[] dataLenBytes = BitConverter.GetBytes(dataLength);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(dataLenBytes);
        }

        //Converting the actual data into bytes.
        byte[] msgInBytes = BitConverter.GetBytes(numToSend);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(msgInBytes);
        }

        byte[] combyte = checkBytes.Concat(lenBytes).Concat(nameBytes).Concat(typeBytes).Concat(dataLenBytes).Concat(msgInBytes).ToArray();

        //Writing the data to stream.
        stream.Write(combyte, 0, combyte.Length);
    }
    
    public void SendVector (Vector3 vector)
    {
        NetworkStream stream = client.GetStream();

        //This is for validating that message is sent by client.
        byte[] checkBytes = Encoding.ASCII.GetBytes("client");

        //Getting name length and converting to bytes.
        int nameLength = playerName.Length;
        byte[] lenBytes = BitConverter.GetBytes(nameLength);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(lenBytes);
        }

        //Converting name to bytes.
        byte[] nameBytes = Encoding.ASCII.GetBytes(playerName);

        //Getting Data Type of info and converting to bytes.
        string _type = "VEC"; //TODO: This will be changed depending on the Data.
        byte[] typeBytes = Encoding.ASCII.GetBytes(_type);

        //Getting datalength
        int dataLength = 3;
        byte[] dataLenBytes = BitConverter.GetBytes(dataLength);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(dataLenBytes);
        }

        //Converting the actual data into bytes.
        float x = vector.x;
        float y = vector.y;
        float z = vector.z;

        byte[] xBytes = BitConverter.GetBytes(x);
        byte[] yBytes = BitConverter.GetBytes(y);
        byte[] zBytes = BitConverter.GetBytes(z);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(xBytes);
            Array.Reverse(yBytes);
            Array.Reverse(zBytes);
        }
        byte[] msgInBytes = xBytes.Concat(yBytes).Concat(zBytes).ToArray();

        byte[] combyte = checkBytes.Concat(lenBytes).Concat(nameBytes).Concat(typeBytes).Concat(dataLenBytes).Concat(msgInBytes).ToArray();

        //Writing the data to stream.
        stream.Write(combyte, 0, combyte.Length);
    }
}
