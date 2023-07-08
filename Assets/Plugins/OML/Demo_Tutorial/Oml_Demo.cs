using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace OML_CE
{
    public class Oml_Demo : MonoBehaviour
    {
        public GameObject proVersionUI;
        public GameObject communityVersionUI;

        public Button mFightButton;
        public List<Image> mHpFills;
        public Text mRoundLabel;
        public Text mVictoryLabel;


        public enum GameState { Ready, Fighting, GameOver }

        int mRound = 0;
        int[] mUnitHps = new int[2];
        int[] mWinCounts = new int[2];

        Coroutine[] mUnitRoutines = new Coroutine[2];

        GameState mState = GameState.Ready;

        bool isCommunityVersion
        {
            get { return EditorPrefs.GetString("oml.version") == "community"; }
        }

        void Awake()
        {
            Debug.Assert(proVersionUI);
            Debug.Assert(communityVersionUI);
            Debug.Assert(mFightButton);
            Debug.Assert(mHpFills != null);
            Debug.Assert(mHpFills.Count >= 2);
            Debug.Assert(mRoundLabel);
            Debug.Assert(mVictoryLabel);
        }

        void Start()
        {   
            mRoundLabel.gameObject.SetActive(false);
            mVictoryLabel.gameObject.SetActive(false);

            OnTap_ViewNoraml();
            Debug.Log("/clear");
            Debug.Log("/highlight-clear");
            //Debug.Log("/theme dark");
            Debug.Log("/cmd AutoFoldOff");
        }


        
        void Update()
        {
            communityVersionUI.SetActive(isCommunityVersion);

            proVersionUI.SetActive(!isCommunityVersion);
            

            if (mState == GameState.Fighting)
            {
                if (mUnitHps[1] == 0) { DeclareVictory(0); }
                else if (mUnitHps[0] == 0) { DeclareVictory(1); }
                else
                {
                    // still in battle.
                }
            }
        }

        public void DeclareVictory(int index)
        {
            DoSesssionEnd("Fighting-Phase");



            mWinCounts[index]++;

            {
                Debug.Log("[Battle] Round Ended. Victory: " + GetUnitName(index));

                Debug.Log("[Scene] unloading scene...: ");
                Debug.Log("[Scene] unloading units...: ");

                Debug.Log("[Net] reporting result to server...: ");
            }

            DoSesssionEnd();

            mRoundLabel.gameObject.SetActive(false);

            for (int i = 0; i < 2; i++)
            {
                if (mUnitRoutines[i] != null)
                {
                    StopCoroutine(mUnitRoutines[i]);
                    mUnitRoutines[i] = null;
                }
            }

            if (++mRound < 3)
            {
                RoundBegin();
            }
            else
            {

                mVictoryLabel.gameObject.SetActive(true);
                mVictoryLabel.text = string.Format("{0} win!", GetUnitName(mWinCounts[0] > mWinCounts[1] ? 0 : 1));

                Debug.Log("[Battle] Battle is over. ");
                Debug.LogFormat("[Battle] {0} win count: {1}", GetUnitName(0), mWinCounts[0]);
                Debug.LogFormat("[Battle] {0} win count: {1}", GetUnitName(1), mWinCounts[1]);
                Debug.LogFormat("[Battle] {0} ", mVictoryLabel.text);

                Debug.Log("[Tips] Battle is over. Press [Fight] to play again.");

                SetState(GameState.GameOver);
            }
        }

        public void SetState(GameState s)
        {
            Debug.Log("[Core] state change: " + mState + " >>> " + s);

            mState = s;

            switch (mState)
            {
                case GameState.Fighting:
                    InitBattle();
                    break;

                case GameState.GameOver:
                    SetState(GameState.Ready);
                    break;

                case GameState.Ready:
                    Debug.Log("[View/Scene] re-enable UI.");
                    mFightButton.gameObject.SetActive(true);
                    break;
            }
        }

        public void InitBattle()
        {
            Debug.Log("/clear");

            DoSesssionBegin("Battle", true);

            
            Debug.Log("[Battle] initializing battle...");

            mRound = 0;
            mWinCounts[0] = 0;
            mWinCounts[1] = 0;

            Debug.Log("[Audio/BGM] play battle.mp3");

            Debug.LogFormat("[Net/Client] getting battle data from server...");
            Debug.LogFormat("[Net/Client] RECV Packet: SvBattleData (len=128)");


            Debug.Log("[View/Scene] update UI.");
            mFightButton.gameObject.SetActive(false);
            mVictoryLabel.gameObject.SetActive(false);

            RoundBegin();
        }

        void DoSesssionBegin( string name, bool main = false)
        {
            if (isCommunityVersion)
            {
                Debug.Log("---------- " + name + " BEGIN ----------");
            }
            else
            {
                if (main)
                {
                    Debug.Log("/main-session " + name);
                }
                else
                {
                    Debug.Log("/session " + name); 
                }
            }
        }

        void DoSesssionEnd(string name = "")
        {
            if (isCommunityVersion)
            {
                Debug.Log("---------- " + name + " END ----------");
            }
            else
            {
                Debug.Log("/session-end " + name);
            }
        }

        public void RoundBegin()
        {
            mRoundLabel.gameObject.SetActive(true);
            mRoundLabel.text = string.Format("Round-{0}", (mRound + 1));
            DoSesssionBegin("Round-" + (mRound + 1));

            DoSesssionBegin("Init-Phase");
            
            Debug.Log("[Audio] loading sound effects...");
            Debug.Log("[Scene] loading stage...");
            Debug.Log("[Scene] spawning units...");
            Debug.Log("[Battle] reseting character HPs.");

            DoSesssionEnd();
            

            mUnitHps[0] = 100;
            mUnitHps[1] = 100;

            DoSesssionBegin("Fighting-Phase");
            
            mUnitRoutines[0] = StartCoroutine(Co_UnitFlow(0, 0.5f));
            mUnitRoutines[1] = StartCoroutine(Co_UnitFlow(1, 1f));
        }

        IEnumerator Co_UnitFlow(int index, float delay)
        {
            yield return new WaitForSeconds(delay);

            while (mUnitHps[index] > 0)
            {
                Attack(index == 0 ? 1 : 0);

                yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
            }

            Debug.LogFormat("[Unit#{0}] {1} has fallen.", index + 1, GetUnitName(index));

            if (mUnitRoutines[index] != null)
                mUnitRoutines[index] = null;
        }

        string GetUnitName(int index)
        {
            return index == 0 ? "Skeleton" : "Orc";
        }

        public void Attack(int target)
        {
            var self = target == 0 ? 1 : 0;

            var miss = Random.Range(0, 10);
            if (miss < 2)
            {
                Debug.LogFormat("[Unit#{0}] {1} attack {2}...MISS", self + 1, GetUnitName(self), GetUnitName(target));
                return;
            }

            var dmg = Random.Range(20, 30);

            Debug.LogFormat("[Unit#{0}] {1} attack {2}...DMG={3}", self + 1, GetUnitName(self), GetUnitName(target), dmg);

            Debug.LogFormat("[Audio/SFX] play {0}_attack.mp3", GetUnitName(self));
            Debug.LogFormat("[View/Anim] Start-Anim: {0}_atttack", GetUnitName(self));

            mUnitHps[target] -= dmg;

            if (mUnitHps[target] < 0)
                mUnitHps[target] = 0;

            Debug.Log("target = " + target);
            Debug.Log("mUnitHps.length = " + mUnitHps.Length);

            mHpFills[target].fillAmount = mUnitHps[target] / 100f;

            Debug.LogFormat("[Unit#{0}] {1} HP={2}", target + 1, GetUnitName(target), mUnitHps[target]);

            Debug.LogFormat("[Audio/SFX] play {0}_hurt.mp3", GetUnitName(target));
            Debug.LogFormat("[View/Anim] Start-Anim: {0}_hurt", GetUnitName(target));
        }

        public void OnTap_Fight()
        {
            SetState(GameState.Fighting);
        }

        public void OnTap_ResetHighligts()
        {
            Debug.Log("/highlight-clear");
        }

        public void OnTap_EnableHighligts()
        {
            StartCoroutine(Co_EnableHighlights());
        }

        IEnumerator Co_EnableHighlights()
        {
            Debug.Log("/open-setting text_highlights");

            yield return new WaitForSeconds(0.1f);

            AddHighLight("DMG=[0-9]+", "Unit", "1,0.1,0.1"); yield return new WaitForSeconds(0.1f);
            AddHighLight("MISS", "Unit", "1,0.5,0.25"); yield return new WaitForSeconds(0.1f);
            AddHighLight("HP=[0-9]+", "Unit", "0.2,1,0"); yield return new WaitForSeconds(0.1f);
            AddHighLight("Victory", "Battle", "1,0.85,0"); yield return new WaitForSeconds(0.1f);
            AddHighLight(">>>.+", "Core", "1,1,0"); yield return new WaitForSeconds(0.1f);
            AddHighLight("attack", "Unit", "1,1,0.5", FontStyle.BoldAndItalic); yield return new WaitForSeconds(0.1f);
            AddHighLight("Start-Anim", "Anim", "1,0.9,0.8", FontStyle.BoldAndItalic); yield return new WaitForSeconds(0.1f);
            AddHighLight("[^\\s]+.mp3", "SFX", "1,0.4,0.95", FontStyle.Italic); yield return new WaitForSeconds(0.1f);
            AddHighLight("[^\\s]+.mp3", "BGM", "1,0.4,0.95", FontStyle.Italic); yield return new WaitForSeconds(0.1f);

            //yield return new WaitForSeconds(1f);

            //Debug.Log("/close-setting");
        }

        void AddHighLight(string pattern, string tag, string color, FontStyle style = FontStyle.Bold)
        {
            var fmt = "/highlight {0}\n{1}\n{2}\n{3}";

            Debug.LogFormat(fmt, pattern, tag, color, (int)style);
        }

        public void OnTap_AddGroups()
        {
            Debug.Log("/new-group Battle, Battle, Unit");
            Debug.Log("/new-group Network, Net,Client");
            Debug.Log("/new-group Audio, Audio, BGM, SFX");
            Debug.Log("/new-group Anim, Anim");
            Debug.Log("/cmd ViewBar_TabMode");
        }

        public void OnTap_ViewNoraml()
        {
            //Debug.Log("/cmd ViewBar_Off");
            Debug.Log("/cmd ViewBar_TabMode");

            Debug.Log("/set-channel 0,0");
            Debug.Log("/set-channel 0,1");
        }

        public void OnTap_ViewAnimGroup()
        {
            Debug.Log("/cmd ViewBar_TabMode");

            //Debug.Log("/select-group default,0");
            Debug.Log("/select-group Anim,0");

            Debug.Log("/set-channel 0,0");
            //Debug.Log("/set-channel 0,1");
        }

        public void OnTap_ViewChannel1()
        {
            Debug.Log("/cmd ViewBar_MixedMode");

            Debug.Log("/select-group default,0");
            //Debug.Log("/select-group Battle,1");

            Debug.Log("/set-channel 1,0");
            //Debug.Log("/set-channel 1,1");
        }

    }
}