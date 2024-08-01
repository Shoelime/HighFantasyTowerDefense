public static class ScoreCalculator
{
    public static int GetScore()
    {
        /// How to calculate score
        /// 5 stars = no gem was touched
        /// 4 stars = no gems were stolen
        /// 3 stars = only one gem stolen 
        /// 2 stars = 2 stolen, 2 remain untouched
        /// 1 star  = every other scenario

        IGemManager gemManager = Services.Get<IGemManager>();
        IGameManager gameManager = Services.Get<IGameManager>();

        int gemsAtBase = gemManager.GemCountAtBase;
        if (gemsAtBase == gameManager.GetLevelData.GemCount)
            return 5;

        int gemsStolen = gameManager.GetLevelData.GemCount - gemManager.AvailableGems.Count;
        if (gemsStolen == 0)
            return 4;

        if (gemsStolen == 1)
            return 3;

        if (gemsStolen < 3 && gemsAtBase >= 2)
            return 2;

        return 1;
    }
}
