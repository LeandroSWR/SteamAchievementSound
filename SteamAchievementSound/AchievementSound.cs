using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace SteamAchievementSound
{
    class AchievementSound
    {
        private ScrapingBrowser scrapingBrowser;

        private GamesManager gamesManager;

        private Dictionary<string, AchievementInfo> achievements;

        private string userID;
        private string appID;

        public AchievementSound(string userID)
        {
            scrapingBrowser = new ScrapingBrowser();
            gamesManager = new GamesManager(userID);
            achievements = new Dictionary<string, AchievementInfo>();

            this.userID = userID;
        }

        public void Run()
        {
            HtmlNode html = GetHtml($"https://steamcommunity.com/profiles/{userID}");

            appID = gamesManager.GetAppID(GetGameName(html));

            html = GetHtml($"https://steamcommunity.com/profiles/{userID}/stats/appid/{appID}/achievements");
            GetAchievements(html);

            foreach(AchievementInfo info in achievements.Values)
            {
                Console.WriteLine(info.ToString());
                Console.WriteLine();
            }
        }

        private string GetGameName(HtmlNode html) => 
            html.CssSelect("div.profile_in_game_name").First().InnerText;

        private void GetAchievements(HtmlNode html)
        {
            List<HtmlNode> achieveTexts = html.CssSelect("div.achieveTxtHolder").ToList();

            int numCompletedAchievements = GetNumUnlockedAchievements(html);

            for (int i = 0; i < achieveTexts.Count; i++)
            {
                if (i < numCompletedAchievements)
                {
                    achievements.Add(
                        achieveTexts[i].SelectSingleNode("div/h3").InnerText,
                        new AchievementInfo(
                            achieveTexts[i].SelectSingleNode("div/h3").InnerText, 
                            achieveTexts[i].SelectSingleNode("div/h5").InnerText, true,
                            achieveTexts[i].LastChild.PreviousSibling.FirstChild.InnerText.Remove(0, 8)));
                }
                else
                {
                    achievements.Add(
                        achieveTexts[i].SelectSingleNode("div/h3").InnerText,
                        new AchievementInfo(
                            achieveTexts[i].SelectSingleNode("div/h3").InnerText,
                            achieveTexts[i].SelectSingleNode("div/h5").InnerText, false));
                }
            }
        }

        private int GetNumUnlockedAchievements(HtmlNode html) =>
            html.CssSelect("div.achieveUnlockTime").Count();

        private HtmlNode GetHtml(string url)
        {
            WebPage webPage = scrapingBrowser.NavigateToPage(new Uri(url));
            return webPage.Html;
        }
    }
}
