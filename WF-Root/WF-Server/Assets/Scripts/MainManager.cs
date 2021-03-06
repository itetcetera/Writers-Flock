﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace WritersFlock
{
    public class MainManager : MonoBehaviour
    {
        private AudioSource audioSource;
        public AudioSource narSource;

        public AudioClip round1Intro;
        public AudioClip round2Intro;
        public AudioClip round3Intro;

        public List<AudioClip> music;
        public GameObject writingPanel;
        public GameObject singleVotingPanel;
        public GameObject groupVotingPanel;
        public Text singleVotingTitle;
        public Text singleVotingBody;
        public List<Text> groupVotingTitles;
        public List<Text> groupVotingBody;
        public List<RoundSettings> rounds;
        [HideInInspector]
        public int roundIndex = 0;
        [HideInInspector]
        public int currentTurn = 0;

        public AudioClip round1Music;
        public AudioClip round2Music;
        public AudioClip round3Music;


        public void Start ()
        {
            audioSource = GetComponent<AudioSource>();
            ChangeToWritingPanel();
            ServerManager.instance.StartWritingRound(this);
        }

        public void ChangeToGroupVotingPanel (List<Title> titles)
        {
            currentTurn = 0;
            groupVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
            for (int i = 0; i < groupVotingTitles.Count; i++)
            {
                if(i >= titles.Count)
                {
                    groupVotingTitles[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }
                groupVotingTitles[i].text = titles[i].titleText;
                groupVotingBody[i].text = "";
            }
        }

        public void ChangeToGroupVotingPanel (List<Story> stories)
        {
            currentTurn = 0;
            groupVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
            for (int i = 0; i < groupVotingTitles.Count; i++)
            {
                if (i >= stories.Count)
                {
                    groupVotingTitles[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }

                string body = "";
                var sentances = stories[i].sentances;
                for (int j = 0; j < sentances.Count; j++)
                {
                    body += sentances[j] + " ";
                }

                groupVotingTitles[i].text = stories[i].title;
                groupVotingBody[i].text = body;
            }
        }

        public void ChangeToGroupVotingPanel (List<string> titles, List<string> bodys)
        {
            currentTurn = 0;
            groupVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
            for (int i = 0; i < groupVotingTitles.Count; i++)
            {
                if (i >= titles.Count)
                {
                    groupVotingTitles[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }
                groupVotingTitles[i].text = titles[i];
                groupVotingBody[i].text = bodys[i];
            }
        }

        public void ChangeToSingleVotingPanel ()
        {
            currentTurn = 0;
            singleVotingPanel.SetActive(true);
            writingPanel.SetActive(false);
            groupVotingPanel.SetActive(false);
        }

        public void AdvanceSingleVotingPanel(string title, string body)
        {
            singleVotingTitle.text = title;
            singleVotingBody.text = body;
        }

        public void ChangeToWritingPanel ()
        {
            currentTurn = 0;
            writingPanel.SetActive(true);
            groupVotingPanel.SetActive(false);
            singleVotingPanel.SetActive(false);
            switch (CurrentRound().roundNumber)
            {
                case 1:
                    audioSource.clip = round1Music;
                    narSource.clip = round1Intro;
                    break;
                case 2:
                    audioSource.clip = round2Music;
                    narSource.clip = round2Intro;
                    break;
                case 3:
                    audioSource.clip = round3Music;
                    narSource.clip = round3Intro;
                    break;
            }
            audioSource.Play();
            narSource.Play();

        }

        public RoundSettings CurrentRound ()
        {
            return rounds[roundIndex];
        }
    }
}
