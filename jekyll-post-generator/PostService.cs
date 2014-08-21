using LibGit2Sharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            using (var defaultHeader = File.OpenText("default-header.json"))
            {
                var headerFormat = await defaultHeader.ReadToEndAsync();

                dynamic obj = new DynamicDictionary();
                obj = JsonConvert.DeserializeAnonymousType<DynamicDictionary>(headerFormat, obj);
                Console.WriteLine("According to your \"default-header.json\" file you need {0} arguments. Here there are:\n", obj.Count);

                
                foreach (var item in obj.ToList())
                {
                    if(!item.Value.Contains("{"))
                        Console.Write("{0}: //default is '{1}': ", item.Key, item.Value);
                    else
                        Console.Write("{0}: ", item.Key);
                    
                    var readValue = Console.ReadLine();

                    if (readValue != string.Empty)
                        obj.InnerDictionary[item.Key] = readValue;
                    else
                    {
                        if (item.Value != "DateTime.Now")
                        {
                            Console.Write("Left default value: {0} for {1}\n", item.Value, item.Key);
                        }
                        else
                        {
                            obj.InnerDictionary[item.Key] = DateTime.Now;
                        }
                    }
                }
                Console.ReadLine();
            }
        }
    }
}
