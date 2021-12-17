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

        private Dictionary<string, AchievementInfo> achievements;

        public AchievementSound()
        {
            scrapingBrowser = new ScrapingBrowser();
            achievements = new Dictionary<string, AchievementInfo>();
        }

        public void Run()
        {
            HtmlNode html = GetHtml("https://steamcommunity.com/id/xShadoWalkeR/stats/appid/12210/achievements");
            GetAchievements(html);

            foreach(AchievementInfo info in achievements.Values)
            {
                Console.WriteLine(info.ToString());
                Console.WriteLine();
            }
        }

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
