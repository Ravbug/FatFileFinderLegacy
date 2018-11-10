using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FatFileFinder
{
    public class FolderData
    {
        //path to this folder
        public DirectoryInfo path;

        //size of the non-directory items
        public double files_size = 0;
        public int num_items  = 0;
        public double total_size = 0;

        //List of subfolders in this folder
        public List<FolderData> subFolders = new List<FolderData>();

       
        //list of the files for the UI
        public List<FileInfo> files = new List<FileInfo>();

        //for quick accessions by the UI
        public Dictionary<string, FolderData> sfDict = new Dictionary<string, FolderData>();
        public Dictionary<string, FileInfo> fDict = new Dictionary<string, FileInfo>();

        //subfolders as non-tree to prevent explosion when instantiating one of these
        public DirectoryInfo[] sfdi;

        //constructor
        public FolderData(string inpath)
        {  
            try
            {
                path = new DirectoryInfo(inpath);
                sfdi = path.GetDirectories();
            }
            catch(Exception e)
            {
                //record the error somewhere
            }
        }

        //get the size of non-directory items in this folder
        void size_files()
        {
            num_items = 0;
            files = new List<FileInfo>();
            fDict = new Dictionary<string, FileInfo>();
            // Get the files in the directory and record info about them
            try
            {
                System.IO.FileInfo[] fileNames = path.GetFiles("*.*");

                foreach (System.IO.FileInfo fi in fileNames)
                {
                    files_size += fi.Length;
                    num_items++;
                    files.Add(fi);
                    fDict.Add(fi.Name,fi);
                }
                total_size = files_size;
            }
            catch (System.UnauthorizedAccessException e)
            {
                //record the error somewhere
            }
        }

        //size this folder and all subfolders, and update the fields
        public void size(Func<FolderData, double, bool> callback = null)
        {
            sfDict = new Dictionary<string, FolderData>();
            subFolders = new List<FolderData>();

            if (sfdi == null)
            {
                return;
            }
            size_files();


            //build the list of subfolders by sizing each one
            double prog = 0;
            foreach (DirectoryInfo d in sfdi)
            {
                FolderData t = new FolderData(d.FullName);
                t.size();
                total_size += t.total_size;
                num_items += t.num_items;
                subFolders.Add(t);
                sfDict.Add(t.path.Name,t);

                //call the callback
                if (callback != null)
                {
                    prog++;
                    callback(this, prog / sfdi.Count());
                }
            }
        }
        
        /* Atempts to find the FolderData object specified in this folder
         */ 
        public FolderData find(string path)
        {
            FolderData root = this;
            string[] components = path.Split(Path.DirectorySeparatorChar);
            for (int i = 1; i < components.Length; i++)
            {
                string component = components[i];
                try
                {
                    root = root.sfDict[component];
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return root;
        }

        public override string ToString()
        {
            return this.path.Name;
        }
    }
}
