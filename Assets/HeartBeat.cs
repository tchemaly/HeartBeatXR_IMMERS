using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class HeartBeat : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    float receivedPos = 0;

    bool running;

    public UnityEngine.UI.Text myText;

    private Animation anim;
    void Start()
    {
        
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        anim = gameObject.GetComponent<Animation>();
    }
    // double the spin speed when true
    public float bpm = 60.0f;
    void Update()
    {
        this.transform.localPosition = new Vector3(0.0f, 0.5f, 0.5f);
        // have spin speed reverted to 1.0 second
        bpm = receivedPos;
        anim["heartbeat"].speed = (1.0f * bpm) / 60.0f;
        Debug.Log(receivedPos);
        myText.text = bpm.ToString();
        // leave spin or jump to complete before changing
        if (anim.isPlaying)
        {
            return;
        }
        anim.Play();
        this.transform.localPosition = new Vector3(0.0f, 0.5f, 0.5f);
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

        if (dataReceived != null)
        {
            //---Using received data---
            receivedPos = StringToFloat(dataReceived); //<-- assigning receivedPos value from Python
            print("received pos data, and moved the Cube!");
        }
    }

    public static float StringToFloat(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // store as a Vector3
        float result = float.Parse(sVector);

        return result;
    }

}
