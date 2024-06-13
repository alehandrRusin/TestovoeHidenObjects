using System.Linq;
using SQLite4Unity3d;
#if !UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.IO;
#endif

namespace Common.Database
{
	public class DataBase
	{
		private const string DatabaseName = "Database.db";
		private SQLiteConnection _connection;

		public void CreateTable()
		{

#if UNITY_EDITOR
			var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb =
 new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb =
 Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb =
 Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb =
 Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif
        }

        var dbPath = filepath;
#endif
			_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

			_connection.CreateTable<DataBaseLevelData>();
		}

		public bool TryLoadData(int levelDataId, out DataBaseLevelData dataBaseLevelData)
		{
			dataBaseLevelData = _connection.Table<DataBaseLevelData>().FirstOrDefault(x => x.id == levelDataId);
			return dataBaseLevelData != null;
		}


		public void DropTable()
		{
			_connection.DropTable<DataBaseLevelData>();
		}

		public void Delete(string key)
		{
			_connection.Delete<DataBaseLevelData>(key);
		}

		public void SaveData(LevelData levelData)
		{
			var data = new DataBaseLevelData
			{
				id = levelData.id,
				counter = levelData.counter.Value
			};
			
			_connection.InsertOrReplace(data, typeof(DataBaseLevelData));
		}
	}
}
