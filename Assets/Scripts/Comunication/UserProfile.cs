using System;

[Serializable]
public class UserProfile
{
    public string user_id;
    public string user_name;
    public UserProfile(string _id,string _name){
        this.user_id=_id;
        this.user_name=_name;
    }
}
