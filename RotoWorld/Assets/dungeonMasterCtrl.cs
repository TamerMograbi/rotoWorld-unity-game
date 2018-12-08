using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dungeonMasterCtrl : MonoBehaviour {

    public Text sentence;
    public bool sentenceWasDisplayed;
    int waitTimeBetweenTexts;
    string[] seqOfTexts0, seqOfTexts1;
    // Use this for initialization
    void Start () {
        sentence.text = "";
        sentenceWasDisplayed = false;
        waitTimeBetweenTexts = 3;

        seqOfTexts0 = new string[6];
        seqOfTexts0[0] = "yo bro, ya'll need to guess my name If you want my treasure";
        seqOfTexts0[1] = "you need to enter each level and find the cube with one of the letters on it";
        seqOfTexts0[2] = "After that, you'll need to rearrange them here.";
        seqOfTexts0[3] = "If the order is right, then you win and get my treasure";
        seqOfTexts0[4] = "If it ain't, then ama divide by zero and make the game crash into oblivion";
        seqOfTexts0[5] = "Go on, try me. see If I'm joking";

        seqOfTexts1 = new string[6];
        seqOfTexts1[0] = "good, very good, you found the letter cube of the first level";
        seqOfTexts1[1] = "Now go get the others too";
        seqOfTexts1[2] = "My creater was too lazy to check if you already got the other cubes.";
        seqOfTexts1[3] = "So If you already got them then great, good job, you rock";
        seqOfTexts1[4] = "I wish I could move like you. you are so lucky";
        seqOfTexts1[5] = "but you know what they say... the grass always looks greener on the other side";


    }
	
	// Update is called once per frame
	void Update () {


	}

    public IEnumerator displaySequenceOfTexts(string[] seq)
    {
        for(int i = 0; i < seq.Length; i++)
        {
            sentence.text = seq[i];
            yield return new WaitForSeconds(5f);
        }
        sentence.text = "";


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            switch (GlobalCtrl.instance.dungeonMasterState)
            {
                case 0:
                {
                    StartCoroutine(displaySequenceOfTexts(seqOfTexts0));
                    break;
                }
                case 1:
                {
                    StartCoroutine(displaySequenceOfTexts(seqOfTexts1));
                    break;
                }
            }
            
        }
    }
}
