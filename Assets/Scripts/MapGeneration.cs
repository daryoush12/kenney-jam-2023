using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace KennyJam
{

    public enum GameTileFlags { Wall = 1, Floor = 0, Door = 3 };

    public class MapGeneration : MonoBehaviour
    {
        public float largeCaveFrequency = 0.05F;
        public float squiggleCaveFrequency = 0.2F;

        public float seed;

        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int[,] _mapArray;

        [SerializeField] private TileBase _base;
        [SerializeField] private TileBase _empty;
        [SerializeField] private Tilemap _tlMap;
        [SerializeField] private Tilemap _floorMap;

        [SerializeField] private Texture2D _noiseText;

        /// <summary>
        /// Generates Empty world map.
        /// 
        /// 
        /// 1 = Contains Rock
        /// 0 = Empty
        /// 
        ///</summary>
        public void CreateArray()
        {
            _mapArray = new int[_width, _height];

            for (int x = 0; x < _mapArray.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _mapArray.GetUpperBound(1); y++)
                {
                    _mapArray[x, y] = (int)GameTileFlags.Floor;
                }
            }
        }

        public void PerlinNoise()
        {
            for (int x = 0; x < _mapArray.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _mapArray.GetUpperBound(1); y++)
                {
                    float p = Mathf.PerlinNoise((x + seed) * largeCaveFrequency, (y + seed) * largeCaveFrequency);
                    if (p < 0.5F)
                    {
                        _mapArray[x, y] = (int)GameTileFlags.Wall;
                    }
                    else
                    {
                        _mapArray[x, y] = (int)GameTileFlags.Floor;
                    }
                }
            }
        }

        public void RenderMap()
        {
            _tlMap.ClearAllTiles();
            for (int x = 0; x < _mapArray.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _mapArray.GetUpperBound(1); y++)
                {
                    switch (_mapArray[x, y])
                    {
                        case (int)GameTileFlags.Wall:
                            _tlMap.SetTile(new Vector3Int(x, y, 0), _base);
                            break;
                        case (int)GameTileFlags.Floor:
                            _floorMap.SetTile(new Vector3Int(x, y, 0), _empty);
                            break;
                        default:
                            _tlMap.SetTile(new Vector3Int(x, y, 0), _empty);
                            break;
                    }
                }
            }
        }

        public void UpdateMap()
        {
            for (int x = 0; x < _mapArray.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _mapArray.GetUpperBound(1); y++)
                {
                    if (_mapArray[x, y] == 1)
                    {
                        _tlMap.SetTile(new Vector3Int(x, y, 0), _base);
                    }
                    else
                    {

                    }
                }
            }
        }

        private void Start()
        {
            //CreateArray();
            //PerlinNoise();
            //RenderMap();


            CreateArray();
            PerlinNoise();
            SetBounds(2);
            //GenerateNoise();
            RenderMap();

        }

        private void GenerateNoise()
        {
            _noiseText = new Texture2D(_width, _height);
            for (int x = 0; x < _mapArray.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _mapArray.GetUpperBound(1); y++)
                {
                    float p = Mathf.PerlinNoise((x + seed) * largeCaveFrequency, (y + seed) * largeCaveFrequency);
                    _noiseText.SetPixel(x, y, new Color(p, p, p));
                }
            }

            _noiseText.Apply();
        }


        private void SetBounds(int h = 2)
        {

            for (int t = 0; t < h; t++)
            {
                for (int x = 0; x < _mapArray.GetUpperBound(0); x++)
                {
                    _mapArray[x, t] = (int)GameTileFlags.Wall;
                    _mapArray[x, (_mapArray.GetUpperBound(1) - 1) - t] = (int)GameTileFlags.Wall;
                }

                for (int y = 0; y < _mapArray.GetUpperBound(1); y++)
                {
                    _mapArray[t, y] = (int)GameTileFlags.Wall;

                    _mapArray[(_mapArray.GetUpperBound(0) - 1) - t, y] = (int)GameTileFlags.Wall;
                }
            }
        }
    }
}