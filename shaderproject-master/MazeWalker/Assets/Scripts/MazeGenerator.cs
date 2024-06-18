using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public Texture2D mazeImage;
    public GameObject wallPrefab;
    public GameObject groundPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze(mazeImage);
    }

    private void GenerateMaze(Texture2D mazeImage)
    {
        var pixels = mazeImage.GetPixels();

        bool IsFloor(int x, int y) {
            if (x < 0 || x >= mazeImage.width || y < 0 || y >= mazeImage.height)
                return false;
            return pixels[x + y * mazeImage.width] != Color.black;
        }

        for (int y = 0; y < mazeImage.height; y++) {
            for (int x = 0; x < mazeImage.width; x++) {
                var i = x + y * mazeImage.width;

                var c = pixels[i];

                if (c != Color.black) {
                    var floor = GameObject.Instantiate(groundPrefab, new Vector3(x * 2, 0, y * 2), Quaternion.identity, transform);
                }
                else if (IsFloor(x - 1, y - 1) ||
                        IsFloor(x - 1, y    ) ||
                        IsFloor(x - 1, y + 1) ||
                        IsFloor(x    , y - 1) ||
                        IsFloor(x    , y + 1) ||
                        IsFloor(x + 1, y - 1) ||
                        IsFloor(x + 1, y    ) ||
                        IsFloor(x + 1, y + 1))
                {

                    var wall = GameObject.Instantiate(wallPrefab, new Vector3(x * 2, 0, y * 2), Quaternion.identity, transform);
                }
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
