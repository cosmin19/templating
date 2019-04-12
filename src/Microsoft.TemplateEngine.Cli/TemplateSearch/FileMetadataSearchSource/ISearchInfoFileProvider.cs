using Microsoft.TemplateEngine.Edge;

namespace Microsoft.TemplateEngine.Cli.TemplateSearch.FileMetadataSearchSource
{
    internal interface ISearchInfoFileProvider
    {
        /// <summary>
        /// Sets up the search metadata file.
        /// The provider can get it however is appropriate. The file must be placed in the input metadataFileTargetLocation
        /// </summary>
        /// <param name="paths">A Paths instance, so the abstracted file system operations are available.</param>
        /// <param name="metadataFileTargetLocation">The expected location of the metadata file, after this is run.</param>
        /// <returns></returns>
        bool TryEnsureSearchFile(Paths paths, string metadataFileTargetLocation);
    }
}