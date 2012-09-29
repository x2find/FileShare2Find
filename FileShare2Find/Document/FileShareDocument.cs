using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using EPiServer.Find;
using EPiServer.Find.Helpers;

namespace FileShareToFind.Document
{
    public class FileShareDocument
    {
        public Attachment Attachment { get; set; }
        public string Name { get; set; }
        public List<string> ACL { get; set; }
        public DateTime CreationTime { get; set; }
        [Id]
        public string NameHashed { get; set; }

        public FileShareDocument(){}

        public FileShareDocument(string name)
        {
            Name = name;
            NameHashed = HashName(name);
        }

        public FileShareDocument(string name, string path)
        {
            Attachment = new FileAttachment(path);
            var fileInfo = new FileInfo(path);
            CreationTime = fileInfo.CreationTime;
            SetACLForFile(fileInfo);
            Name = name;
            NameHashed = HashName(name);
        }

        //Hash name and use it as Id so we can remove it later.
        private string HashName(string name)
        {
            MD5CryptoServiceProvider cryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(name);
            byte[] hashBytes = cryptoServiceProvider.ComputeHash(bytes);
            // Make base64 url-safe
            String hash = Convert.ToBase64String(hashBytes).Replace('+', '-').Replace('/', '_');
            return hash;
        }

        //Add the ACL for the file so it can be filered on later when searching.
        private void SetACLForFile(FileInfo fileInfo)
        {
            ACL = new List<string>();
            var accessRules = fileInfo.GetAccessControl().GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
            foreach (FileSystemAccessRule fileRule in accessRules)
            {
                ACL.Add(fileRule.IdentityReference.Value);
            }
        }
    }
}
