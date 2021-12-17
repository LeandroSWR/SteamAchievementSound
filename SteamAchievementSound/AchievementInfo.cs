using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAchievementSound
{
    class AchievementInfo
    {
        public string Title { get; }

        public string Description { get; }

        public bool IsUnlocked { get; }

        public string UnlockDate { get; }

        public AchievementInfo(string title, string description, bool unlocked, string unlockDate = "")
        {
            Title = title;
            Description = description;
            IsUnlocked = unlocked;
            UnlockDate = unlockDate;
        }

        public override string ToString()
        {
            if (IsUnlocked)
            {
                return string.Format("Name: {0}\nDescription: {1}\nUnlocked:{2}\nUnlock Date: {3}",
                    Title, Description, IsUnlocked, UnlockDate);
            }
            else
            {
                return string.Format("Name: {0}\nDescription: {1}\nUnlocked:{2}", 
                    Title, Description, IsUnlocked);
            }
            
        }
    }
}
