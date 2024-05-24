namespace LazyRedpaw.FigmaToUnity
{
    public class IsNewImageDataFilterCondition : BoolFilterCondition
    {
        public IsNewImageDataFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsBoolConditionTrue(RequestImageData imageData)
        {
            return imageData is NewImageData;
        }
    }
}