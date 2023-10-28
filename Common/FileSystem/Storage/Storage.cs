namespace Common.FileSystem.Storage
{
    public static class Storage
    {
        const string root = "Resource";
        public static void SaveFile(mFile file)
        {
            using (FileStream fs = File.Create(Path.Combine(root, file.GetFileName())))
                fs.Write(file.GetData());
        }
        public static mFile GetFile(string filename) 
            => new mFile(filename, File.ReadAllBytes(Path.Combine(root, filename)));
        
    }
}
