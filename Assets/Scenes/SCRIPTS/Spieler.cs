using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spieler : MonoBehaviour
{
    readonly float eingabeFaktor = 10;
    public GameObject gewinn;

    int anzahlPunkte = 0;
    public TMP_Text punkteAnzeige;
    public TMP_Text lebenAnzeige;
    public TMP_Text zeitAnzeige;
    public TMP_Text zeitAltAnzeige;
    int anzahlLeben = 3;
    bool spielGestartet = false;
    float zeitStart;

    // Start is called before the first frame update
    void Start()
    {
        float zeitAlt = 0;
        if (PlayerPrefs.HasKey("zeitAlt"))
        {
            zeitAlt = PlayerPrefs.GetFloat("zeitAlt");
            zeitAltAnzeige.text = string.Format("Alt: {0,6:0.0} sec", zeitAlt);
        }
        lebenAnzeige.text = "Leben: " + anzahlLeben;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gewinn"))
        {
            anzahlPunkte++;
            if (anzahlPunkte < 6)
            {
                punkteAnzeige.text = "Punkte: " + anzahlPunkte;
            }
            else
            {
                gameObject.SetActive(false);
                gewinn.SetActive(false);
                punkteAnzeige.text = "Gewonnen";
                PlayerPrefs.SetFloat("zeitAlt", Time.time - zeitStart);
                PlayerPrefs.Save();
            }

            float xNeu = Random.Range(-8.0F, 8.0f);
            float yNeu;
            if (anzahlPunkte < 2)
            {
                yNeu = -2.7f;
            }
            else if (anzahlPunkte < 4)
            {
                yNeu = 0.15f;
            }
            else
            {
                yNeu = 3;
            }
            gewinn.transform.position = new Vector3(xNeu, yNeu, 0);



        }
        else if (collision.gameObject.CompareTag("Gefahr"))
        {
            anzahlLeben--;
            lebenAnzeige.text = "Leben: " + anzahlLeben;
            gameObject.SetActive(false);
            if (anzahlLeben > 0) { 
                Invoke(nameof(NaechstesLeben), 2);
            } else
            {
                gewinn.SetActive(false);
                punkteAnzeige.text = "Verloren, du versager!";
            }
        }

    }

    void NaechstesLeben()
    {
        transform.position = new Vector3(1f, -3.4f, 0);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Eingabe zwischenspeichern
        float xEingabe = Input.GetAxis("Horizontal");
        float yEingabe = Input.GetAxis("Vertical");

        if (xEingabe != 0)
        {
            Debug.Log("X: " + xEingabe);
        }
        if (yEingabe != 0) {
            Debug.Log("Y: " + yEingabe);
        }

        if (yEingabe < 0)
        {
            return;
        }

        if (!spielGestartet && (xEingabe != 0 || yEingabe != 0))
        {
            spielGestartet = true;
            zeitStart = Time.time;
        }
        if (spielGestartet)
        {
            zeitAnzeige.text = string.Format("Zeit: {0,6:0.0} sec.", Time.time - zeitStart);
        }

        // Neue Position bestimmen
        float xNeu = transform.position.x + xEingabe * eingabeFaktor * Time.deltaTime;
        float yNeu = transform.position.y + yEingabe * eingabeFaktor * Time.deltaTime;

        /*
        if (xNeu > 8.3f)
        {
            xNeu = 8.3f;
        }
        if (xNeu < -8.3f)
        {
            xNeu = -8.3f;
        }
        */

        transform.position = new Vector3(xNeu, yNeu, 0);

    }
}
