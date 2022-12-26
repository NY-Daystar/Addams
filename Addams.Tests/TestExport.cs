﻿using Addams.Api;
using Addams.Entities;
using Addams.Exceptions;
using NUnit.Framework;
using Addams.Export;

namespace Addams.Tests
{
    public class TestsExport
    {
        // TODO 
        [Test]
        public void TestSavePlaylistWithInvalidFilename()
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location ?? throw new Exception("exe path is null");

            string exeFolder = Path.GetDirectoryName(exePath) ?? throw new Exception("exe folder path is null");

            string wDir = Path.Combine(exeFolder, "data");
            if (!Directory.Exists(wDir))
                Directory.CreateDirectory(wDir);
            SpotifyExport.SavePlaylist(wDir, new Models.Playlist
            {
                Name = "Chillhop Radio 🐾 jazz/lofi hip hop beats to study/relax to | Study Music | Chillhop Music 2022"
                ///
                /// System.IO.DirectoryNotFoundException: 'Could not find a part of the path 'D:\Dev\Addams\Addams\bin\Debug\net7.0\data\'.'
                ///,
            });

            // TODO assert si le fichier existe
        }

    }
}