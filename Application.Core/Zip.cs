using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using java.util;
using java.util.zip;
using java.io;
using System.IO;

namespace NCC.ClearView.Application.Core
{
    public class Zip
    {
        public void CreateZipFile(string filename, string[] items)
        {
            FileOutputStream fout = new FileOutputStream(filename);
            ZipOutputStream zout = new ZipOutputStream(fout);
            zout.close();
            ZipFile zipfile = new ZipFile(filename);
            AddEntries(zipfile, items);
        }

        public static void AddToZipFile(string filename, string[] items)
        {
            ZipFile file = new ZipFile(filename);
            AddEntries(file, items);
        }

        public static void RemoveFromZipFile(string filename, string[] items)
        {
            ZipFile file = new ZipFile(filename);
            RemoveEntries(file, items);
        }

        public void ExtractZipFile(string zipfilename, string destination)
        {
            ZipFile zipfile = new ZipFile(zipfilename);
            List<ZipEntry> entries = GetZippedItems(zipfile);
            foreach (ZipEntry entry in entries)
            {
                if (!entry.isDirectory())
                {
                    InputStream s = zipfile.getInputStream(entry);
                    try
                    {
                        string fname = System.IO.Path.GetFileName(entry.getName());
                        string dir = System.IO.Path.GetDirectoryName(entry.getName());
                        string newpath = destination + @"\" + dir;

                        System.IO.Directory.CreateDirectory(newpath);

                        FileOutputStream dest = new FileOutputStream(System.IO.Path.Combine(newpath, fname));
                        try
                        {
                            CopyStream(s, dest);
                        }
                        finally
                        {
                            dest.close();
                        }
                    }
                    finally
                    {
                        s.close();
                    }
                }

            }
        }

        public string[] GetZippedFileNames(string zipfilename)
        {
            ZipFile file = new ZipFile(zipfilename);
            List<ZipEntry> entries = GetZippedItems(file);
            List<string> itemNames = new List<string>();
            foreach (ZipEntry entry in entries)
            {
                itemNames.Add(entry.getName());
            }
            return itemNames.ToArray();
        }

        private static void CopyStream(InputStream source, OutputStream destination)
        {
            sbyte[] buffer = new sbyte[8000];
            int data;
            while (true)
            {
                try
                {
                    data = source.read(buffer, 0, buffer.Length);
                    if (data > 0)
                    {
                        destination.write(buffer, 0, data);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        private static void AddEntries(ZipFile file, string[] newFiles)
        {
            string fileName = file.getName();
            string tempFileName = Path.GetTempFileName();
            ZipOutputStream destination = new ZipOutputStream(new FileOutputStream(tempFileName));
            try
            {
                CopyEntries(file, destination);
                if (newFiles != null)
                {
                    foreach (string f in newFiles)
                    {
                        ZipEntry z = new ZipEntry(f.Remove(0, Path.GetPathRoot(f).Length));
                        z.setMethod(ZipEntry.DEFLATED);
                        destination.putNextEntry(z);
                        try
                        {
                            FileInputStream s = new FileInputStream(f);
                            try
                            {
                                CopyStream(s, destination);
                            }
                            finally
                            {
                                s.close();
                            }
                        }
                        finally
                        {
                            destination.closeEntry();
                        }
                    }
                }
            }
            finally
            {
                destination.close();
            }
            file.close();

            System.IO.File.Copy(tempFileName, fileName, true);
            System.IO.File.Delete(tempFileName);

        }

        private static void RemoveEntries(ZipFile file, string[] items)
        {
            string fileName = file.getName();
            string tempFileName = Path.GetTempFileName();
            ZipOutputStream destination = new ZipOutputStream(new FileOutputStream(tempFileName));
            try
            {
                List<ZipEntry> allItems = GetZippedItems(file);
                List<string> filteredItems = new List<string>();
                foreach (ZipEntry entry in allItems)
                {
                    bool found = false;
                    foreach (string s in items)
                    {
                        if (s != entry.getName())
                        {
                            found = true;
                        }
                    }
                    if (found)
                    {
                        filteredItems.Add(entry.getName());
                    }
                }

                CopyEntries(file, destination, filteredItems.ToArray());
            }
            finally
            {
                destination.close();
            }
            file.close();

            System.IO.File.Copy(tempFileName, fileName, true);
            System.IO.File.Delete(tempFileName);

        }

        private static List<ZipEntry> GetZippedItems(ZipFile file)
        {
            List<ZipEntry> entries = new List<ZipEntry>();
            Enumeration e = file.entries();
            while (true)
            {
                if (e.hasMoreElements())
                {
                    ZipEntry entry = (ZipEntry)e.nextElement();
                    entries.Add(entry);
                }
                else
                {
                    break;
                }
            }
            return entries;
        }

        private static void CopyEntries(ZipFile source, ZipOutputStream destination)
        {
            List<ZipEntry> entries = GetZippedItems(source);
            foreach (ZipEntry entry in entries)
            {
                InputStream s = source.getInputStream(entry);
                destination.putNextEntry(entry);
                CopyStream(s, destination);
                destination.closeEntry();
                s.close();
            }
        }

        private static void CopyEntries(ZipFile source, ZipOutputStream destination, string[] entryNames)
        {
            List<ZipEntry> entries = GetZippedItems(source);

            for (int i = 0; i < entryNames.Length; i++)
            {
                foreach (ZipEntry entry in entries)
                {
                    if (entry.getName() == entryNames[i])
                    {
                        InputStream s = source.getInputStream(entry);
                        destination.putNextEntry(entry);
                        CopyStream(s, destination);
                        destination.closeEntry();
                        s.close();
                    }
                }
            }

        }
    }
}
