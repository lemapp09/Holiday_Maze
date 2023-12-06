using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace HolidayMaze
{
    public class MazeRenderer : MonoSingleton<MazeRenderer>
    {
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private GameObject mazeCellPrefab;
        [SerializeField] private GameObject suzie;
        private Vector3 suziePosition;
        private Quaternion suzieRotation;
        public int mazeWidth, mazeHeight ;

        // This is the physical size of our maze cells. Getting this wrong will result in overlapping
        // or visible gaps between each cell.
        public float CellSize = 3f;
        private int level = 0;

        private void Start()
        {
            suziePosition = suzie.transform.position;
            suzieRotation = suzie.transform.localRotation;
            BeginLevel();
        }

        public void BeginLevel()
        {
            if (level == 10) {
                UIManager.Instance.GameWon();
                StartCoroutine(EndGame());
            }
            level++;
            UIManager.Instance.updateLevel(level);
            suzie.transform.position = suziePosition;
            suzie.transform.localRotation =  suzieRotation;
            mazeWidth = 5 + level;
            mazeHeight = 5 + level ;
            // Get our MazeGenerator script to make us  maze
            MazeCell[,] maze = new MazeCell[1, 1];
            Debug.Log("Maze length: " +maze.Length);
            ClearMaze(); // Clear the old maze before generating a new one

            maze = mazeGenerator.GetMaze(mazeWidth, mazeHeight);
            // Random Select where the Tree Exit will be located
            Vector2Int treeSpace = new Vector2Int(UnityEngine.Random.Range(0, mazeWidth),
                UnityEngine.Random.Range(0, mazeHeight));
            
            // Loop through every cell in the maze.
            for (int x = 0; x < mazeWidth; x++) {
                for (int y = 0; y < mazeHeight; y++)
                {
                    // Instantiate a new maze cell as a child of the MazeRenderer object.
                    GameObject newCell = Instantiate(mazeCellPrefab, new Vector3((float)x * CellSize, 0f,
                        (float)y * CellSize), quaternion.identity, transform);
                    
                    // Get a reference to the cell's MazeCellPrefab script.
                    MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();
                    
                    // Determine which walls need to be active.
                    bool top = maze[x, y].topWall;
                    bool left = maze[x, y].leftWall;
                    
                    // Place Tree
                    bool tree = treeSpace.x == x && treeSpace.y == y;

                    // Bottom and right walls are deactivated by default unless we are at the bottom or right
                    // edges of ethe maze.
                    bool right = false;
                    bool bottom = false;
                    if (x == mazeWidth - 1) right = true;
                    if (y == 0) bottom = true;
                    mazeCell.Init(top, bottom, right, left, tree, level);
                }
            }
        }

        private IEnumerator EndGame()
        {
            yield return new WaitForSeconds(15);
            
            // If we are running in a standalone build of the game
#if UNITY_STANDALONE
            // Quit the application
            Application.Quit();
#endif

            // If we are running in the editor
#if UNITY_EDITOR
            // Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void ClearMaze() { 
            foreach (Transform child in transform) {
                Destroy(child.gameObject); 
            } 
        }
    }
}