using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.XPath;

namespace SteamAchievementSound
{
    class GamesManager
    {
        private Dictionary<string, string> games;

        private string userID;

        public GamesManager(string userID)
        {
            this.userID = userID;

            games = new Dictionary<string, string>();

            GetGamesList();
        }

        public string GetAppID(string name) => games[name];

        private void GetGamesList()
        {
            byte[] bytes;
            using (WebClient webClient = new WebClient())
            {
                bytes = webClient.DownloadData($"https://steamcommunity.com/profiles/{userID}/games/?tab=all&xml=1");
            }

            using (MemoryStream stream = new MemoryStream(bytes, false))
            {
                XPathDocument document = new XPathDocument(stream);
                XPathNavigator navigator = document.CreateNavigator();
                var nodes = navigator.Select("/gamesList/games/game");

                // For testing purposes
                games.Add("Spacewar", "480");

                while (nodes.MoveNext())
                {
                    if (!games.ContainsKey(nodes.Current.SelectSingleNode("name").Value))
                    {
                        games.Add(nodes.Current.SelectSingleNode("name").Value, nodes.Current.SelectSingleNode("appID").Value);
                    }
                }
            }
        }
    }
}
