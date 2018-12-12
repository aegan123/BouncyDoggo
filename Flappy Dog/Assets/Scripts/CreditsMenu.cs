using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour {


    public static bool inCredits = false;

    public GameObject CreditsMenuUI;

	public void openCredits()
    {
        CreditsMenuUI.SetActive(true);
        inCredits = true;
    }

    public void back()
    {
        CreditsMenuUI.SetActive(false);
        inCredits = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inCredits)
            {
                openCredits();
            }
            else
            {
                back();
            }
        }
    }
}
