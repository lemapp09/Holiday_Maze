using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HolidayMaze
{
    public class MazeGenerator : MonoBehaviour
    {
        private int _mazeWidth = 5, _mazeHeight = 5; // The dimensions of the Maze
        public int startX, startY; // The position our algorithm will start from.
        private MazeCell[,] maze;
        private Vector2Int currentCell;

        public MazeCell[,] GetMaze(int mazeWidth, int mazeHeight)
        {
            _mazeWidth = mazeWidth;
            _mazeHeight = mazeHeight;
            maze = new MazeCell[mazeWidth, mazeHeight];
            for (int x = 0; x < mazeWidth; x++)
            {
                for (int y = 0; y < mazeHeight; y++)
                {
                    maze[x, y] = new MazeCell(x, y);
                }
            }
            CarvePath(startX, startY);
            return maze;
        }

        List<Direction> directions = new List<Direction>
        {
            Direction.Up, Direction.Down, Direction.Left, Direction.Right,
        };

        List<Direction> GetRandomDirections()
        {
            // Make a copy of our Direrction list that we can mess around with.
            List<Direction> dir = new List<Direction>(directions);

            // Make a directions list to put our directions into.
            List<Direction> rndDir = new List<Direction>();
            while (dir.Count > 0) //  Loop until our rndDir list is empty
            {
                int rnd = Random.Range(0, dir.Count); // Get random index in list
                rndDir.Add(dir[rnd]); // Add the random direction to our list
                dir.RemoveAt(rnd); // Remove that direction so we can't choose it again
            }

            // When we've got all four directions in a random order, return the queue.
            return rndDir;
        }

        bool IsCellValid(int x, int y)
        {
            if (x < 0 || y < 0 || x > _mazeWidth - 1 || y > _mazeHeight - 1 || maze[x, y].visited) return false;
            else return true;
        }

        Vector2Int CheckNeighbors()
        {
            List<Direction> rndDir = GetRandomDirections();
            for (int i = 0; i < rndDir.Count; i++)
            {
                // Set neighbor coordinates to current cell for now.
                Vector2Int neighbor = currentCell;
                switch (rndDir[i])
                {
                    case Direction.Up:
                        neighbor.y++;
                        break;
                    case Direction.Down:
                        neighbor.y--;
                        break;
                    case Direction.Left:
                        neighbor.x--;
                        break;
                    case Direction.Right:
                        neighbor.x++;
                        break;

                }

                // if the neighbor we just tried is valid, we can return that neighbor. If not, we go again.
                if (IsCellValid(neighbor.x, neighbor.y))
                {
                    return neighbor;
                }
            }

            return currentCell;
        }

        void BreakWalls(Vector2Int primaryCell, Vector2Int secondaryCell)
        {
            if (primaryCell.x > secondaryCell.x) { // Primary Cell's Left Wall
                maze[primaryCell.x, primaryCell.y].leftWall = false;
            }  else if (primaryCell.x < secondaryCell.x) { // Secondary Cell's Left Wall
                maze[secondaryCell.x, secondaryCell.y].leftWall = false;
            }  else if (primaryCell.y < secondaryCell.y) { // Primary Cell's Top Wall
                maze[primaryCell.x, primaryCell.y].topWall = false;
            }  else if (primaryCell.y > secondaryCell.y) { // Secondary Cell's Top Wall
                maze[secondaryCell.x, secondaryCell.y].topWall = false;
            }
        }

        // Starting at the x, y passed in, carves a path through the maze until it encounters a "dead end"
        // )a dead end is a cell with no valid neighbors.)
        void CarvePath(int x, int y) {
            // Perform a quick check to make sure our start position is within the boundaries of the map,
            // If noy, set them to a default (I'm using 0) and throw in a little warning.
            if (x < 0 || y < 0 || x > _mazeWidth - 1 || y > _mazeHeight - 1) {
                x = y = 0;
                Debug.LogWarning("Starting Position is out of bounds, defaulting to 0,0");
            }
            // Set current cell to the starting position we were passed.
            currentCell = new Vector2Int(x, y);
            
            // A list to keep track of our current path.
            List<Vector2Int> path = new List<Vector2Int>();
            
            // Loop until we encounter a dead end.
            bool deadEnd = false;
            while (!deadEnd)
            {
                // Get the next cell we're going to try
                Vector2Int nextCell = CheckNeighbors();
                
                // If that cell has no valid neighbors, set deadend to true so we break out of loop
                if (nextCell == currentCell)
                {
                    // if that cell has no valid neighbors, set deadend to true so we break out of loop.
                    for (int i = path.Count - 1; i >= 0; i--)
                    {
                        currentCell = path[i];          // Set currentCell to the next step back along the path
                        path.RemoveAt(i);               // Remove this step from the path
                        nextCell = CheckNeighbors();    // Check that cell to see if any other neighbors are valid
                        
                        // If we find a valid neighbor, break out of loop.
                        if (nextCell != currentCell) break;
                    }

                    if (nextCell == currentCell) deadEnd = true;
                }  else  {
                    BreakWalls(currentCell, nextCell);  // Set wall flags on these two cells.
                    maze[currentCell.x, currentCell.y].visited = true;       // Set cell to visited before moving on.
                    currentCell = nextCell;                                  // Set the current cell to the valid neighbor we found.
                    path.Add(currentCell);                                   //  Add this cell to our path
                }
            }
        }
        
    }
    
    public enum Direction
    {
        Up, Down, Left, Right
    }

    public class MazeCell
    {
        public bool visited;
        public int x, y;
        public bool topWall, leftWall;

        //Return x and y as a Vector2Int for convinence sake.
        public Vector2Int position
        {
            get { return new Vector2Int(x, y); }
        }


        public MazeCell(int x, int y)
        {
            // The coordinates of this cell in the maze grid
            this.x = x;
            this.y = y;
            
            //Whether the algorithm has visited this cell or not - flase to start
            visited = false;
            
            //All walls are present until the algorithm removes them.
            topWall = leftWall = true;
        }
    }
}