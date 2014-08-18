using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JekyllPostGenerator
{
    public class PostService
    {
        private static Repository _repo;
        private static string _repoPath;
        public PostService(string path)
        {
            if (IsValidPath(path))
            {
                _repo = new Repository(path);
                _repoPath = path;
            }
            else
            {
                throw new InvalidOperationException("Path is not valid git directory.");
            }
        }

        private static bool IsValidPath(string path)
        {
            return Repository.IsValid(path);
        }

        public async Task NewPost(string postName, string applicationExecutablePath)
        {
            var postsPath = string.Empty;

            if (!_repoPath.EndsWith("\\"))
            {
                postsPath = string.Format("{0}\\_posts\\", _repoPath);
            }
            else
            {
                postsPath = _repoPath + "_posts\\";
            }

            var postFileName = string.Format("{0}-{1}.md", DateTime.Now.ToString("yyyy-MM-dd"), postName.Replace(' ', '-'));
            using (var file = File.CreateText(string.Format("{0}{1}", postsPath, postFileName)))
            {
                await file.WriteAsync("test");
            }
        }
    }
}
