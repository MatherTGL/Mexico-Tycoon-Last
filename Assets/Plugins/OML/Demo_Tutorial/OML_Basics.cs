using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
namespace OML_CE
{
    public class OML_Basics : MonoBehaviour
    {

        #region [ ========== PreConfig ========== ]

        public Transform pagesContainer;
        public GameObject nextBtn;
        public GameObject prevBtn;

        #endregion [ ========== PreConfig (End) ========== ]

        List<GameObject> pages;

        int page = 0;
        int counter = 1;

        bool isCommunityVersion {
            get { return EditorPrefs.GetString("oml.version") == "community"; }
        }

        //
        // 
        //

        void Start()
        {
            pages = new List<GameObject>();

            foreach ( Transform page in pagesContainer)
            {
                pages.Add(page.gameObject);
            }

            UpdatePageButtonState();

//             for (int i = 0; i < pages.Count; i++)
//             {
//                 pages[i].SetActive(i == page);
//             }
// 
//             nextBtn.SetActive(true);
//             prevBtn.SetActive(false);
        }

        public void Print_Page1(int index)
        {
            switch (index)
            {
                case 0: Debug.Log("Hello World"); break;
                case 1: Debug.Log("[Greeting]Hello World"); break;
                case 2: Debug.Log("[Idea/Eng] Save the world!"); break;
            }
        }

        public void Print_Page2(int index)
        {
            switch (index)
            {
                case 0: Debug.Log("[*Tips] Hello World"); break;
                case 1: Debug.Log("[Tips*] Hello World"); break;
                case 2: Debug.Log("[Idea/Eng] Save the world!"); break;
            }
        }

        public void Print_Page3(int index)
        {
            switch (index)
            {
                case 0: Debug.Log("/main-session Testing"); break;
                case 1: Debug.Log("/main-session"); break;
                case 2: Debug.Log("[Test] message-" + (counter++)); break;
            }
        }


        public void Print_Page4(int index)
        {
            switch (index)
            {
                case 0: Debug.Log("/session"); break;
                case 1: Debug.Log("/session-end"); break;
                case 2: Debug.Log("[Test] message-" + (counter++)); break;
            }
        }

        public void Print_Page5(int index)
        {
            switch (index)
            {
                case 0: Debug.Log("/clear"); break;
                case 1:
                    Debug.Log("[OML/Info] <i>Welcome~</i> to <b>Oh My Log!</b>");

                    Debug.Log("/main-session Sample Session");

                    Debug.Log("[Greeting/Eng] Hello");
                    Debug.Log("[Greeting/Cht] 你好");
                    Debug.Log("[Greeting/Jap] こんにちは");
                    Debug.Log("[Chat] How are you?");
                    Debug.Log("[Leaving/Eng] Good Bye.");
                    Debug.Log("[Leaving/Cht] 再見");
                    Debug.Log("[Leaving/Jap] さようなら.");
                    Debug.Log("[OML/Test] very long sample~ very long sample~ very long sample~ very long sample~ very long sample~ very long sample~ very long sample~ very long sample~ very long sample~ ");
                    break;
                case 2: Debug.Log("[Test] message-" + (counter++)); break;
            }
        }

        public void Print_Page6(int index)
        {
            switch (index)
            {
                case 0: Debug.Log("/set Score=100"); break;
                case 1: Debug.Log("/set Score=50"); break;
                case 2:
                    Debug.Log("/set Apple.Radius=3");
                    Debug.Log("/set Apple.Size=6"); break;
            }
        }


        bool IsVisiblePage()
        {
            if (!isCommunityVersion)
                return true;

            return !pages[page].name.Contains("ProOnly");
        }

        public void Next()
        {
            while (true)
            {
                page++;
                if (IsVisiblePage())
                    break;
            }

            page = Mathf.Clamp(page, 0, pages.Count - 1);

            UpdatePageButtonState();
        }

        public void Prev()
        {
            while (true)
            {
                page--;
                if (IsVisiblePage())
                    break;
            }

            page = Mathf.Clamp(page, 0, pages.Count - 1);
            UpdatePageButtonState();
        }

        void UpdatePageButtonState()
        {
            for (int i = 0; i < pages.Count; i++)
            {
                pages[i].SetActive(i == page);
            }

            nextBtn.SetActive(page < pages.Count - 1);
            prevBtn.SetActive(page > 0);
        }
    }
}
