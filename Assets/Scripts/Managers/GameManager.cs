using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Speed Control")]
    public Text SpeedButtonText;



    [Header("Planet Text Description")]
    public GameObject PlanetPanel;
    public Text PlanetHeaderText;
    public Text PlanetBodyText;

    public void OnClickSpeedButton()
    {
        //  Time.timeScale ?
        //  게임의 배속을 조절하는 변수 (기본 값 = 1)
        switch (Time.timeScale)
        {
            case 0:
                {
                    Time.timeScale = 1;
                    SpeedButtonText.text = "X 1";
                }
                break;

            case 1:
                {
                    Time.timeScale = 2;
                    SpeedButtonText.text = "X 2";
                }
                break;

            case 2:
                {
                    Time.timeScale = 4;
                    SpeedButtonText.text = "X 4";
                }
                break;

            case 4:
                {
                    Time.timeScale = 0;
                    SpeedButtonText.text = "||";
                }
                break;
        }
    }

    public void ReadPlanetDescription(GameObject planet)
    {
        if(planet == null)
        {
            PlanetPanel.SetActive(false);
            PlanetHeaderText.text = null;
            PlanetBodyText.text = null;
            return;

        }

        TextAsset textAsset = Resources.Load<TextAsset>(planet.name);

        using (MemoryStream ms = new MemoryStream(textAsset.bytes))
        using (StreamReader reader = new StreamReader(ms, System.Text.Encoding.UTF8))
        {
            while(!reader.EndOfStream)
            {
                string str = reader.ReadLine();
                if(str[0] == '#')
                {
                    if(str.Contains("Header"))
                    {
                        string header = str.Substring(8);
                        PlanetHeaderText.text = header;
                    }
                    else if (str.Contains("Color"))
                    {
                        string colorStr = str.Substring(7);
                        string[] c = colorStr.Split(',');
                        float r = float.Parse(c[0]) / 255;
                        float g = float.Parse(c[1]) / 255;
                        float b = float.Parse(c[2]) / 255;
                        float a = float.Parse(c[3]);

                        PlanetHeaderText.color = new Color(r, g, b, a);
                    }
                    else if (str.Contains("Description"))
                    {
                        string body = null;
                        while (!reader.EndOfStream)
                        {
                            body += reader.ReadLine().Replace("\\n", "\n");
                        }

                        PlanetBodyText.text = body;
                    }
                }
            }
        }
        PlanetPanel.SetActive(true);
    }

}
