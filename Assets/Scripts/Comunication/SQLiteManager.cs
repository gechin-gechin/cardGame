using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace DB{
    public class SQLiteManager : Singleton<SQLiteManager>
    {
        public SqliteDatabase sqlDB {get;private set;}
        private const string dbname="service.db";
        protected override void AltAwake()
        {
            string dbPath=Application.persistentDataPath+"/service.db";
            if(!File.Exists(dbPath)){
                File.Create(dbPath);
            }
            
            sqlDB = new SqliteDatabase(dbname);
            UserProfileFactory.Create(sqlDB);
            Debug.Log(sqlDB);
        }
    }
}
