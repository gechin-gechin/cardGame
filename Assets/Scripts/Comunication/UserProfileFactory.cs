using System.Collections;
using System.Collections.Generic;
using DB;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class UserProfileFactory
{
    public static void Create(SqliteDatabase db){
        string query = "create table if not exists user_profile (user_id text, user_name text, primary key(user_id))";
        db.ExecuteQuery(query);
    }

    public static void Set(string _id, string _name){
        string query = "insert or replace into user_profile (user_id, user_name) values (\""
                        + _id + "\",\""+_name+"\")";
        var db = SQLiteManager.I.sqlDB;
        db.ExecuteQuery(query);
    }

    public static void Get(string _id){
        string query = "select * from user_profile where user_id = "+_id;
        var db = SQLiteManager.I.sqlDB;
        var table = db.ExecuteQuery(query);
        foreach(DataRow r in table.Rows){
            Debug.Log(r["user_id"].ToString());
            Debug.Log(r["user_name"].ToString());
        }
    }
}
