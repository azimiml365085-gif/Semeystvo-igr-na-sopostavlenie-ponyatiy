using Unity.VisualScripting;
using UnityEngine;

public class HelpCall : MonoBehaviour
{
    [SerializeField] private GameObject tipPanelCS;
    [SerializeField] private GameObject tipPanelB;
    [SerializeField] private GameObject tipPanelLvlEd;
    [SerializeField] private GameObject tipPanelQ;
    
    void Awake()
    {
        tipPanelB.SetActive(false);
        tipPanelLvlEd.SetActive(false);
        tipPanelCS.SetActive(false);
        tipPanelQ.SetActive(false);
    }
    public void CSHelpOn()
    {
        tipPanelCS.SetActive(true);
    }

    public void BHelpOn()
    {
        tipPanelB.SetActive(true);
    }

    public void LvlEdHelpOn()
    {
        tipPanelLvlEd.SetActive(true);
    }

    public void QHelpOn()
    {
        tipPanelQ.SetActive(true);
    }

    public void CSHelpOff()
    {
        tipPanelCS.SetActive(false);
    }

    public void BHelpOff()
    {
        tipPanelB.SetActive(false);
    }

    public void LvlEdHelpOff()
    {
        tipPanelLvlEd.SetActive(false);
    }

    public void QHelpOff()
    {
        tipPanelQ.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.OpenURL("file:///" + Application.streamingAssetsPath + "/Help.chm");
        }

    }
}
