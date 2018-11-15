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
        public bool root;

        //List of subfolders in this folder
        public List<FolderData> subFolders = new List<FolderData>();

       
        //list of the files for the UI
        public List<FileInfo> files = new List<FileInfo>();

        //for quick accessions by the UI
        public Dictionary<string, FolderData> sfDict = new Dictionary<string, FolderData>();
        public Dictionary<string, FileInfo> fDict = new Dictionary<string, FileInfo>();

        //subfolders as non-tree to prevent explosion when instantiating one of these
        public DirectoryInfo[] sfdi;

        /// <summary>
        /// Constructor for a FolderData
        /// </summary>
        /// <param name="inpath">Path to the folder on disk</param>
        /// <param name="r">Whether this FolderData is your root folder. Defaults to false.</param>
        public FolderData(string inpath, bool r = false)
        {
            root = r;
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

       /// <summary>
       /// Gets the total size of the non-folder items in the immediate folder (not including subfolders)
       /// Updates the num_items and total_size properties
       /// </summary>
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
            catch (System.UnauthorizedAccessException)
            {
                //record the error somewhere
            }
        }

        /// <summary>
        /// Recursively determines the total size of the folder, including subfolders
        /// </summary>
        /// <param name="callback">Bool returning function that takes a double 0-1. Pass this if you want progress updates (i.e. doing the size asyncronously)</param>
        public void size(Func<FolderData, double, bool> callback = null)
        {
            sfDict = new Dictionary<string, FolderData>();
            subFolders = new List<FolderData>();

            if (sfdi == null)
            {
                return;
            }
            total_size = 0;
            files_size = 0;
            num_items = 0;
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
            //if the target folder has no subfolders
            if (prog == 0 && callback != null)
            {
                callback(this, 1);
            }
        }
        
        /// <summary>
        /// Determines if a folder is in this folder, including subfolders
        /// </summary>
        /// <param name="path">Fully qualified path to search for</param>
        /// <returns>the FolderData object, or null if it does not exist</returns>
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

        /// <summary>
        /// ToString override. Returns the name property
        /// </summary>
        /// <returns>The name property</returns>
        public override string ToString()
        {
            return this.path.Name;
        }
    }
}
