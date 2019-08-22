using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.File;

namespace pro_web_a.Helpers
{
    public class FileHandler
    {
        private readonly string _storageName = WebConfigurationManager.AppSettings["FileStorageName"];
        private readonly string _storageConnectionString = WebConfigurationManager.AppSettings["FileStorageConnectionString"];
        private static CloudStorageAccount _storageAccount;
        private static CloudBlobContainer _cloudBlobContainer;

        public static CloudStorageAccount StorageAccount
        {
            get
            {
                if (_storageAccount == null)
                    new FileHandler();
                return _storageAccount;

            }
        }

        private FileHandler()
        {
            _storageAccount = new CloudStorageAccount(new StorageCredentials(_storageName,_storageConnectionString),true);
        }

        public  static void CreateFileShared()
        {
            var cloudFileShare = StorageAccount.CreateCloudBlobClient();
             _cloudBlobContainer = cloudFileShare.GetContainerReference(WebConfigurationManager.AppSettings["BlobFolder"]);
             if(!_cloudBlobContainer.Exists())
                _cloudBlobContainer.Create();
        }

        public static void WriteToFile(string path, string textToWrite)
        {
            var parentFolder = _cloudBlobContainer.GetAppendBlobReference(path);
            if(!parentFolder.Exists())
                parentFolder.CreateOrReplace();
            parentFolder.AppendText(textToWrite);
        }

        public static void WriteToFile(string p0)
        {
            const string dd = @"D:\MIT\L3S1\log.txt";
            p0 += "\n";
            File.AppendAllText(dd, p0);
        }
    }
}