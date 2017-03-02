using System.Collections.Generic;

namespace UI.Modules.FineUploader.Models
{
    public class FineUploaderConfig
    {
        public string ElementID { get; }
        public string TemplateID => ElementID + "_template";
        public IList<string> AllowedExtensions { get; }
        public int LimitFilesCount { get; }
        public int MaxFileSizeBytes { get; }
        public string UploadFileActionUrl { get; }

        public FineUploaderConfig(string elementID, string uploadFileActionUrl, IList<string> allowedExtensions, int limitFilesCount, int maxFileSizeBytes)
        {
            ElementID = elementID;
            AllowedExtensions = allowedExtensions;
            LimitFilesCount = limitFilesCount;
            MaxFileSizeBytes = maxFileSizeBytes;
            UploadFileActionUrl = uploadFileActionUrl;
        }
    }
}
