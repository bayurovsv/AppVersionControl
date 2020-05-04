using AppVersionControl.Domain.ApplicationVersion;
using AppVersionControl.Domain.Interfaces;
using AppVersionControl.Domain.Iterfaces;
using AppVersionControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleTests
{
    class Program
    {
        // подумать над реализацей IDictionary<FileOfVersion, byte[]> (метаданные и содержимое файла)
        static void Main(string[] args)
        {
            #region test
            string path = @"C:\Users\PC\source\repos\ConsoleApp1";
            IAssemblyAppRepo assemblyApp = new AssemblyAppRepo(path);
            IDictionary<FileOfVersion, byte[]> fileContents = assemblyApp.FindFileOfVersionsWithData();
            IList<FileOfVersion> files = assemblyApp.FindFileOfVersions();
            AppVersion versTest = new AppVersion("TestApp", new VersionNumber(1, 2), files);
            //IOneVersionSaveRepo repoF3 = new SaveOneVersionRepo(@"C:\sss\");
            //IFileRepo repoF2 = new FileInSystemRepo(@"C:\");
            IFileRepo repoDBF1 = new FileInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
            //var isSuccesDB = repoDBF1.Save(versTest, fileContents);
            //IVersionSaveRepo saveRepo = new VersionSaveInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
            //var isSucces2 = saveRepo.SaveApplication(versTest);
            IDictionary<FileOfVersion, byte[]> fileContentsInDB = repoDBF1.FindFileOfVersionsWithData(versTest);
            #endregion
            #region test file system
            //IVersionRepo repoV1 = new VersionInFileSystemRepo(Directory.GetCurrentDirectory());
            //AppVersion srcV = repoV1.Find("TestApp");
            //IFileRepo repoF1 = new FileInSystemRepo(AppDomain.CurrentDomain.BaseDirectory);
            //IDictionary<FileOfVersion, byte[]> fileContents = repoF1.FindFileOfVersionsWithData(srcV);
            //IFileRepo repoF2 = new FileInSystemRepo(@"C:\");
            //var isSucces = repoF2.Save(srcV, fileContents);
            //IOneVersionSaveRepo repoF3 = new SaveOneVersionRepo(@"C:\www\");
            //var isSucces2 = repoF3.SaveOneVersionApplication(srcV, fileContents);
            #endregion

            #region test DB
            //IVersionRepo repoDBV1 = new VersionInDBRepo(Directory.GetCurrentDirectory());
            //AppVersion srcDBV = repoDBV1.Find("TestApp");
            //IFileRepo repoDBF1 = new FileInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
            //IDictionary<FileOfVersion, byte[]> fileContentsInDB = repoDBF1.FindFileOfVersionsWithData(srcDBV);
            //var isSuccesDB = repoDBF1.Save(srcDBV, fileContentsInDB);
            #endregion

            #region file system
            //VersionInFileSystemRepo repo_file = new VersionInFileSystemRepo(AppDomain.CurrentDomain.BaseDirectory);
            //AppVersion app = repo_file.Find("TestApp");
            //VersionSaveInFileSystemRepo repo_filesave = new VersionSaveInFileSystemRepo(AppDomain.CurrentDomain.BaseDirectory);
            //var save = repo_filesave.SaveApplication(app);
            //var versionfile_Find = repo_file.Find("TestApp");
            //var versionfile_Find2 = repo_file.Find(app);
            //var versionfile_FindAll = repo_file.FindAll("TestApp");
            //var versionfile_FindAll2 = repo_file.FindAll();
            //FileInSystemRepo repo_file2 = new FileInSystemRepo(AppDomain.CurrentDomain.BaseDirectory);
            //var fileinSys_FindByte = repo_file2.FindByteFileOfVersions(app);
            //var fileinSys_FindFile = repo_file2.FindFileOfVersions(app);
            //var fileinSys_Dictionary = repo_file2.FindFileOfVersionsWithData(app);
            #endregion

            #region Db
            //VersionInDBRepo repo = new VersionInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
            //VersionNumber versTest = new VersionNumber(1, 0);
            //AppVersion app1 = new AppVersion("TestApp", versTest);
            //var verinDB_Find = repo.Find("TestApp");
            //var verinDB_Find2 = repo.Find(app1);
            //var verinDB_FindAll = repo.FindAll("TestApp");
            //var verinDB_FindAll2 = repo.FindAll();
            //FileInDBRepo repo1 = new FileInDBRepo(AppDomain.CurrentDomain.BaseDirectory);
            //var fileinDB_FindByte = repo1.FindByteFileOfVersions(app1);
            //var fileinDB_FindFile = repo1.FindFileOfVersions(app1);
            //var fileinDb_Dictionary = repo1.FindFileOfVersionsWithData(app1);
            #endregion

            Console.ReadKey();
        }

    }
}
