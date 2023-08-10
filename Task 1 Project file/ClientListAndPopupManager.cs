using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ClientListAndPopupManager : MonoBehaviour
{
    public GameObject clientListItemPrefab;
    public Transform clientListContainer;
    public Dropdown filterDropdown;
    public GameObject popup;
    public Text popupName;
    public Text popupPoints;
    public Text popupAddress;

    private const string apiURL = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";

    private void Start()
    {
        StartCoroutine(GetClientData());
    }

    private IEnumerator GetClientData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching client data: " + webRequest.error);
                yield break;
            }

            string jsonData = webRequest.downloadHandler.text;

            ClientDataWrapper dataWrapper = JsonConvert.DeserializeObject<ClientDataWrapper>(jsonData);




            PopulateClientList(dataWrapper.clients, dataWrapper.data);
        }

    }


    private void PopulateClientList(Client[] clients, Dictionary<string, ClientData> data)
    {
        void OnDropdownValueChanged(int value)
        {

            FilterIndex(clients, data);

        }
        filterDropdown.onValueChanged.AddListener(OnDropdownValueChanged);



        FilterIndex(clients, data);

    }

    private void FilterIndex(Client[] clients, Dictionary<string, ClientData> data)
    {
        int index = filterDropdown.value;

        foreach (Transform child in clientListContainer)
        {
            Destroy(child.gameObject);
        }

        if (index == 0)
        {
            foreach (Client client in clients)
            {

                GameObject listItem = Instantiate(clientListItemPrefab, clientListContainer);
                Text label = listItem.transform.Find("Label").GetComponent<Text>();
                Text points = listItem.transform.Find("Points").GetComponent<Text>();


                label.text = client.label;

                if (data.TryGetValue(client.id.ToString(), out var clientData))
                {
                    int clientPoints = clientData.points;
                    points.text = clientPoints.ToString();
                }
                else
                {
                    points.text = "N/A";
                }
                listItem.GetComponent<Button>().onClick.AddListener(() => ShowPopup(client, data));
            }
        }

        else if (index == 1)
        {
            foreach (Client client in clients)
            {
                if (client.isManager)
                {
                    GameObject listItem = Instantiate(clientListItemPrefab, clientListContainer);
                    Text label = listItem.transform.Find("Label").GetComponent<Text>();
                    Text points = listItem.transform.Find("Points").GetComponent<Text>();


                    label.text = client.label;

                    if (data.TryGetValue(client.id.ToString(), out var clientData))
                    {
                        int clientPoints = clientData.points;
                        points.text = clientPoints.ToString();
                    }
                    else
                    {
                        points.text = "N/A";
                    }
                    listItem.GetComponent<Button>().onClick.AddListener(() => ShowPopup(client, data));
                }
            }
        }
        else if (index == 2)
        {
            foreach (Client client in clients)
            {
                if (!client.isManager)
                {
                    GameObject listItem = Instantiate(clientListItemPrefab, clientListContainer);
                    Text label = listItem.transform.Find("Label").GetComponent<Text>();
                    Text points = listItem.transform.Find("Points").GetComponent<Text>();


                    label.text = client.label;

                    if (data.TryGetValue(client.id.ToString(), out var clientData))
                    {
                        int clientPoints = clientData.points;
                        points.text = clientPoints.ToString();
                    }
                    else
                    {
                        points.text = "N/A";
                    }
                    listItem.GetComponent<Button>().onClick.AddListener(() => ShowPopup(client, data));
                }
            }
        }

    }

    private void ShowPopup(Client client, Dictionary<string, ClientData> data)
    {
        if (data.TryGetValue(client.id.ToString(), out var clientData))
        {
            popupName.text = "Name :  " + clientData.name;
            popupPoints.text = "Points :  " + clientData.points.ToString();
            popupAddress.text = "Address :  " + clientData.address;
        }
        else
        {
            popupName.text = "Name :  " + "N/A";
            popupPoints.text = "Points :  " + "N/A";
            popupAddress.text = "Address :  " + "N/A";
        }

        popup.SetActive(true);
        popup.transform.DOScale(Vector3.one, 0.3f);

        popup.GetComponent<Button>().onClick.AddListener(() => ClosePopup());

    }

    private void ClosePopup()
    {
        popup.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => popup.SetActive(false));
    }


    [System.Serializable]
    public class Client
    {
        public bool isManager;
        public int id;
        public string label;
    }

    [System.Serializable]
    public class ClientData
    {
        public string address;
        public string name;
        public int points;

    }

    [System.Serializable]
    public class ClientDataWrapper
    {
        public Client[] clients;
        public Dictionary<string, ClientData> data;
        public string label;
    }


}

