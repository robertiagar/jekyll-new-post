using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JekyllPostGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var repoPath = args[0];
            var postTitle = args[1];


            var postService = new PostService(repoPath);

            postService.NewPost("test post", string.Empty).Wait();
        }
    }
}
