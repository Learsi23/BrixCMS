

namespace TestBrixCMS.Areas.Manager.Models
{
    public class MediaViewModel
    {
        public List<string> Folders { get; set; } = new List<string>();
        public List<string> Files { get; set; } = new List<string>();
        /// <summary>All folders recursively (relative paths), including root as ""</summary>
        public List<string> AllFolders { get; set; } = new List<string>();
    }
}
