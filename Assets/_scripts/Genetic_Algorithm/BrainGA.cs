using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// read the DNA and tells what to do to the character


public class BrainGA : MonoBehaviour
{
    private int NumberOfgenes = 3; // still need to hard code the genes anyway.. nned to change the brain move and the dna mutation...
	public float TotalScore;  // THIS IS IMPORTANT SINCE LINKS TO THE SORTED POPULATION INDIVIDUAL BRAIN
	public DNA dna;

    public float scorePixelsBalanceIndividual { get; set; }
    public float scoreUnityVisualIndividual { get; set; }
    public float scoreBoundsBalanceIndividual { get; set; }
    public float scoreLawOfLeverIndividual { get; set; }
    public float scoreIsolationBalanceIndividual { get; set; }

    public float scoreAllscorefeaturesIndividual { set; get; } 
    public float scoreNNFrontTopIndividual { get; set; }
    public float scoreMobileNetIndividual { get; set; }
    public float scoreFinalOutIndividual { get; set; }

    public List<float> genes = new List<float>();

    private int DNALength;
    private ScoreCalculator scoreCalculator; 
    private BrainNN_CompML brainNN_compML;
    private GamePopulationController gamePopulationController;
    

    private void Awake()
    {
        brainNN_compML = FindObjectOfType<BrainNN_CompML>();
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        gamePopulationController = FindObjectOfType<GamePopulationController>();
        DNALength = gamePopulationController.ElementsCompositions.Count * NumberOfgenes;
        
    }

    public void Init()
	{
        dna = new DNA(DNALength, gamePopulationController.MaxValues_x, gamePopulationController.MinValues_z);
    }

    public void InitForBreed()
    {
        dna = new DNA(DNALength, gamePopulationController.MaxValues_x, gamePopulationController.MinValues_z);
    }

    public void MoveComposition()
    {
        int genePos = 0;
        foreach (var elementComp in gamePopulationController.ElementsCompositions)
        {
            elementComp.transform.position = new Vector3(dna.GetGene(genePos), 0, dna.GetGene(genePos + 1));

            if (NumberOfgenes == 3)
            {
                elementComp.transform.eulerAngles = new Vector3(elementComp.transform.eulerAngles.x, dna.GetGene(genePos + 2), elementComp.transform.eulerAngles.z);
            }
            genePos = genePos + NumberOfgenes; // need to add rotation   // which also need to change the code in the mutation DNA..to account of this..
        }
        genes.Clear();
        genes = dna.GetGenesList();
    }

    public void MoveCompositionOfBestFitAfterAIfinishedIsTurn(List<float> genes)
    {
        int genePos = 0;
        foreach (var elementComp in gamePopulationController.ElementsCompositions)
        {
            elementComp.transform.position = new Vector3(genes[genePos], 0, genes[genePos + 1]);
            if (NumberOfgenes == 3)
            {
                elementComp.transform.eulerAngles = new Vector3(elementComp.transform.eulerAngles.x, genes[genePos + 2], elementComp.transform.eulerAngles.z);
            }
            genePos = genePos + NumberOfgenes;
        }
    }

    public void CalculateTotalScore()
    {
        if (scoreCalculator != null && brainNN_compML != null)
        {

            // call to trigger the events
            // the other calls are from leantouch event trigger
            //openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
            //gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();

            // change it soon to one score from ANN...or another TF model from external... which might be better to produce graphs.. 


            //scoreBoundsBalanceIndividual = scoreCalculator.scoreBoundsBalance; // not used

            //scorePixelsBalanceIndividual = scoreCalculator.visualScoreBalancePixelsCount; 
            //scoreUnityVisualIndividual = scoreCalculator.scoreUnityVisual;
            //scoreLawOfLeverIndividual = scoreCalculator.scoreLawOfLever;
            //scoreIsolationBalanceIndividual = scoreCalculator.scoreIsolationBalance;

            //scoreNNFrontTopIndividual = scoreCalculator.scoreNNFrontTop;
            //scoreMobileNetIndividual = scoreCalculator.scoreMobileNet;
            //scoreAllscorefeaturesIndividual = scoreCalculator.scoreAllscorefeatures;

            //TotalScore = scoreAllscorefeaturesIndividual + scoreNNFrontTopIndividual + scoreMobileNetIndividual;

            scoreFinalOutIndividual = scoreCalculator.scoreFinalOut;

            TotalScore = scoreFinalOutIndividual;

            //TotalScore = scorePixelsBalanceIndividual + scoreUnityVisualIndividual 
            //             + scoreLawOfLeverIndividual + scoreIsolationBalanceIndividual
            //             + scoreNNFrontTopIndividual + scoreMobileNetIndividual;

            // when I add a new score I need to update the last generation in population manager so that the score has sent to the database

        }
    }
}
