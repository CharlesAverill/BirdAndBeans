using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    static Ground _instance;
    public static Ground Instance { get { return _instance; } }

    public Transform rootTileTransform;
    public GameObject tilePrefab;

    public int numTiles;
    public Tile[] tiles;

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

        for(int i = 0; i < numTiles; i++)
        {
            Tile newTile = Instantiate(tilePrefab, rootTileTransform.position, Quaternion.identity).GetComponent<Tile>();

            newTile.transform.position += i * new Vector3(newTile.size, 0f, 0f);
            newTile.transform.parent = transform;
            newTile.transform.localScale = rootTileTransform.localScale;

            tiles[i] = newTile;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
