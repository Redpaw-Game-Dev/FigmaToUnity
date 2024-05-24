namespace LazyRedpaw.FigmaToUnity
{
    public class IsExcludedFilterCondition : BoolFilterCondition
    {
        public IsExcludedFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsBoolConditionTrue(RequestImageData imageData)
        {
            return !imageData.IsIncluded;
        }
    }
}