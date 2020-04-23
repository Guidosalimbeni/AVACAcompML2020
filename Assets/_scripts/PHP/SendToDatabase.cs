using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.EventSystems;
using System;

public class SendToDatabase : MonoBehaviour
{
    // for opencv score usually I created an events from Opencv Manager
    // for not opencv score usually to access scores via components and methods from UI manager -- which I am fixing.. since do not make sense

    public ImageMatrixData imagePixelsValues; // not using it ... sending empty strings..
    public bool collectExtraStringImageData = false;

    private CollectDataRenderTexture collectdatarendertexture;
    private GamePopulationController gamePopulationController;
    private ScoreCalculator scoreCalculator;
    private PopulationManager populationmanager;
    private float judge;
    private List<GameObject> listOfelementsInComposition;
    private List<float> ListOfInfoForDatabase;
    private float scorePixelsBalance;
    private float scoreBoundsBalance;
    private float scoreUnityVisual;
    private string genesString;
    private int numberOfItems;
    private string sequence;
    private float scoreLawOfLever;
    private float scoreIsolationBalance;
    private float scoreAllscorefeatures;
    private float scoreNNFrontTop;
    private float scoreMobileNet;
    private float scoreFinalOut;

    private void Awake()
    {
        collectdatarendertexture = FindObjectOfType<CollectDataRenderTexture>();
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        populationmanager = FindObjectOfType<PopulationManager>();
        gamePopulationController = FindObjectOfType<GamePopulationController>();
    }

    private void Start()
    {
        ListOfInfoForDatabase = new List<float>();

        if (gamePopulationController != null)
        {
            listOfelementsInComposition = gamePopulationController.ElementsCompositions; // to fix since it is probably already in composition -- I think I fixed
        }
    }

    public void PostDataNegativeJudge()
    {
        UpdateActualListOfInfoForDatabase();

        scoreBoundsBalance = scoreCalculator.scoreBoundsBalance;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;
        scorePixelsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        scoreLawOfLever = scoreCalculator.scoreLawOfLever;
        scoreIsolationBalance = scoreCalculator.scoreIsolationBalance;
        scoreAllscorefeatures = scoreCalculator.scoreAllscorefeatures;
        scoreNNFrontTop = scoreCalculator.scoreNNFrontTop;
        scoreMobileNet = scoreCalculator.scoreMobileNet;
        scoreFinalOut = scoreCalculator.scoreFinalOut;

        judge = 0;               

        ListOfInfoForDatabase.Clear();

        StartCoroutine(PostData(LoadSceneInputUsername.username));
    }

    public void PostDataForPositiveJudge()
    {
        UpdateActualListOfInfoForDatabase();

        scoreBoundsBalance = scoreCalculator.scoreBoundsBalance;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;
        scorePixelsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        scoreLawOfLever = scoreCalculator.scoreLawOfLever;
        scoreIsolationBalance = scoreCalculator.scoreIsolationBalance;
        scoreAllscorefeatures = scoreCalculator.scoreAllscorefeatures;
        scoreNNFrontTop = scoreCalculator.scoreNNFrontTop;
        scoreMobileNet = scoreCalculator.scoreMobileNet;
        scoreFinalOut = scoreCalculator.scoreFinalOut;

        judge = 1;               

        ListOfInfoForDatabase.Clear();

        StartCoroutine(PostData(LoadSceneInputUsername.username)); //TO FIX

    }
    
    public void PostDataFromSnapShot(int scoreFromButton)
    {
        UpdateActualListOfInfoForDatabase();

        scoreBoundsBalance = scoreCalculator.scoreBoundsBalance;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;
        scorePixelsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        scoreLawOfLever = scoreCalculator.scoreLawOfLever;
        scoreIsolationBalance = scoreCalculator.scoreIsolationBalance;
        scoreAllscorefeatures = scoreCalculator.scoreAllscorefeatures;
        scoreNNFrontTop = scoreCalculator.scoreNNFrontTop;
        scoreMobileNet = scoreCalculator.scoreMobileNet;
        scoreFinalOut = scoreCalculator.scoreFinalOut;

        judge = scoreFromButton;

        ListOfInfoForDatabase.Clear();

        StartCoroutine(PostData("AI_TURN"));
    }

