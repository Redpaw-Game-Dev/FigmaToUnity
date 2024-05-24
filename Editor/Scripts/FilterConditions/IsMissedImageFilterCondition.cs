namespace LazyRedpaw.FigmaToUnity
{
    public class IsMissedImageFilterCondition : BoolFilterCondition
    {
        public IsMissedImageFilterCondition(string inspectorName) : base(inspectorName) { }
        protected override bool IsBoolConditionTrue(RequestImageData imageData)
        {
            return imageData is UpdateImageData updateImageData && updateImageData.Image == null;
        }
    }
}