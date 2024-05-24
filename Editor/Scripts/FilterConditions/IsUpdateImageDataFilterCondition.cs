namespace LazyRedpaw.FigmaToUnity
{
    public class IsUpdateImageDataFilterCondition : BoolFilterCondition
    {
        public IsUpdateImageDataFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsBoolConditionTrue(RequestImageData imageData)
        {
            return imageData is UpdateImageData;
        }
    }
}