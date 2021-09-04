using System;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;
using System.Text;
using System.IO.Compression;
using Infoline.Framework.Database;
using System.Linq;

namespace Infoline.Framework.Helper
{
    public static class Zip
    {

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] GZip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }
                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }
                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }


        public static ResultStatus Created(string zipFile, string[] files, bool delfile = false)
        {
            try
            {

                if (File.Exists(zipFile)) File.Delete(zipFile);
                var type = ArchiveType.Zip;
                var compressionType = CompressionType.Deflate;

                switch (Path.GetExtension(zipFile))
                {

                    case ".zip": //ok
                        type = ArchiveType.Zip;
                        compressionType = CompressionType.Deflate;
                        break;

                    case ".tar": // ok
                        type = ArchiveType.Tar;
                        compressionType = CompressionType.BZip2;
                        break;

                    case ".gz": //ok
                        type = ArchiveType.GZip;
                        compressionType = CompressionType.GZip;
                        break;
                }

                using (Stream stream = File.Open(zipFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var writer = WriterFactory.Open(stream, type, compressionType))
                    {
                        foreach (var file in files)
                        {
                            writer.Write(Path.GetFileName(file), file);
                        }
                    }
                    stream.Flush();
                }
                if (delfile)
                {
                    foreach (var file in files)
                    {
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultStatus { result = false, message = ex.Message };
            }
            return new ResultStatus { result = true, message = "işlem başarılı" };
        }

        public static ResultStatus Create(string zipFile, string directory, bool delDirectory = false)
        {

            try
            {

                ZipFile.CreateFromDirectory(directory, zipFile, CompressionLevel.Optimal, false);

                if (delDirectory)
                {
                    foreach (var file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories).Where(a => a != zipFile).ToArray())
                    {
                        File.Delete(file);
                    }
                }

                return new ResultStatus { result = true, message = "işlem başarılı bir şekilde gerçekleşti." };

            }
            catch (Exception ex)
            {
                return new ResultStatus { result = false, message = ex.Message };
            }

        }

        public static ResultStatus Created(string zipFile, string directory, bool delDirectory = false)
        {

            try
            {
                if (File.Exists(zipFile)) File.Delete(zipFile);
                var type = ArchiveType.Zip;
                var compressionType = CompressionType.Deflate;

                switch (Path.GetExtension(zipFile))
                {

                    case ".zip": //ok
                        type = ArchiveType.Zip;
                        compressionType = CompressionType.Deflate;
                        break;

                    case ".tar": // ok
                        type = ArchiveType.Tar;
                        compressionType = CompressionType.BZip2;
                        break;

                    case ".gz": //ok
                        type = ArchiveType.GZip;
                        compressionType = CompressionType.GZip;
                        break;
                }


                var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
                using (Stream stream = File.Open(zipFile, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    files = files.Where(a => a != zipFile).ToArray();
                    using (var writer = WriterFactory.Open(stream, type, compressionType))
                    {
                        foreach (var file in files)
                        {
                            writer.Write(Path.GetFileName(file), file);
                        }
                    }
                    stream.Flush();
                }
                if (delDirectory)
                {
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultStatus { result = false, message = ex.Message };
            }
            return new ResultStatus { result = true, message = "işlem başarılı bir şekilde gerçekleşti." };
        }



        public static bool OpenZip(string zipFile, string outdir = null)
        {
            if (!File.Exists(zipFile)) return false;
            if (outdir == null || Directory.Exists(zipFile))
            {
                outdir = Path.GetDirectoryName(zipFile);
            }

            using (Stream stream = File.OpenRead(zipFile))
            using (IReader reader = ReaderFactory.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        var c = new ExtractionOptions();
                        c.ExtractFullPath = true;
                        c.Overwrite = true;
                        reader.WriteEntryToDirectory(outdir, c);
                    }
                }

            }

            return true;
        }



    }
}
