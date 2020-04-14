using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Tile_Map_Manager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_Grid;

    [SerializeField]
    GameObject m_Cell;

    [SerializeField]
    List<Material> m_Tiles; 

    private void Start()
    {
        m_LoadMapFromTextFile("Assets/Resources/Level_One.txt"); 
    }

    public void m_CreateTileMap(int rows, int columns)
    {
        float l_fSpacing = 1.0f; 

        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;
                m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));
            }
        }

        for(int k = 0; k < m_Grid.Count; k++)
        {
            m_Grid[k].transform.parent = gameObject.transform;
        }
    }

    public void m_CreateTileMap(int rows, int columns, string[] tileConfig)
    {
        float l_fSpacing = 1.0f;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 l_Pos = new Vector3(i, j, 0) * l_fSpacing;

                m_Grid.Add(Instantiate(m_Cell, l_Pos, Quaternion.identity));
            }
        }

        for (int k = 0; k < m_Grid.Count; k++)
        {
            m_Grid[k].transform.parent = gameObject.transform;

            m_Grid[k].GetComponent<Cell_Manager>().m_SetTile(CellTile.none, m_Tiles[int.Parse(tileConfig[k])]);
        }
    }

    public void m_LoadMapFromTextFile(string filePath)
    {
        string l_Path = filePath;

        int l_Rows, l_Columns;

        // Create Reader and begin parsing file. 

        StreamReader reader = new StreamReader(l_Path);

        l_Rows = int.Parse(reader.ReadLine());

        l_Columns = int.Parse(reader.ReadLine());

        // Collect cell data from the file. 

        string[] l_GridConfig;

        string l_Text = reader.ReadLine();

        do
        {
            Debug.Log(l_Text);

            l_GridConfig = l_Text.Split(',');

            l_Text = reader.ReadLine();

        } while (l_Text != null);

        m_CreateTileMap(l_Rows, l_Columns, l_GridConfig);

        reader.Close();
    }

}
