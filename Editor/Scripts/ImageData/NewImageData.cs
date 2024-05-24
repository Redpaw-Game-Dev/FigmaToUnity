namespace LazyRedpaw.FigmaToUnity
{
    public class NewImageData : RequestImageData
    {
        public override bool IsIncluded => true;

        public NewImageData() { }
        
        public override void UpdateAssetPath(string savePath)
        {
            if (string.IsNullOrEmpty(_name)) _name = "NONAME_IMAGE";
            base.UpdateAssetPath(savePath);
        }
    }
}