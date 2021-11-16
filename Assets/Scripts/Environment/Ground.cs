using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ground : MonoBehaviour
{
    static Ground _instance;
    public static Ground Instance { get { return _instance; } }

    public Transform rootTileTransform;
    public GameObject tilePrefab;

    public int numTiles;
    public Tile[] tiles;
    List<int> emptyTiles;

    float tileSize = -1f;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tiles = new Tile[numTiles];
        emptyTiles = new List<int>();

        for(int i = 0; i < numTiles; i++)
        {
            addTile(i);
        }
    }

    Tile getNewTile(int index)
    {
        Tile newTile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity).GetComponent<Tile>();
        if(tileSize == -1f){
            tileSize = newTile.size;
        }

        newTile.index = index;
        newTile.transform.position = new Vector3(GetTileX(index), rootTileTransform.position.y, 0f);
        newTile.transform.parent = transform;
        newTile.transform.localScale = rootTileTransform.localScale;

        return newTile;
    }

    public float GetTileX(int index)
    {
        return rootTileTransform.position.x + index * tileSize;
    }

    void addTile(int index)
    {
        tiles[index] = getNewTile(index);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetEmptyTile()
    {
        for(int i = 0; i < numTiles; i++)
        {
            if(tiles[i] == null){
                return i;
            }
        }

        return -1;
    }

    public void AddEmptyTile(int index)
    {
        emptyTiles.Add(index);
    }

    public int GetRandomEmptyTile()
    {
        int index = -1;
        if(emptyTiles.Count > 0){
            index = emptyTiles[Random.Range(0, emptyTiles.Count)];
            emptyTiles.Remove(index);
        }

        return index;
    }

    public void FillEmptyTile(int index)
    {
        addTile(index);
        //emptyTiles.Remove(index);
    }

    public int NumEmptyTiles()
    {
        return emptyTiles.Count;
    }
}
