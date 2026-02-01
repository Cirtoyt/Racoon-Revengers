using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DrawPad : MonoBehaviour
{

    [SerializeField]
    private List<InputActionReference> actionReferences;

    private List<Vector2> vectors;

    public int borders = 5;

    Vector2 currentDraw;
    Texture2D texture;

    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUpTexture();

        vectors = new List<Vector2>(actionReferences.Count);
        SetUpVectors();

        currentDraw = new Vector2(texture.width/2, texture.height/2);
    }

    private void SetUpTexture()
    {
        Material material = GetComponent<Renderer>().material;
        if (material == null)
        {
            Debug.LogError("No material found for drawpad");
            return;
        }
        int width = 256;
        int height = 256;
        Texture startTexture = material.mainTexture;
        if (startTexture == null)
        {
            Debug.LogError("No main texture found for drawpad material");
        }
        else
        {
            width = startTexture.width;
            height = startTexture.height;
        }
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Material newMaterial = new Material(material);
        newMaterial.mainTexture = texture;
        GetComponent<Renderer>().material = newMaterial;
        //materials[0].SetTexture("main", texture);
        texture.Apply();
    }

    void SetUpVectors()
    {
        if(vectors.Capacity == 0)
        {
            return;
        }
        int numDirections = vectors.Capacity;

        float degrees = 360.0f / numDirections;
        for (int i = 0; i < numDirections; i++)
        {
            float radians = degrees * i * Mathf.Deg2Rad;
            Vector2 newVector = new Vector2((float)Mathf.Sin(radians), (float)Mathf.Cos(radians));
            vectors.Add(newVector);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (texture == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer < 30)
        {
            currentDraw += GetInputs();
            currentDraw.x = Mathf.Clamp(currentDraw.x, borders, texture.width - borders);
            currentDraw.y = Mathf.Clamp(currentDraw.y, borders, texture.height - borders);
            drawPixels3By3(currentDraw);
            texture.Apply();
        }
        else if (timer > 40)
        {
            SceneManager.LoadScene(0);
        }
    }

    void drawPixels3By3(Vector2 location)
    {
        int startLocationX = (int)location.x;
        int startLocationY = (int)location.y;
        int currentX = -1;
        int currentY = -1;
        for(int i = 0; i < 9; ++i)
        {
            texture.SetPixel(startLocationX + currentX++, startLocationY + currentY, Color.red);
            if(currentX == 2)
            {
                currentY++;
                currentX = -1;
            }
        }

    }

    Vector2 GetInputs()
    {
        Vector2 returnVector = new Vector2(0, 0);
        for(int i = 0; i < actionReferences.Capacity; ++i)
        {
            if (actionReferences[i] != null)
            {
                if (actionReferences[i].action.IsPressed())
                {
                    returnVector -= vectors[i];
                }
            }
        }
        return returnVector.normalized;
    }
}
