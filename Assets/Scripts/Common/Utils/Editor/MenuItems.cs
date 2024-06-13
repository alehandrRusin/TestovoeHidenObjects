using Common.Database;
using UnityEditor;
using UnityEngine;

namespace Common.Utils.Editor
{
    public class MenuItems
    {
        [MenuItem("Custom/Reset")]
        public static void Reset()
        {
            PlayerPrefs.DeleteAll();

            DataBase dataBase = new DataBase();
            dataBase.CreateTable();
            dataBase.DropTable();

            dataBase = null;
        }
    }
}