    IEnumerator PostData(string username)
    {
        if (username == "" | username == null)
        {
            username = "Debugging";
        }

        if (collectExtraStringImageData == true)
        {
            collectdatarendertexture.CollectPixelsValuesFromImageForMainViewRecordInDatabase(); // this update the scriptable object with the image string list pixels values
            collectdatarendertexture.CollectPixelsValuesFromImageForNeuralNetworkDNNOfflineTraining(); // this also update the scriptable object
        }

        // I now use the blob which is faster and then in python I can open using the script in COmp Anlysis in Artist Supervision Project
        byte[] bytesFullViewPaintCam = collectdatarendertexture.CollectPNGMainPaintCameraFullResolution();
        byte[] bytesFrontCam = collectdatarendertexture.CollectPNGFrontViewNN();
        byte[] bytesTopCam = collectdatarendertexture.CollectPNGFTopViewNN();
        WWWForm form = new WWWForm();


        form.AddField("name", username);
        form.AddField("filenameImg", "NotImplemented");
        form.AddField("numberOfItems", numberOfItems.ToString()); //
        form.AddField("sequence", sequence); // 
        form.AddField("genesString", genesString);
        form.AddField("scoreBoundsBalance", scoreBoundsBalance.ToString());
        form.AddField("scorePixelsBalance", scorePixelsBalance.ToString());
        form.AddField("scoreUnityVisual", scoreUnityVisual.ToString());
        form.AddField("scoreLawOfLever", scoreLawOfLever.ToString());
        form.AddField("scoreIsolationBalance", scoreIsolationBalance.ToString());

        form.AddField("scoreAllscorefeatures", scoreAllscorefeatures.ToString());
        form.AddField("scoreNNFrontTop", scoreNNFrontTop.ToString());
        form.AddField("scoreMobileNet", scoreMobileNet.ToString());

        form.AddField("NNtopView", imagePixelsValues.ImageNNtopView.ToString()); // not need anymore // sending empty string 
        form.AddField("NNFrontView", imagePixelsValues.ImageNNFrontView.ToString()); // not need anymore // sending empty string
        form.AddField("ImagePixelsList", imagePixelsValues.ImagePixelsListMainPaintView.ToString()); // not need anymore // sending empty string

        form.AddField("scoreFinalOut", scoreFinalOut.ToString());
        form.AddField("judge", judge.ToString());

        //form.AddBinaryData("image", bytes, "screenShot.png", "image/png");
        form.AddBinaryData("image", bytesFullViewPaintCam);
        form.AddBinaryData("imageFront", bytesFrontCam);
        form.AddBinaryData("imageTop", bytesTopCam);

        using (UnityWebRequest www = UnityWebRequest.Post("http://www.guidosalimbeni.it/UnityComp/AddToDatabase.php", form))
        {
            //System.Net.ServicePointManager.Expect100Continue = false; // to avoid some proxy blocking
            www.useHttpContinue = false;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!" + www.downloadHandler.text);
            }
        }
    }

    private void UpdateActualListOfInfoForDatabase()
    {
        sequence = "";
        genesString = "";


        foreach (GameObject element in listOfelementsInComposition)
        {
            ListOfInfoForDatabase.Add(element.transform.position.x);

            ListOfInfoForDatabase.Add(element.transform.position.z);
            ListOfInfoForDatabase.Add(element.transform.localEulerAngles.y);
            sequence += element.GetComponent<TagMeElementOfComposition>().ElementOfCompositionID.ToString() + ",";
            genesString += element.transform.position.x.ToString("0.000") + "," + element.transform.position.z .ToString("0.000") + "," + element.transform.localEulerAngles.y.ToString("0.000") + ",";

        }

        //genesString = string.Join(",", ListOfInfoForDatabase.ToArray());

        numberOfItems = listOfelementsInComposition.Count;
    }


}
