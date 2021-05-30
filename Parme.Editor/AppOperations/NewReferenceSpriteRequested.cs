namespace Parme.Editor.AppOperations
{
    public class NewReferenceSpriteRequested : IAppOperation
    {
        private readonly string _referenceSpriteFilename;

        public NewReferenceSpriteRequested(string referenceSpriteFilename)
        {
            _referenceSpriteFilename = referenceSpriteFilename;
        }

        public AppOperationResult Run()
        {
            return new AppOperationResult
            {
                SelectedReferenceSpriteChanged = true,
                SelectedReferenceSpriteFileName = _referenceSpriteFilename,
            };
        }
    }
}