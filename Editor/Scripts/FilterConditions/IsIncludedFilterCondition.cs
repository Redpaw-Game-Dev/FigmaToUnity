namespace LazyRedpaw.FigmaToUnity
{
    public class IsIncludedFilterCondition : BoolFilterCondition
    {
        public IsIncludedFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsBoolConditionTrue(RequestImageData imageData)
        {
            return imageData.IsIncluded;
        }
    }
}