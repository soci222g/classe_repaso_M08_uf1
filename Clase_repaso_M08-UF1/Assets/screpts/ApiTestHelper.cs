using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApiTestHelper : MonoBehaviour
{
    [Header("APi call setup")]
    public string url = "";
    public Dictionary<string, string> parameters = new();

    [Header("Objects")]
    public TextMeshProUGUI resultField;
    public RawImage imageField;

    public void MakeApiCall()
    {

        IEnumerator apiCall = ApiHelper.Get(url, parameters, OnSucces, onFailuer);
        resultField.text = "In progres";

        StartCoroutine(apiCall);
    }

    private void onFailuer(Exception exception)
    {
        resultField.text = "call error:" + "<br>" + exception.Message;
    }

    private void OnSucces(string result) { 
        resultField.text = result;
    }

    //pokemon parse

    [Serializable]
    public class Pokemon
    {
        public string name;
        public List<SlotType> types;
        public Sprites sprites;

        [Serializable]
        public class SlotType
        {
            public int Slot;
            public Type type;

            [Serializable]
            public class Type
            {
                public string name;
            }
        }
        [Serializable]
        public class Sprites
        {
            public string front_default;
        }
    }

    public void MakePokemonApiCall()
    {
        IEnumerator apicall = ApiHelper.Get<Pokemon>(url, parameters, onPokemonSucces, OnPokemonFailure);
        resultField.text = "In progres";

        StartCoroutine(apicall);   

    }
    public void OnPokemonFailure(Exception expectation)
    {
        resultField.text = "CAll Error:" + "<br>" + expectation.Message;
    }
    public void onPokemonSucces(Pokemon result)
    {
        resultField.text = "Name: " + result.name;
       

        foreach (Pokemon.SlotType type in result.types)
        {
            resultField.text += "<br>Type:";
            resultField.text += " " + type.type.name;
        }

        resultField.text += "<br>ImageURL:<br>" + result.sprites.front_default;

        IEnumerator imagApiCall = ApiHelper.GetTexture(result.sprites.front_default, OnImagSuccess, onImagFeilure);
        StartCoroutine(imagApiCall);

    }
    private void onImagFeilure(Exception exeption)
    {
        imageField.texture = null;
        imageField.color = Color.red;
    }

    private void OnImagSuccess(Texture texture)
    {

        imageField.texture = texture;
    }

}
