using UnityEngine;

namespace HolidayMaze
{
   public class MazeCellObject : MonoBehaviour
   {
      [SerializeField] private GameObject topWall, bottomWall, rightWall, leftWall;
      [SerializeField] private GameObject[] Lights, Gifts;
      [SerializeField] private GameObject Tree, Snowman;

      public void Init(bool top, bool bottom, bool right, bool left, bool tree, int level )
      {
         topWall.SetActive(top);
         bottomWall.SetActive(bottom);
         rightWall.SetActive(right);
         leftWall.SetActive(left);
         Lights[Random.Range(0,Lights.Length)].SetActive(true);
         if(Random.Range(0,10) < 5) Gifts[Random.Range(0,Gifts.Length)].SetActive(true);
         if(Random.Range(0,100 - (level * 9)) == 1) Snowman.SetActive(true);
         if (tree) Tree.SetActive(true);
      }
   }
}