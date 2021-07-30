using System;
using System.Collections.Generic;

public class GamePlayModel
{
    [Serializable]
    public class HomeData
    {
        public GameData GameData;
        public UserData UserData;

        public HomeData()
        {
            this.GameData = new GameData();
            this.UserData = new UserData();
        }

        public HomeData(GameData GameData, UserData UserData)
        {
            this.GameData = GameData;
            this.UserData = UserData;
        }
    }

    [Serializable]
    public class GameData
    {
        public int LoginRuby;
        public string Resource;

        public GameData()
        {
            this.LoginRuby = 0;
            this.Resource = "";
        }

        public GameData(int LoginRuby, string Resource)
        {
            this.LoginRuby = LoginRuby;
            this.Resource = Resource;
        }
    }

    [Serializable]
    public class UserData
    {
        public string UserName;
        public int UserPlayScore;
        public int UserRuby;
        public string LastPlayTime;

        public UserData()
        {
            this.UserName = "";
            this.UserPlayScore = 0;
            this.UserRuby = 0;
            this.LastPlayTime = "";
        }

        public UserData(string UserName, int UserPlayScore, int UserRuby, string LastPlayTime)
        {
            this.UserName = UserName;
            this.UserPlayScore = UserPlayScore;
            this.UserRuby = UserRuby;
            this.LastPlayTime = LastPlayTime;
        }
    }

    [Serializable]
    public class ShopData
    {
        public UserData UserData;

        public ShopData()
        {
            UserData = new UserData();
        }
        public ShopData(UserData UserData)
        {
            this.UserData = UserData;
        }
    }

    [Serializable]
    public class SyncData
    {
        public UserData UserData;

        public SyncData()
        {
            UserData = new UserData();
        }
        public SyncData(UserData UserData)
        {
            this.UserData = UserData;
        }
    }
}
