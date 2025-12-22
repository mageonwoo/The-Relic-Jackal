namespace Gameplay.Board.PathCalculator
{
    // 이동 계산에 사용되는 cost 예산 컨테이너
    public class MoveCostBudget
    {
        //기본 cost
        public int BaseCost { get; private set; }
        //외부 요인에 의해 조정된 cost
        public int AdjustedCost { get; private set; }
        //실제 이동에 사용되는 cost
        public int CurrentCost => BaseCost + AdjustedCost;

        public MoveCostBudget(int _baseCost)
        {
            BaseCost = _baseCost;
            AdjustedCost = 0;
        }

        public void AddCostAdjustment(int delta)
        {
            AdjustedCost += delta;
        }

        public void ResetCostAdjustment()
        {
            AdjustedCost = 0;
        }
    }
}