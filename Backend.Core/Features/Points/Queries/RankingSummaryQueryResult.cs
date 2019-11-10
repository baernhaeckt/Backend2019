namespace Backend.Core.Features.Points.Queries
{
    public class RankingSummaryQueryResult
    {
        public RankingSummaryQueryResult(long localRank, long globalRank, long friendRank)
        {
            LocalRank = localRank;
            GlobalRank = globalRank;
            FriendRank = friendRank;
        }

        public long LocalRank { get; }

        public long GlobalRank { get; }

        public long FriendRank { get; }
    }
}